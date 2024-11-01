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
        //customer node
        if (evt.target is GraphView)
        {
            evt.menu.AppendAction("Add Customer", (actionEvent) =>
            CreateNode("Customer", typeof(CustomerNode),
                GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)));
        }
    }
}
#endif