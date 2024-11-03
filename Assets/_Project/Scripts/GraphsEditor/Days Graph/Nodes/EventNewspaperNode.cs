#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

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
            //try fileID
            newspaper = GetGameObjectFromFileID();

            //else, try by name
            if (newspaper == null)
            {
                Debug.LogWarning($"Impossible to find newspaper by Local Identifier: {EventNewspaper.NewspaperFileID}. Try find by name: {EventNewspaper.NewspaperName}");
                Transform newspaperTr = LevelEventsManager.instance.NewspapersContainer.Find(EventNewspaper.NewspaperName);
                newspaper = newspaperTr ? newspaperTr.gameObject : null;
            }

        }

        //set object field
        ObjectField objectField = CreateElementsUtilities.CreateObjectField("Newspaper in scene", newspaper, typeof(GameObject), x =>
        {
            EventNewspaper.NewspaperFileID = x.newValue ? GetFileID(x.newValue) : 0;
            EventNewspaper.NewspaperName = x.newValue ? x.newValue.name : "";
        },
        allowSceneObjects: true);

        extensionContainer.Add(objectField);
    }

    private GameObject GetGameObjectFromFileID()
    {
        GameObject[] gameObjectsInScene = Object.FindObjectsOfType<GameObject>(true);

        //find file
        foreach (GameObject go in gameObjectsInScene)
        {
            if (GetFileID(go) == EventNewspaper.NewspaperFileID)
                return go;
        }

        //file not found
        return null;
    }

    long GetFileID(Object obj)
    {
        PropertyInfo inspectorModeInfo = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
        SerializedObject serializedObject = new SerializedObject(obj);
        inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);
        SerializedProperty localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile");

        return localIdProp.longValue;
    }
}
#endif