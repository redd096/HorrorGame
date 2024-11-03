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
        //if saved, try find newspaper in scene
        GameObject newspaper = null;
        if (string.IsNullOrEmpty(EventNewspaper.NewspaperName) == false)
        {
            //try by InstanceID
            newspaper = GetGameObjectFromInstanceID(EventNewspaper.NewspaperInstanceID);

            //else, try by name
            if (newspaper == null)
            {
                Debug.LogWarning($"Impossible to find newspaper by InstanceID {EventNewspaper.NewspaperInstanceID}. Try find by name {EventNewspaper.NewspaperName}");
                Transform newspaperTr = LevelEventsManager.instance.NewspapersContainer.Find(EventNewspaper.NewspaperName);
                newspaper = newspaperTr ? newspaperTr.gameObject : null;
            }

        }

        //set object field
        ObjectField objectField = CreateElementsUtilities.CreateObjectField("Newspaper in scene", newspaper, typeof(GameObject), x =>
        {
            EventNewspaper.NewspaperInstanceID = x.newValue ? x.newValue.GetInstanceID() : 0;
            EventNewspaper.NewspaperName = x.newValue ? x.newValue.name : "";
        },
        allowSceneObjects: true);

        extensionContainer.Add(objectField);
    }

    private GameObject GetGameObjectFromInstanceID(int instanceID)
    {
        GameObject[] gameObjectsInScene = Object.FindObjectsOfType<GameObject>(true);

        //find file
        foreach (GameObject go in gameObjectsInScene)
        {
            int id = go.GetInstanceID();
            if (id == instanceID)
                return go;
        }
        ////for some reason in Inspector Debug Mode, the showed Instance ID is wrong. It has +2 at its value, so try decrease to find the correct object
        //foreach (GameObject go in gameObjectsInScene)
        //{
        //    int id = go.GetInstanceID() - 2;
        //    if (id == instanceID)
        //        return go;
        //}

        //file not found
        return null;
    }
}
#endif