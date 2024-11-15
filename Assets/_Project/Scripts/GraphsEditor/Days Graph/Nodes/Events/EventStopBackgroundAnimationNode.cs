#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

/// <summary>
/// Node to use inside a graph view, to declare a EventStopBackgroundAnimation
/// </summary>
public class EventStopBackgroundAnimationNode : GraphNode
{
    public EventStopBackgroundAnimation EventStopBackgroundAnimation = new EventStopBackgroundAnimation();

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
        FloatField delayFloatField = CreateElementsUtilities.CreateFloatField("Delay before next node", EventStopBackgroundAnimation.DelayBeforeNextNode, x => EventStopBackgroundAnimation.DelayBeforeNextNode = x.newValue);

        extensionContainer.Add(delayFloatField);
    }
}
#endif