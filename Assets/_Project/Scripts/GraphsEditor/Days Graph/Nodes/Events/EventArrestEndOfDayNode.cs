#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Node to use inside a graph view, to declare a EventArrestEndOfDay
/// </summary>
public class EventArrestEndOfDayNode : GraphNode
{
    public EventArrestEndOfDay EventArrestEndOfDay = new EventArrestEndOfDay();

    protected override void DrawInputPorts()
    {
        //input port
        Port inputPort = CreateElementsUtilities.CreatePort(this, "", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        inputContainer.Add(inputPort);
    }

    protected override void DrawOutputPorts()
    {
        //output port
        Port truePort = CreateElementsUtilities.CreatePort(this, "Next", Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        truePort.userData = true;       //value to send to input in another node
        outputContainer.Add(truePort);
    }

    protected override void DrawContent()
    {
        //create foldout for customer icons + Add Button
        Foldout iconsFoldout = null;
        iconsFoldout = CreateElementsUtilities.CreateFoldoutWithButton("PoliceMan Icons", "Add Icon", out Button addButton, collapsed: true, onClick: () =>
        {
            EventArrestEndOfDay.CustomerImage.Add(null);
            CreateCustomerIcon(iconsFoldout, EventArrestEndOfDay.CustomerImage.Count - 1);
        });
        //StyleSheetUtilities.AddToClassesList(addButton, "ds-node__button");
        
        //and create current icons
        for (int i = 0; i < EventArrestEndOfDay.CustomerImage.Count; i++)
            CreateCustomerIcon(iconsFoldout, i);

        //create dialogue textfield
        TextField dialogueTextField = CreateElementsUtilities.CreateTextField("Dialogue PoliceMan", EventArrestEndOfDay.DialogueWhenCome, x => EventArrestEndOfDay.DialogueWhenCome = x.newValue.Trim());
        
        //and add to container
        extensionContainer.Add(iconsFoldout);
        extensionContainer.Add(dialogueTextField);
    }

    private void CreateCustomerIcon(VisualElement container, int index)
    {
        ObjectField iconObjectField = CreateElementsUtilities.CreateObjectFieldWithPreview($"Icon {index}", EventArrestEndOfDay.CustomerImage[index], Vector2.one * 100, out Image iconImage, x => EventArrestEndOfDay.CustomerImage[index] = x.newValue as Sprite);
        
        //add before Add Button
        container.Insert(container.childCount - 1, iconObjectField);
        container.Insert(container.childCount - 1, iconImage);
    }
}
#endif