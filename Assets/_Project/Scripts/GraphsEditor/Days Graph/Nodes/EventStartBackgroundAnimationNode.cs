#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

/// <summary>
/// Node to use inside a graph view, to declare a EventStartBackgroundAnimation
/// </summary>
public class EventStartBackgroundAnimationNode : GraphNode
{
    public EventStartBackgroundAnimation EventStartBackgroundAnimation = new EventStartBackgroundAnimation();

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
        ObjectField animationToPlayField = CreateElementsUtilities.CreateObjectField("Animation to play", EventStartBackgroundAnimation.AnimationToPlay, typeof(AnimationClip), x => EventStartBackgroundAnimation.AnimationToPlay = x.newValue as AnimationClip);
        Toggle waitAnimationToggle = CreateElementsUtilities.CreateToggle("Wait animation", EventStartBackgroundAnimation.WaitAnimation, x => EventStartBackgroundAnimation.WaitAnimation = x.newValue);
        FloatField delayFloatField = CreateElementsUtilities.CreateFloatField("Delay after animation", EventStartBackgroundAnimation.DelayAfterAnimation, x => EventStartBackgroundAnimation.DelayAfterAnimation = x.newValue);

        extensionContainer.Add(animationToPlayField);
        extensionContainer.Add(waitAnimationToggle);
        extensionContainer.Add(delayFloatField);
    }
}
#endif