using redd096;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using System.Collections;

/// <summary>
/// This manage the level: show customers or events, save player's choice and so on
/// </summary>
public class LevelManager : SimpleInstance<LevelManager>
{
    [Header("Level")]
    [SerializeField] LevelData levelData;
    [SerializeField] FDate currentDate;

    [Header("Managers")] 
    [SerializeField] LevelEventsManager eventsManager;
    [SerializeField] CheckPlayerChoiceManager choiceManager;
    [SerializeField] ResidentsManager residentsManager;
    [SerializeField] AppointmentsManager appointmentsManager;
    [SerializeField] JournalManager journalManager;

    //current vars in this LevelNode
    private LevelNodeData currentNode;
    private Customer currentCustomer;                   //if this node is a customer, save customer values
    private CustomerBehaviour currentCustomerInstance;  //if this node is a customer, save customer instance in scene
    private bool currentChoice;         //user allowed customer to ENTER (true) or NOT ENTER (false)
    private bool alreadySetChoice;      //this is used to save result only first time. If user put other stamps, they're ignored
    private bool canCustomerEnter;      //false if waiting player to press the Bell, then become true and currentCustomer is instantiated

    //used by nodes SaveChoice and GetChoice
    private Dictionary<string, bool> savedChoices = new Dictionary<string, bool>();
    //used to show images on Newspaper
    private List<ResidentData> residentsKilledThisDay = new List<ResidentData>();
    private List<ResidentData> residentsArrestedThisDay = new List<ResidentData>();

    //warnings
    private int warningsCounter;

    //used by editor
    public LevelNodeData CurrentNode => currentNode;

    protected override void InitializeInstance()
    {
        base.InitializeInstance();

        //add events
        DeskManager.instance.onClickBell += OnPlayerClickBell;
        DeskManager.instance.onDocumentReceiveStamp += OnDocumentReceiveStamp;
        DeskManager.instance.onGiveBackAllDocuments += OnGiveBackAllDocuments;

        //initialize
        choiceManager.InitializeForThisLevel(currentDate, residentsManager, appointmentsManager);
        journalManager.InitializeForThisLevel(currentDate, choiceManager, residentsManager, appointmentsManager);

        //set every interactable in scene
        InteractableBase[] interactablesInScene = FindObjectsOfType<InteractableBase>();
        foreach (var interactable in interactablesInScene)
            interactable.SetInteractable(true);

        //not the bell
        DeskManager.instance.SetBellInteractable(false);

        //start first node
        CheckNextNode();
    }

    private void OnDestroy()
    {
        //remove events
        if (DeskManager.instance)
        {
            DeskManager.instance.onClickBell -= OnPlayerClickBell;
            DeskManager.instance.onDocumentReceiveStamp -= OnDocumentReceiveStamp;
            DeskManager.instance.onGiveBackAllDocuments -= OnGiveBackAllDocuments;
        }
    }

    #region registered events

    /// <summary>
    /// Player click bell to make enter next customer
    /// </summary>
    private void OnPlayerClickBell()
    {
        canCustomerEnter = true;
    }

    /// <summary>
    /// When user set stamp on a document
    /// </summary>
    /// <param name="choice"></param>
    private void OnDocumentReceiveStamp(bool choice)
    {
        //only one time
        if (alreadySetChoice == false)
        {
            alreadySetChoice = true;
            currentChoice = choice;
        }
    }

    /// <summary>
    /// When user gave back every document, customer go away and enter new one
    /// </summary>
    private void OnGiveBackAllDocuments()
    {
        //reset vars for next turn, player can set again stamp
        alreadySetChoice = false;

        //start coroutine
        StartCoroutine(OnGiveBackAllDocumentsCoroutine());
    }

    private IEnumerator OnGiveBackAllDocumentsCoroutine()
    {
        //start Leave dialogue
        string dialogue = currentChoice ? currentCustomer.DialogueWhenPlayerSayYes : currentCustomer.DialogueWhenPlayerSayNo;
        yield return LevelUtilities.instance.WaitDialogue(dialogue);

        //move customer away from screen
        Sequence sequence = LevelUtilities.instance.MoveCustomerAwayFromScreen(currentCustomerInstance, currentCustomer);

        //check if player did something wrong
        //TEMP - do this only if not a ghost, because we have some customer that are ghosts. It's not important what user says, they just fade away
        //probably is better to have an "event node" just for ghosts
        if (currentCustomer.GoAwayLikeGhost == false)
        {
            bool correctChoice = choiceManager.CheckPlayerChoice(currentNode, currentChoice, out string problem);
            sequence.ChainCallback(() => OnPlayerCheckChoice(correctChoice, problem));
        }
        
        //move to next node
        sequence.ChainCallback(CheckNextNode);
    }

    #endregion

    /// <summary>
    /// Give player a warning if player lets customer enter, but customer has wrong documents. Or viceversa
    /// </summary>
    private void OnPlayerCheckChoice(bool isCorrectChoice, string problem)
    {
        if (isCorrectChoice)
            return;

        //increase warnings counter
        warningsCounter++;

        Sequence sequence = Sequence.Create();
        sequence.ChainDelay(2.5f);
        sequence.ChainCallback(() => DeskManager.instance.InstantiateWarning(warningsCounter, problem));
    }

