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
    public CustomerModel CustomerModel = new CustomerModel();

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
        CreatePoliceDocument();

        CreateObjectsToGiveToPlayer();
    }

    #region create default elements

    void CreateDefaultElements()
    {
        //create foldout for customer icons + Add Button
        Foldout iconsFoldout = null;
        iconsFoldout = CreateElementsUtilities.CreateFoldoutWithButton("Customer Icons", "Add Icon", out Button addButton, collapsed: true, onClick: () =>
        {
            CustomerModel.CustomerImage.Add(null);
            CreateCustomerIcon(iconsFoldout, CustomerModel.CustomerImage.Count - 1);
        });
        //StyleSheetUtilities.AddToClassesList(addButton, "ds-node__button");

        //and create current icons
        for (int i = 0; i < CustomerModel.CustomerImage.Count; i++)
            CreateCustomerIcon(iconsFoldout, i);

        //create dialogue textfield
        TextField dialogueTextField = CreateElementsUtilities.CreateTextField("Dialogue When Come", CustomerModel.DialogueWhenCome, x => CustomerModel.DialogueWhenCome = x.newValue);
        TextField dialogueTrueTextField = CreateElementsUtilities.CreateTextField("Dialogue OK Enter", CustomerModel.DialogueWhenPlayerSayYes, x => CustomerModel.DialogueWhenPlayerSayYes = x.newValue);
        TextField dialogueFalseTextField = CreateElementsUtilities.CreateTextField("Dialogue NOT Enter", CustomerModel.DialogueWhenPlayerSayNo, x => CustomerModel.DialogueWhenPlayerSayNo = x.newValue);

        //and add to container
        extensionContainer.Add(iconsFoldout);
        extensionContainer.Add(dialogueTextField);
        extensionContainer.Add(dialogueTrueTextField);
        extensionContainer.Add(dialogueFalseTextField);
    }

    private void CreateCustomerIcon(VisualElement container, int index)
    {
        ObjectField iconObjectField = CreateElementsUtilities.CreateObjectFieldWithPreview("Customer Icon", CustomerModel.CustomerImage[index], Vector2.one * 100, out Image iconImage, x => CustomerModel.CustomerImage[index] = x.newValue as Sprite);
        
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

    #region create default elements

    void CreateObjectsToGiveToPlayer()
    {
        //create a space between documents and this
        var space = CreateElementsUtilities.CreateSpace(Vector2.one * 10);

        //create foldout for objects to give + Add Button
        Foldout foldout = null;
        foldout = CreateElementsUtilities.CreateFoldoutWithButton("Objects to give to Player", "Add Object", out Button addButton, collapsed: true, onClick: () =>
        {
            CustomerModel.ObjectsToGiveToPlayer.Add(default);
            CreateObjectToGive(foldout, CustomerModel.ObjectsToGiveToPlayer.Count - 1);
        });

        //and create current objects
        for (int i = 0; i < CustomerModel.ObjectsToGiveToPlayer.Count; i++)
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

        FGiveToUser currentObj = CustomerModel.ObjectsToGiveToPlayer[index];

        //create both objects fields
        ObjectField left = CreateElementsUtilities.CreateObjectField("Left Prefab", currentObj.LeftPrefab, typeof(InteractableOnTheLeft), x =>
        {
            FGiveToUser obj = CustomerModel.ObjectsToGiveToPlayer[index];
            CustomerModel.ObjectsToGiveToPlayer[index] = new FGiveToUser(x.newValue as InteractableOnTheLeft, obj.RightPrefab);
        });

        ObjectField right = CreateElementsUtilities.CreateObjectField("Right Prefab", currentObj.RightPrefab, typeof(InteractableOnTheRight), x =>
        {
            FGiveToUser obj = CustomerModel.ObjectsToGiveToPlayer[index];
            CustomerModel.ObjectsToGiveToPlayer[index] = new FGiveToUser(obj.LeftPrefab, x.newValue as InteractableOnTheRight);
        });

        //add to foldout
        foldout.Add(left);
        foldout.Add(right);
    }

    #endregion
}
#endif