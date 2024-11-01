#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

/// <summary>
/// Node to use inside a graph view, to declare a Customer
/// </summary>
public class CustomerNode : GraphNode
{
    public CustomerModel CustomerModel;

    protected override void DrawInputPorts()
    {
        //input port
        Port inputPort = CreateElementsUtilities.CreatePort(this, "", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        inputContainer.Add(inputPort);
    }

    protected override void DrawOutputPorts()
    {
        //output ports - here we create two ports: True and False
        Port truePort = CreateElementsUtilities.CreatePort(this, "Fatto entrare", Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        truePort.userData = true;       //value to send to input in another node
        Port falsePort = CreateElementsUtilities.CreatePort(this, "NON fatto entrare", Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        falsePort.userData = false;     //value to send to input in another node
        outputContainer.Add(truePort);
        outputContainer.Add(falsePort);
    }

    protected override void DrawContent()
    {
        //be sure customer model isn't null
        if (CustomerModel == null)
        {
            CustomerModel = new CustomerModel();
            CustomerModel.CustomerImage = new List<Sprite>() { default };
            CustomerModel.IDCard = new IDCard();
            CustomerModel.RenunciationCard = new RenunciationCard();
            CustomerModel.ResidentCard = new ResidentCard();
            CustomerModel.PoliceCard = new PoliceCard();
            CustomerModel.ObjectsToGiveToPlayer = new List<FGiveToUser>();
        }

        //create elements
        CreateDefaultElements();
        CreateIDCard();
        CreateRenunciationCard();
        CreateResidentCard();
        CreatePoliceDocument();
    }

    void CreateDefaultElements()
    {
        //create elements
        ObjectField iconObjectField = CreateElementsUtilities.CreateObjectFieldWithPreview("Customer Icon", CustomerModel.CustomerImage[0], Vector2.one * 100, out Image iconImage, x => CustomerModel.CustomerImage[0] = x.newValue as Sprite);
        TextField dialogueTextField = CreateElementsUtilities.CreateTextField("Dialogue Path", CustomerModel.Dialogue, x => CustomerModel.Dialogue = x.newValue);

        //and add
        extensionContainer.Add(iconObjectField);
        extensionContainer.Add(iconImage);
        extensionContainer.Add(dialogueTextField);
    }

    void CreateGiveDocumentToggle(string documentName, bool value, out VisualElement container, EventCallback<ChangeEvent<bool>> callback)
    {
        //space and toggle
        var space = CreateElementsUtilities.CreateSpace(Vector2.one * 10);
        Toggle toggle = CreateElementsUtilities.CreateToggle($"Give [{documentName}] to player", value, callback);

        //create foldout container
        container = CreateElementsUtilities.CreateFoldout(documentName, collapsed: true);

        //add
        extensionContainer.Add(space);
        extensionContainer.Add(toggle);
        extensionContainer.Add(container);
    }

    #region documents

    void CreateIDCard()
    {
        //create toggle and generate Graph inside container
        CreateGiveDocumentToggle("ID Card", CustomerModel.GiveIDCard, out VisualElement container, x => CustomerModel.GiveIDCard = x.newValue);
        CustomerModel.IDCard.CreateGraph(container);
    }

    void CreateRenunciationCard()
    {
        //create toggle and generate Graph inside container
        CreateGiveDocumentToggle("Renunciation Card", CustomerModel.GiveRenunciationCard, out VisualElement container, x => CustomerModel.GiveRenunciationCard = x.newValue);
        CustomerModel.RenunciationCard.CreateGraph(container);
    }

    void CreateResidentCard()
    {
        //create toggle and generate Graph inside container
        CreateGiveDocumentToggle("Resident Card", CustomerModel.GiveResidentCard, out VisualElement container, x => CustomerModel.GiveResidentCard = x.newValue);
        CustomerModel.ResidentCard.CreateGraph(container);
    }

    void CreatePoliceDocument()
    {
        //create toggle and generate Graph inside container
        CreateGiveDocumentToggle("Police Card", CustomerModel.GivePoliceCard, out VisualElement container, x => CustomerModel.GivePoliceCard = x.newValue);
        CustomerModel.PoliceCard.CreateGraph(container);
    }

    #endregion
}
#endif