using System.Collections.Generic;
using UnityEngine;

namespace GraphSystem.Data
{
    [System.Serializable]
    public class GSGroupData
    {
        //editor
        public string GroupName;
        public string ID;
        public Vector2 Position;

        //useless?
        public List<int> ContentNodesIndex;
        [HideInInspector] public List<string> ContentNodeIDs;
    }
}