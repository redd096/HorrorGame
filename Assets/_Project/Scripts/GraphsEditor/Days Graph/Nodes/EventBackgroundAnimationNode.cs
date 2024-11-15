#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

/// <summary>
/// Node to use inside a graph view, to declare a EventBackgroundAnimation
/// </summary>
public class EventBackgroundAnimationNode : GraphNode
{
    public EventBackgroundAnimation EventBackgroundAnimation = new EventBackgroundAnimation();

    ObjectField animationToPlayField;
    Toggle waitAnimationToggle;

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
        //toggle for "PlayAnimation" and object field for "AnimationToPlay"
        Toggle toggle = CreateElementsUtilities.CreateToggle("Play animation (false to stop animations)", EventBackgroundAnimation.PlayAnimation, OnUpdateToggle);
        animationToPlayField = CreateElementsUtilities.CreateObjectField("Animation to play", EventBackgroundAnimation.AnimationToPlay, typeof(AnimationClip), x => EventBackgroundAnimation.AnimationToPlay = x.newValue as AnimationClip);
        waitAnimationToggle = CreateElementsUtilities.CreateToggle("Wait animation", EventBackgroundAnimation.WaitAnimation, x => EventBackgroundAnimation.WaitAnimation = x.newValue);

        extensionContainer.Add(toggle);

        //add object field only if PlayAnimation is true
        if (EventBackgroundAnimation.PlayAnimation)
        {
            extensionContainer.Add(animationToPlayField);
            extensionContainer.Add(waitAnimationToggle);
        }
    }

    private void OnUpdateToggle(ChangeEvent<bool> evt)
    {
        //update value
        EventBackgroundAnimation.PlayAnimation = evt.newValue;

        //show or hide object field
        if (EventBackgroundAnimation.PlayAnimation)
        {
            extensionContainer.Add(animationToPlayField);
            extensionContainer.Add(waitAnimationToggle);
        }
        else
        {
            extensionContainer.Remove(animationToPlayField);
            extensionContainer.Remove(waitAnimationToggle);
        }

        RefreshExpandedState();
    }
}
#endif