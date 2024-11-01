#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Node to use inside a graph view, to declare a Customer
/// </summary>
public class CustomerNode : GraphNode
{
    public CustomerModel Customer;

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
        Customer = new CustomerModel();

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
        ObjectField iconObjectField = CreateElementsUtilities.CreateObjectFieldWithPreview("Customer Icon", Vector2.one * 100, out Image iconImage, x => Customer.CustomerImage = x.newValue as Sprite);
        TextField dialogueTextField = CreateElementsUtilities.CreateTextField("Dialogue Path", Customer.Dialogue, x => Customer.Dialogue = x.newValue);

        //and add
        extensionContainer.Add(iconObjectField);
        extensionContainer.Add(iconImage);
        extensionContainer.Add(dialogueTextField);
    }

    void CreateGiveDocumentToggle(string documentName, out VisualElement container, EventCallback<ChangeEvent<bool>> callback)
    {
        //space and toggle
        var space = CreateElementsUtilities.CreateSpace(Vector2.one * 10);
        Toggle toggle = CreateElementsUtilities.CreateToggle($"Give [{documentName}] to player", callback);

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
        //create toggle
        CreateGiveDocumentToggle("ID Card", out VisualElement container, x => Customer.GiveIDCard = x.newValue);

        //generate Graph inside container
        Customer.IDCard = new IDCard();
        Customer.IDCard.CreateGraph(container);
    }

    void CreateRenunciationCard()
    {
        //create toggle
        CreateGiveDocumentToggle("Renunciation Card", out VisualElement container, x => Customer.GiveRenunciationCard = x.newValue);

        //generate Graph inside container
        Customer.RenunciationCard = new RenunciationCard();
        Customer.RenunciationCard.CreateGraph(container);
    }

    void CreateResidentCard()
    {
        //create toggle
        CreateGiveDocumentToggle("Resident Card", out VisualElement container, x => Customer.GiveResidentCard = x.newValue);

        //generate Graph inside container
        Customer.ResidentCard = new ResidentCard();
        Customer.ResidentCard.CreateGraph(container);
    }

    void CreatePoliceDocument()
    {
        //create toggle
        CreateGiveDocumentToggle("Police Document", out VisualElement container, x => Customer.GivePoliceDocument = x.newValue);

        //generate Graph inside container
        Customer.PoliceDocument = new PoliceDocument();
        Customer.PoliceDocument.CreateGraph(container);
    }

    #endregion
}
#endif