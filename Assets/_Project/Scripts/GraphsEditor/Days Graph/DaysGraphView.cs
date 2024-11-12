#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

/// <summary>
/// Create every day (customers and events order)
/// </summary>
public class DaysGraphView : NodesGraphView
{
    public DaysGraphView(EditorWindow editorWindow) : base(editorWindow)
    {
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);

        //add group
        if (evt.target is GraphView || evt.target is Node)
        {
            evt.menu.AppendAction("Add Group", (actionEvent) =>
            CreateGroup("Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)));
        }
        if (evt.target is GraphView)
        {
            //customer node
            AddNodeButton<CustomerNode>(evt, "Add Customer", "Customer");

            //save choice node
            AddNodeButton<SaveChoiceNode>(evt, "Save Player Choice", "Save Player Choice");

            //get choice node
            AddNodeButton<GetChoiceNode>(evt, "Get Player Choice", "Get Player Choice");

            //events
            AddNodeButton<EventNewspaperNode>(evt, "Events/Newspaper", "Show Newspaper");
            AddNodeButton<EventKillResidentNode>(evt, "Events/Kill Resident", "Kill Resident");
        }
    }

    void AddNodeButton<T>(ContextualMenuPopulateEvent evt, string buttonPath, string nodeName)
    {
        evt.menu.AppendAction(buttonPath, (actionEvent) => 
        CreateNode(nodeName, typeof(T), GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)));
    }
}
#endif