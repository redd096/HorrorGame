using redd096;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

/// <summary>
/// This manage the level: show customers or events, save player's choice and so on
/// </summary>
public class LevelManager : SimpleInstance<LevelManager>
{
    [SerializeField] LevelData levelData;
    [Space]
    [SerializeField] CustomerBehaviour customerPrefab;
    [SerializeField] Transform customerContainer;
    [SerializeField] Transform customerStartPoint;
    [SerializeField] Transform customerEndPoint;
    [SerializeField] float customerAnimation = 3;

    private LevelNodeData currentNode;
    private bool currentResult;         //user allowed customer to ENTER (true) or NOT ENTER (false)
    private bool alreadySetResult;      //this is used to save result only first time. If user put other stamps, they're ignored
    private CustomerBehaviour currentCustomer;

    //used by nodes SaveChoice and GetChoice
    private Dictionary<string, bool> savedChoices = new Dictionary<string, bool>();

    private void Start()
    {
        CheckNextNode();
    }

    /// <summary>
    /// When user set stamp on a document
    /// </summary>
    /// <param name="result"></param>
    public void OnDocumentReceiveStamp(bool result)
    {
        //only one time
        if (alreadySetResult == false)
        {
            alreadySetResult = true;
            currentResult = result;
        }
    }

    /// <summary>
    /// When user gave back every document, customer go away and enter new one
    /// </summary>
    public void OnGiveBackAllDocuments()
    {
        //reset for next turn, player can set again stamp
        alreadySetResult = false;

        //start end dialogue
        Sequence sequence = Sequence.Create();
        sequence.ChainCallback(() => Debug.Log("Start dialogue"));
        sequence.ChainDelay(2);

        //move customer away from screen
        sequence.ChainCallback(currentCustomer.StartWalk);
        sequence.Chain(MoveCustomer(false));
        sequence.ChainCallback(() => Destroy(currentCustomer.gameObject));

        //check if player did something wrong, then move to next node
        sequence.ChainCallback(() => Debug.Log("Check if player did something wrong"));
        sequence.ChainCallback(CheckNextNode);
    }

    private Tween MoveCustomer(bool enterInScene)
    {
        //move customer (enter in scene, or is leaving the scene)
        Transform startPoint = enterInScene ? customerStartPoint : customerEndPoint;
        Transform endPoint = enterInScene ? customerEndPoint : customerStartPoint;
        return Tween.Position(currentCustomer.transform, startPoint.position, endPoint.position, customerAnimation, Ease.InOutSine);
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
            currentNode = currentResult ? currentNode.NodeOnTrue : currentNode.NodeOnFalse;

        //finish level
        if (currentNode == null)
        {
            Debug.LogWarning("Level finished!");
            return;
        }

        //check node
        if (currentNode is CustomerData customerData)
        {
            CheckCustomer(customerData.Customer);
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
        //or error
        else
        {
            Debug.LogError($"There is nothing to do with this type of node: [{currentNode.GetType()}]. Go to next node");
            CheckNextNode();
        }
    }

    void CheckCustomer(Customer customer)
    {
        //instantiate customer
        currentCustomer = Instantiate(customerPrefab, customerContainer);
        currentCustomer.Init(customer.CustomerImage.ToArray());

        //start move customer
        Sequence sequence = Sequence.Create();
        sequence.ChainCallback(currentCustomer.StartWalk);
        sequence.Chain(MoveCustomer(true));

        //start dialogue
        sequence.ChainCallback(currentCustomer.StopWalk);
        sequence.ChainCallback(() => Debug.Log("Start dialogue"));
        sequence.ChainDelay(2);

        //then give documents
        sequence.ChainCallback(() => CheckCustomerGiveDocuments(customer));
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

        //if don't give documents
        if (giveDocuments == false)
        {
            //move customer away from screen
            sequence.ChainCallback(currentCustomer.StartWalk);
            sequence.Chain(MoveCustomer(false));
            sequence.ChainCallback(() =>
            {
                //and move to next node
                Destroy(currentCustomer.gameObject);
                CheckNextNode();
            });
        }
    }

    void CheckSaveChoice(SaveChoice saveChoice)
    {
        //save choice and go to next node
        savedChoices[saveChoice.VariableName] = currentResult;
        CheckNextNode();
    }

    void CheckGetChoice(GetChoice getChoice)
    {
        string variableName = getChoice.VariableName;

        //find saved variable and set result for next node
        if (savedChoices.ContainsKey(variableName))
            currentResult = savedChoices[variableName];
        else
            Debug.LogError($"There isn't a choice saved with this name: [{variableName}]. Keep current choice and go to next node");

        //go to next node
        CheckNextNode();
    }

    void CheckEventNewspaper(EventNewspaper eventNewspaper)
    {
        string newspaperName = eventNewspaper.NewspaperName;

        //show newspaper for few seconds, then go to next node
        LevelEventsManager.instance.ShowNewspaper(newspaperName)
            .OnComplete(CheckNextNode);
    }

    #endregion
}
