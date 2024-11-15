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
    public Customer Customer = new Customer();

    protected override void DrawInputPorts()
    {
        //input port
        Port inputPort = CreateElementsUtilities.CreatePort(this, "", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        inputContainer.Add(inputPort);
    }

    protected override void DrawOutputPorts()
    {
        //output ports - here we create two ports: True and False
        Port truePort = CreateElementsUtilities.CreatePort(this, "OK Enter", Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        truePort.userData = true;       //value to send to input in another node
        Port falsePort = CreateElementsUtilities.CreatePort(this, "NOT Enter", Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        falsePort.userData = false;     //value to send to input in another node
        outputContainer.Add(truePort);
        outputContainer.Add(falsePort);
    }

    protected override void DrawContent()
    {
        //create elements
        CreateDefaultElements();

        CreateIDCard();
        CreateRenunciationCard();
        CreateResidentCard();
        CreatePoliceCard();
        CreateAppointmentCard();

        CreateObjectsToGiveToPlayer();
    }

    #region create default elements

    void CreateDefaultElements()
    {
        //create foldout for customer icons + Add Button
        Foldout iconsFoldout = null;
        iconsFoldout = CreateElementsUtilities.CreateFoldoutWithButton("Customer Icons", "Add Icon", out Button addButton, collapsed: true, onClick: () =>
        {
            Customer.CustomerImage.Add(null);
            CreateCustomerIcon(iconsFoldout, Customer.CustomerImage.Count - 1);
        });
        //StyleSheetUtilities.AddToClassesList(addButton, "ds-node__button");

        //and create current icons
        for (int i = 0; i < Customer.CustomerImage.Count; i++)
            CreateCustomerIcon(iconsFoldout, i);

        //create dialogue textfield
        TextField dialogueTextField = CreateElementsUtilities.CreateTextField("Dialogue When Come", Customer.DialogueWhenCome, x => Customer.DialogueWhenCome = x.newValue.Trim());
        TextField dialogueTrueTextField = CreateElementsUtilities.CreateTextField("Dialogue OK Enter", Customer.DialogueWhenPlayerSayYes, x => Customer.DialogueWhenPlayerSayYes = x.newValue.Trim());
        TextField dialogueFalseTextField = CreateElementsUtilities.CreateTextField("Dialogue NOT Enter", Customer.DialogueWhenPlayerSayNo, x => Customer.DialogueWhenPlayerSayNo = x.newValue.Trim());

        //create toogle to go away like a ghost instead of a person
        var space = CreateElementsUtilities.CreateSpace(Vector2.one * 10);
        Toggle goAwayLikeGhostToggle = CreateElementsUtilities.CreateToggle("Fade away like ghost", Customer.GoAwayLikeGhost, x => Customer.GoAwayLikeGhost = x.newValue);

        //and add to container
        extensionContainer.Add(iconsFoldout);
        extensionContainer.Add(dialogueTextField);
        extensionContainer.Add(dialogueTrueTextField);
        extensionContainer.Add(dialogueFalseTextField);
        extensionContainer.Add(space);
        extensionContainer.Add(goAwayLikeGhostToggle);
    }

    private void CreateCustomerIcon(VisualElement container, int index)
    {
        ObjectField iconObjectField = CreateElementsUtilities.CreateObjectFieldWithPreview("Customer Icon", Customer.CustomerImage[index], Vector2.one * 100, out Image iconImage, x => Customer.CustomerImage[index] = x.newValue as Sprite);
        
        //add before Add Button
        container.Insert(container.childCount - 1, iconObjectField);
        container.Insert(container.childCount - 1, iconImage);
    }

    #endregion

    #region documents

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

    void CreateIDCard()
    {
        //create toggle and generate Graph inside container
        CreateGiveDocumentToggle("ID Card", Customer.GiveIDCard, out VisualElement container, x => Customer.GiveIDCard = x.newValue);
        Customer.IDCard.CreateGraph(container);
    }

    void CreateRenunciationCard()
    {
        //create toggle and generate Graph inside container
        CreateGiveDocumentToggle("Renunciation Card", Customer.GiveRenunciationCard, out VisualElement container, x => Customer.GiveRenunciationCard = x.newValue);
        Customer.RenunciationCard.CreateGraph(container);
    }

    void CreateResidentCard()
    {
        //create toggle and generate Graph inside container
        CreateGiveDocumentToggle("Resident Card", Customer.GiveResidentCard, out VisualElement container, x => Customer.GiveResidentCard = x.newValue);
        Customer.ResidentCard.CreateGraph(container);
    }

    void CreatePoliceCard()
    {
        //create toggle and generate Graph inside container
        CreateGiveDocumentToggle("Police Card", Customer.GivePoliceCard, out VisualElement container, x => Customer.GivePoliceCard = x.newValue);
        Customer.PoliceCard.CreateGraph(container);
    }

    void CreateAppointmentCard()
    {
        //create toggle and generate Graph inside container
        CreateGiveDocumentToggle("Appointment Card", Customer.GiveAppointmentCard, out VisualElement container, x => Customer.GiveAppointmentCard = x.newValue);
        Customer.AppointmentCard.CreateGraph(container);
    }

    #endregion

    #region objects to give to player

    void CreateObjectsToGiveToPlayer()
    {
        //create a space between documents and this
        var space = CreateElementsUtilities.CreateSpace(Vector2.one * 10);

        //create foldout for objects to give + Add Button
        Foldout foldout = null;
        foldout = CreateElementsUtilities.CreateFoldoutWithButton("Objects to give to Player", "Add Object", out Button addButton, collapsed: true, onClick: () =>
        {
            Customer.ObjectsToGiveToPlayer.Add(default);
            CreateObjectToGive(foldout, Customer.ObjectsToGiveToPlayer.Count - 1);
        });

        //and create current objects
        for (int i = 0; i < Customer.ObjectsToGiveToPlayer.Count; i++)
            CreateObjectToGive(foldout, i);

        //and add to container
        extensionContainer.Add(space);
        extensionContainer.Add(foldout);
    }

    void CreateObjectToGive(VisualElement container, int index)
    {
        //create a foldout for these two object fields, and add it before Add button
        Foldout foldout = CreateElementsUtilities.CreateFoldout($"Object [{index}]");
        container.Insert(container.childCount - 1, foldout);

        FGiveToUser currentObj = Customer.ObjectsToGiveToPlayer[index];

        //create both objects fields
        ObjectField left = CreateElementsUtilities.CreateObjectField("Left Prefab", currentObj.LeftPrefab, typeof(InteractableOnTheLeft), x =>
        {
            FGiveToUser obj = Customer.ObjectsToGiveToPlayer[index];
            Customer.ObjectsToGiveToPlayer[index] = new FGiveToUser(x.newValue as InteractableOnTheLeft, obj.RightPrefab);
        });

        ObjectField right = CreateElementsUtilities.CreateObjectField("Right Prefab", currentObj.RightPrefab, typeof(InteractableOnTheRight), x =>
        {
            FGiveToUser obj = Customer.ObjectsToGiveToPlayer[index];
            Customer.ObjectsToGiveToPlayer[index] = new FGiveToUser(obj.LeftPrefab, x.newValue as InteractableOnTheRight);
        });

        //add to foldout
        foldout.Add(left);
        foldout.Add(right);
    }

    #endregion
}
#endif