#if UNITY_EDITOR
using redd096.NodesGraph.Editor;

/// <summary>
/// Save scriptable object for LevelManager and file to Load graph again
/// </summary>
public class DaysGraphSaveLoad : SaveLoadGraph
{
    public override void Save(NodesGraphView graph, string assetPathRelativeToProject)
    {
        base.Save(graph, assetPathRelativeToProject);
    }

    public override void Load(NodesGraphView graph, string assetPathRelativeToProject)
    {
        base.Load(graph, assetPathRelativeToProject);
    }
}
#endif