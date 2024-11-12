using redd096;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using System.Collections;
using redd096.Attributes;

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

    [Header("Customer")]
    [SerializeField] CustomerBehaviour customerPrefab;
    [SerializeField] Transform customerContainer;
    [SerializeField] Transform customerStartPoint;
    [SerializeField] Transform customerEndPoint;
    [SerializeField] float customerAnimation = 3;

    [Button]
    void SkipDialogue() => DialogueManagerUtilities.PressContinueButton();

    //current vars in this LevelNode
    private LevelNodeData currentNode;
    private Customer currentCustomer;                   //if this node is a customer, save customer values
    private CustomerBehaviour currentCustomerInstance;  //if this node is a customer, save customer instance in scene
    private bool currentChoice;         //user allowed customer to ENTER (true) or NOT ENTER (false)
    private bool alreadySetChoice;      //this is used to save result only first time. If user put other stamps, they're ignored
    private bool canCustomerEnter;      //false if waiting player to press the Bell, then become true and currentCustomer is istantiated

    //used by nodes SaveChoice and GetChoice
    private Dictionary<string, bool> savedChoices = new Dictionary<string, bool>();
    //used by node GetKilledResident
    private ResidentData lastKilledResident;

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

    #region events

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
        yield return WaitDialogue(dialogue);

        //check if player choice is correct
        bool correctChoice = choiceManager.CheckPlayerChoice(currentNode, currentChoice, out string problem);

        Sequence sequence = Sequence.Create();

        //move customer away from screen
        sequence.ChainCallback(currentCustomerInstance.StartWalk);
        sequence.Chain(MoveCustomer(false));
        sequence.ChainCallback(() =>
        {
            //check if player did something wrong
            OnPlayerCheckChoice(correctChoice, problem);

            //and move to next node
            Destroy(currentCustomerInstance.gameObject);
            currentCustomer = null;
            CheckNextNode();
        });
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

    #region private utilities

    /// <summary>
    /// Move customer in front of desk or outside of the screen
    /// </summary>
    /// <param name="enterInScene"></param>
    /// <returns></returns>
    private Tween MoveCustomer(bool enterInScene)
    {
        //move customer (enter in scene, or is leaving the scene)
        Transform startPoint = enterInScene ? customerStartPoint : customerEndPoint;
        Transform endPoint = enterInScene ? customerEndPoint : customerStartPoint;
        return Tween.Position(currentCustomerInstance.transform, startPoint.position, endPoint.position, customerAnimation, Ease.InOutSine);
    }

    /// <summary>
    /// Start a dialogue and wait it to finish
    /// </summary>
    /// <param name="dialogueName"></param>
    /// <returns></returns>
    private IEnumerator WaitDialogue(string dialogueName)
    {
        //be sure there is a dialogue
        if (string.IsNullOrEmpty(dialogueName))
            yield break;

        if (DialogueManagerUtilities.CheckConversationExists(dialogueName) == false)
        {
            Debug.LogError("Missing dialogue: " + dialogueName);
            yield break;
        }

        //register to end dialogue event
        bool isTalking = true;
        DialogueManagerEvents.instance.onConversationEnd += () => isTalking = false;

        //start dialogue
        DialogueManagerUtilities.StartConversation(dialogueName);

        //wait dialogue to finish
        yield return new WaitWhile(() => isTalking);
    }

    #endregion

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
        //or error
        else
        {
            Debug.LogError($"There is nothing to do with this type of node: [{currentNode.GetType()}]. Go to next node");
            CheckNextNode();
        }
    }

    IEnumerator CheckCustomer(Customer customer)
    {
        //TEMP - press the bell only if customer give something to player
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
        currentCustomerInstance = Instantiate(customerPrefab, customerContainer);
        currentCustomerInstance.Init(customer.CustomerImage.ToArray());

        currentCustomer = customer;

        //move customer inside the screen
        currentCustomerInstance.StartWalk();
        yield return MoveCustomer(true).ToYieldInstruction();
        currentCustomerInstance.StopWalk();

        //start dialogue
        yield return WaitDialogue(customer.DialogueWhenCome);

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

        //TEMP - if don't give documents (this isn't a user to check, just come in scene, tell something to player and move away)
        if (giveDocuments == false)
        {
            //move customer away from screen (it's a copy-paste from OnGiveBackAllDocumentsCoroutine, but this doesn't have the check if player did something wrong)
            sequence.ChainCallback(currentCustomerInstance.StartWalk);
            sequence.Chain(MoveCustomer(false));
            sequence.ChainCallback(() =>
            {
                //and move to next node
                Destroy(currentCustomerInstance.gameObject);
                currentCustomer = null;
                CheckNextNode();
            });
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
        eventsManager.ShowNewspaper(eventNewspaper.NewspaperPrefab, lastKilledResident)
            .OnComplete(CheckNextNode);
    }

    void CheckEventKillResident(EventKillResident eventKillResident)
    {
        //remove resident from the list, and save it
        if (eventKillResident.KillRandom)
            lastKilledResident = residentsManager.RemoveRandomResident();
        else
            lastKilledResident = residentsManager.RemoveResident(eventKillResident.SpecificResident);
    }

    #endregion
}
