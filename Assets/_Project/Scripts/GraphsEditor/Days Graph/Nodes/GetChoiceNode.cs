#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

/// <summary>
/// Node to use inside a graph view, to declare a GetChoice
/// </summary>
public class GetChoiceNode : GraphNode
{
    public GetChoice GetChoice = new GetChoice();

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
        TextField textField = CreateElementsUtilities.CreateTextField("Variable Name", GetChoice.VariableName, x => GetChoice.VariableName = x.newValue.Trim());
        extensionContainer.Add(textField);
    }
}
#endif