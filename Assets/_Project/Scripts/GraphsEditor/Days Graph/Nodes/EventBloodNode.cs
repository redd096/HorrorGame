#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

/// <summary>
/// Node to use inside a graph view, to declare an event of type EventBlood
/// </summary>
public class EventBloodNode : GraphNode
{
    public EventBlood EventBlood = new EventBlood();

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
        //set object field
        ObjectField objectField = CreateElementsUtilities.CreateObjectField("Background animation", EventBlood.BackgroundAnimation, typeof(AnimationClip), x => EventBlood.BackgroundAnimation = x.newValue as AnimationClip);
        extensionContainer.Add(objectField);
    }
}
#endif