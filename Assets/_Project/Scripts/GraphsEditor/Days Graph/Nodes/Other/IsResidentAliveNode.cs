#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

/// <summary>
/// Node to use inside a graph view, to declare a IsResidentAlive
/// </summary>
public class IsResidentAliveNode : GraphNode
{
    public IsResidentAlive IsResidentAlive = new IsResidentAlive();

    protected override void DrawInputPorts()
    {
        //input port
        Port inputPort = CreateElementsUtilities.CreatePort(this, "", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        inputContainer.Add(inputPort);
    }

    protected override void DrawOutputPorts()
    {
        //output ports - here we create two ports: True and False
        Port truePort = CreateElementsUtilities.CreatePort(this, "Yes", Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        truePort.userData = true;       //value to send to input in another node
        Port falsePort = CreateElementsUtilities.CreatePort(this, "No", Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        falsePort.userData = false;     //value to send to input in another node
        outputContainer.Add(truePort);
        outputContainer.Add(falsePort);
    }

    protected override void DrawContent()
    {
        ObjectField objectField = CreateElementsUtilities.CreateObjectField("Is resident alive:", IsResidentAlive.Resident, typeof(ResidentData), x => IsResidentAlive.Resident = x.newValue as ResidentData);
        extensionContainer.Add(objectField);
    }
}
#endif