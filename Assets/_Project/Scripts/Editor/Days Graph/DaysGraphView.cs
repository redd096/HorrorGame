#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor;
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
    }
}
#endif