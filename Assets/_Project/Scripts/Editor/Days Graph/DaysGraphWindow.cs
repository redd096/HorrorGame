#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using UnityEditor;
using UnityEngine.UIElements;

/// <summary>
/// Window to show Graph View to create every day (customers and events order)
/// </summary>
public class DaysGraphWindow : WindowGraph
{
    [MenuItem("HORROR GAME/Days Graph")]
    public static void ShowWindow()
    {
        //open window (and set title)
        GetWindow<DaysGraphWindow>("Days Graph");
    }

    protected override void CreateGraph()
    {
        //base.CreateGraph();

        graph = new DaysGraphView(this);
        graph.StretchToParentSize();
    }

    protected override void CreateToolbar()
    {
        //base.CreateToolbar();

        toolbar = new NodesGraphToolbar(graph as DaysGraphView, new DaysGraphSaveLoad());
    }
}
#endif