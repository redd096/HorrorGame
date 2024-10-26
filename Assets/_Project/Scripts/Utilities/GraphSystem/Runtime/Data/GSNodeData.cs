using GraphSystem.Enumerations;
using System.Collections.Generic;
using UnityEngine;

namespace GraphSystem.Data
{
    [System.Serializable]
    public class GSNodeData
    {
        public string NodeName;
        public List<GSNodeOutputData> OutputsData;
        [TextArea] public string ContentText;
        public GSNodeType NodeType;

        [HideInInspector] public int GroupIndex;

        //editor
        [HideInInspector] public string ID;
        [HideInInspector] public string GroupID;
        [HideInInspector] public Vector2 Position;
    }
}