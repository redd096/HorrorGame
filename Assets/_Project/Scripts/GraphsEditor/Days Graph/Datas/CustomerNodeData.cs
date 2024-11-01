#if UNITY_EDITOR
using redd096.NodesGraph.Runtime;

/// <summary>
/// When save node data, keep track of values
/// </summary>
public class CustomerNodeData : NodeData
{
    public CustomerModel CustomerModel;
}
#endif