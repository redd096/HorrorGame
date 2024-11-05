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

    private LevelNodeData currentNode;
    private bool currentResult;         //user allowed customer to ENTER (true) or NOT ENTER (false)
    private bool alreadySetResult;      //this is used to save result only first time. If user put other stamps, they're ignored

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

        Debug.Log("TODO - Customer must says something and leave \n" +
            "then we have to check if the player choice is correct");

        //and continue with next node
        CheckNextNode();
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
        Debug.Log("TODO");
        //istanziare prefab del customer
        //farlo muovere ballonzando fino al bancone
        //fargli dire le sue frasi
        //dare i documenti e gli oggetti segnati

        if (customer.GiveIDCard)
            DeskManager.instance.InstantiateDocument(customer.IDCard);
        if (customer.GiveRenunciationCard)
            DeskManager.instance.InstantiateDocument(customer.RenunciationCard);
        if (customer.GiveResidentCard)
            DeskManager.instance.InstantiateDocument(customer.ResidentCard);
        if (customer.GivePoliceCard)
            DeskManager.instance.InstantiateDocument(customer.PoliceCard);
        if (customer.GiveAppointmentCard)
            DeskManager.instance.InstantiateDocument(customer.AppointmentCard);
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