    #region check next node

    /// <summary>
    /// Find first or next node and execute it
    /// </summary>
    private void CheckNextNode()
    {
        //first node or next one
        if (currentNode == null)
            currentNode = levelData.Nodes[0];
        else
            currentNode = currentChoice ? currentNode.NodeOnTrue : currentNode.NodeOnFalse;

        //finish level
        if (currentNode == null)
        {
            Debug.LogWarning("Level finished!");
            return;
        }

        //check node
        if (currentNode is CustomerData customerData)
        {
            StartCoroutine(CheckCustomer(customerData.Customer));
        }
        else if (currentNode is SaveChoiceData saveChoiceData)
        {
            CheckSaveChoice(saveChoiceData.SaveChoice);
        }
        else if (currentNode is GetChoiceData getChoiceData)
        {
            CheckGetChoice(getChoiceData.GetChoice);
        }
        //and events
        else if (currentNode is EventNewspaperData eventNewspaperData)
        {
            CheckEventNewspaper(eventNewspaperData.EventNewspaper);
        }
        else if (currentNode is EventKillResidentData eventKillResidentData)
        {
            CheckEventKillResident(eventKillResidentData.EventKillResident);
        }
        else if (currentNode is EventStartBackgroundAnimationData eventStartBackgroundAnimationData)
        {
            CheckEventStartBackgroundAnimation(eventStartBackgroundAnimationData.EventStartBackgroundAnimation);
        }
        else if (currentNode is EventStopBackgroundAnimationData eventStopBackgroundAnimationData)
        {
            CheckEventStopBackgroundAnimation(eventStopBackgroundAnimationData.EventStopBackgroundAnimation);
        }
        else if (currentNode is EventBloodData eventBloodData)
        {
            CheckEventBlood(eventBloodData.EventBlood);
        }
        else if (currentNode is EventStartRedEventData)
        {
            CheckEventRedEvent(true);
        }
        else if (currentNode is EventStopRedEventData)
        {
            CheckEventRedEvent(false);
        }
        else if (currentNode is EventArrestEndOfDayData eventArrestEndOfDayData)
        {
            StartCoroutine(CheckEventArrestEndOfDay(eventArrestEndOfDayData.EventArrestEndOfDay));
        }
        //other
        else if (currentNode is IsResidentAliveData isResidentAliveData)
        {
            CheckIsResidentAlive(isResidentAliveData.IsResidentAlive);
        }
        else if (currentNode is DelayForSecondsData delayForSecondsData)
        {
            CheckDelayForSeconds(delayForSecondsData.DelayForSeconds);
        }
        else if (currentNode is DebugLogTextData debugLogTextData)
        {
            CheckDebugLogText(debugLogTextData.DebugLogText);
        }
        //or error
        else
        {
            Debug.LogError($"There is nothing to do with this type of node: [{currentNode.GetType()}]. Go to next node");
            CheckNextNode();
        }
    }

    IEnumerator CheckCustomer(Customer customer)
    {
        //press the bell
        //TEMP - only if customer give something to player, because we have some customer that enter, tell something, and go away
        //probably is better to have an "event node" just for people who enter and say things or give objects to player and move away
        if (customer.GiveIDCard || customer.GiveRenunciationCard || customer.GiveResidentCard || customer.GivePoliceCard)
        {
            //set bell interactable and wait player to press it
            DeskManager.instance.SetBellInteractable(true);
            canCustomerEnter = false;
            yield return new WaitUntil(() => canCustomerEnter);

            //reset bell
            DeskManager.instance.SetBellInteractable(false);
            canCustomerEnter = false;
        }

        //instantiate customer
        currentCustomerInstance = LevelUtilities.instance.InstantiateCustomer(customer);
        currentCustomer = customer;

        //move customer inside the screen and start dialogue
        yield return LevelUtilities.instance.MoveCustomer(currentCustomerInstance, enterInScene: true).ToYieldInstruction();
        yield return LevelUtilities.instance.WaitDialogue(customer.DialogueWhenCome);

        //then give documents
        CheckCustomerGiveDocuments(customer);
    }

