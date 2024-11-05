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
    [SerializeField] float customerAnimation = 2;

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

        //start dialogue
        Sequence sequence = Sequence.Create();
        sequence.ChainCallback(() => Debug.Log("Start dialogue"));

        //move customer away from screen
        sequence.Chain(Tween.Position(currentCustomer.transform, customerEndPoint.position, customerStartPoint.position, customerAnimation));

        //check if player did something wrong, then move to next node
        sequence.ChainCallback(() => Debug.Log("Check if player did something wrong"));
        sequence.ChainCallback(CheckNextNode);
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
        sequence.Chain(Tween.Position(currentCustomer.transform, customerStartPoint.position, customerEndPoint.position, customerAnimation));

        //start dialogue
        sequence.ChainCallback(() => Debug.Log("Start dialogue"));

        //then give documents
        sequence.ChainCallback(() => CheckCustomerGiveDocuments(currentCustomer, customer));
    }

    void CheckCustomerGiveDocuments(CustomerBehaviour customerInstance, Customer customer)
    {
        bool giveDocuments = false;

        //give documents
        if (customer.GiveIDCard)
        {
            DeskManager.instance.InstantiateDocument(customer.IDCard);
            giveDocuments = true;
        }
        if (customer.GiveRenunciationCard)
        {
            DeskManager.instance.InstantiateDocument(customer.RenunciationCard);
            giveDocuments = true;
        }
        if (customer.GiveResidentCard)
        {
            DeskManager.instance.InstantiateDocument(customer.ResidentCard);
            giveDocuments = true;
        }
        if (customer.GivePoliceCard)
        {
            DeskManager.instance.InstantiateDocument(customer.PoliceCard);
            giveDocuments = true;
        }
        if (customer.GiveAppointmentCard)
        {
            DeskManager.instance.InstantiateDocument(customer.AppointmentCard);
            giveDocuments = true;
        }

        //if don't give documents, move to next node
        if (giveDocuments == false)
            OnGiveBackAllDocuments();
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
