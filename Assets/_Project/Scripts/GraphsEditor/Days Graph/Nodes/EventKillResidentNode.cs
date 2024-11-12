#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

/// <summary>
/// Node to use inside a graph view, to declare a EventKillResident
/// </summary>
public class EventKillResidentNode : GraphNode
{
    public EventKillResident EventKillResident = new EventKillResident();

    ObjectField residentDataField;

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
        //toggle for "kill random" and object field for "specific resident"
        Toggle toggle = CreateElementsUtilities.CreateToggle("Kill Random Resident", EventKillResident.KillRandom, OnUpdateToggle);
        residentDataField = CreateElementsUtilities.CreateObjectField("Resident to Kill", EventKillResident.SpecificResident, typeof(ResidentData), x => EventKillResident.SpecificResident = x.newValue as ResidentData);

        extensionContainer.Add(toggle);

        //add object field only if kill random is false
        if (EventKillResident.KillRandom == false)
            extensionContainer.Add(residentDataField);
    }

    private void OnUpdateToggle(ChangeEvent<bool> evt)
    {
        //update value
        EventKillResident.KillRandom = evt.newValue;
        
        //show or hide object field
        if (EventKillResident.KillRandom)
            extensionContainer.Remove(residentDataField);
        else
            extensionContainer.Add(residentDataField);

        RefreshExpandedState();
    }
}
#endif