using System.Collections.Generic;
using UnityEngine;

namespace redd096.NodesGraph.Runtime
{
    /// <summary>
    /// Data to save node
    /// </summary>
    [System.Serializable]
    public class NodeData
    {
        public string NodeName;
        public string ID;
        public string NodeType;
        public List<NodeOutputData> OutputsData;
        public string GroupID;
        public Vector2 Position;
    }
}