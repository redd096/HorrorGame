#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

/// <summary>
/// Node to use inside a graph view, to declare a DebugLogText
/// </summary>
public class DebugLogTextNode : GraphNode
{
    public DebugLogText DebugLogText = new DebugLogText();

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
        TextField textField = CreateElementsUtilities.CreateTextArea("Text: ", DebugLogText.Text, x => DebugLogText.Text = x.newValue.Trim());

        extensionContainer.Add(textField);
    }
}
#endif