using UnityEngine;

namespace GraphSystem.Data
{
    [System.Serializable]
    public class GSNodeOutputData
    {
        public string Text;
        public int NextNodeIndex;

        //editor
        [HideInInspector] public string NextNodeID;

        public GSNodeOutputData(string text, string nextNodeID = "")
        {
            Text = text;
            NextNodeID = nextNodeID;
            NextNodeIndex = -1;
        }
    }
}