    void CheckCustomerGiveDocuments(Customer customer)
    {
        bool giveDocuments = false;

        Sequence sequence = Sequence.Create();

        //give documents
        if (customer.GiveIDCard)
        {
            sequence.ChainCallback(() => DeskManager.instance.InstantiateDocument(customer.IDCard));
            sequence.ChainDelay(0.2f);
            giveDocuments = true;
        }
        if (customer.GiveRenunciationCard)
        {
            sequence.ChainCallback(() => DeskManager.instance.InstantiateDocument(customer.RenunciationCard));
            sequence.ChainDelay(0.2f);
            giveDocuments = true;
        }
        if (customer.GiveResidentCard)
        {
            sequence.ChainCallback(() => DeskManager.instance.InstantiateDocument(customer.ResidentCard));
            sequence.ChainDelay(0.2f);
            giveDocuments = true;
        }
        if (customer.GivePoliceCard)
        {
            sequence.ChainCallback(() => DeskManager.instance.InstantiateDocument(customer.PoliceCard));
            sequence.ChainDelay(0.2f);
            giveDocuments = true;
        }
        if (customer.GiveAppointmentCard)
        {
            sequence.ChainCallback(() => DeskManager.instance.InstantiateDocument(customer.AppointmentCard));
            sequence.ChainDelay(0.2f);
            giveDocuments = true;
        }

        //TEMP - if customer doesn't give documents, just move customer away (this isn't a customer to check. We have some customer that enter, tell something, and go away)
        //probably is better to have an "event node" just for people who enter and say things or give objects to player and move away
        if (giveDocuments == false)
        {
            sequence.Chain(LevelUtilities.instance.MoveCustomerAwayFromScreen(currentCustomerInstance, currentCustomer));
            sequence.ChainCallback(CheckNextNode);
        }
    }

    void CheckSaveChoice(SaveChoice saveChoice)
    {
        //save choice and go to next node
        savedChoices[saveChoice.VariableName] = currentChoice;
        CheckNextNode();
    }

    void CheckGetChoice(GetChoice getChoice)
    {
        string variableName = getChoice.VariableName;

        //find saved variable and set choice for next node
        if (savedChoices.ContainsKey(variableName))
            currentChoice = savedChoices[variableName];
        else
            Debug.LogError($"There isn't a choice saved with this name: [{variableName}]. Keep current choice and go to next node");

        //go to next node
        CheckNextNode();
    }

    void CheckEventNewspaper(EventNewspaper eventNewspaper)
    {
        //show newspaper for few seconds, then go to next node
        eventsManager.ShowNewspaper(eventNewspaper.NewspaperPrefab, residentsKilledThisDay)
            .OnComplete(CheckNextNode);
    }

    void CheckEventKillResident(EventKillResident eventKillResident)
    {
        //remove resident from the list, and save it
        if (eventKillResident.KillRandom)
            residentsKilledThisDay.Add(residentsManager.KillRandomResident());
        else
            residentsKilledThisDay.Add(residentsManager.KillResident(eventKillResident.SpecificResident));

        //go to next node
        CheckNextNode();
    }

    void CheckEventStartBackgroundAnimation(EventStartBackgroundAnimation eventStartBackgroundAnimation)
    {
        Sequence sequence = Sequence.Create();

        //play background animation
        sequence.ChainCallback(() => eventsManager.PlayBackgroundEvent(eventStartBackgroundAnimation.AnimationToPlay));

        //wait animation
        if (eventStartBackgroundAnimation.WaitAnimation)
            sequence.ChainDelay(eventStartBackgroundAnimation.AnimationToPlay.length);

        //delay, then move to next node
        sequence.ChainDelay(eventStartBackgroundAnimation.DelayAfterAnimation);
        sequence.ChainCallback(CheckNextNode);
    }

    void CheckEventStopBackgroundAnimation(EventStopBackgroundAnimation eventStopBackgroundAnimation)
    {
        Sequence sequence = Sequence.Create();

        //stop background animation
        sequence.ChainCallback(eventsManager.StopBackgroundEvent);

        //delay, then move to next node
        sequence.ChainDelay(eventStopBackgroundAnimation.DelayBeforeNextNode);
        sequence.ChainCallback(CheckNextNode);
    }

    void CheckEventBlood(EventBlood eventBlood)
    {
        //show event blood, then go to next node
        eventsManager.PlayBloodEvent(eventBlood.BackgroundAnimation)
            .OnComplete(CheckNextNode);
    }

    void CheckEventRedEvent(bool play)
    {
        //play or stop red event and go to next node
        eventsManager.RedEvent(play);
        CheckNextNode();
    }

    void CheckIsResidentAlive(IsResidentAlive isResidentAlive)
    {
        //check if resident is still in the building and set choice for next node
        bool alive = residentsManager.IsResidentAlive(isResidentAlive.Resident) && residentsManager.IsResidentFree(isResidentAlive.Resident);
        currentChoice = alive;

        //go to next node
        CheckNextNode();
    }

    void CheckDelayForSeconds(DelayForSeconds delayForSeconds)
    {
        Sequence sequence = Sequence.Create();

        //delay
        sequence.ChainDelay(delayForSeconds.Delay);

        //then move to next node
        sequence.ChainCallback(CheckNextNode);
    }

    void CheckDebugLogText(DebugLogText debugLogText)
    {
        //stamp text in console
        Debug.Log(debugLogText.Text);

        //and move to next node
        CheckNextNode();
    }

    IEnumerator CheckEventArrestEndOfDay(EventArrestEndOfDay eventArrestEndOfDay)
    {
        //player select who arrest
        yield return eventsManager.PlayArrestAtTheEndOfDayEvent(eventArrestEndOfDay.CustomerImage, eventArrestEndOfDay.DialogueWhenCome, residentsManager,
            arrestedResidentData =>
            {
                residentsArrestedThisDay.Add(residentsManager.ArrestResident(arrestedResidentData));
            });

        //then move to next node
        CheckNextNode();
    }

    #endregion
}
