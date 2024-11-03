#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Node to use inside a graph view, to declare an event of type Newspaper
/// </summary>
public class EventNewspaperNode : GraphNode
{
    public EventNewspaper EventNewspaper = new EventNewspaper();

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
        //find newspaper in scene
        Transform newspaper = string.IsNullOrEmpty(EventNewspaper.NewspaperName) ? null : LevelEventsManager.instance.NewspapersContainer.Find(EventNewspaper.NewspaperName);
        GameObject newspaperObj = newspaper ? newspaper.gameObject : null;

        //set object field
        ObjectField objectField = CreateElementsUtilities.CreateObjectField("Newspaper in scene", newspaperObj, typeof(GameObject), 
            x => EventNewspaper.NewspaperName = x.newValue ? x.newValue.name : "", 
            allowSceneObjects: true);

        extensionContainer.Add(objectField);
    }
}
#endif