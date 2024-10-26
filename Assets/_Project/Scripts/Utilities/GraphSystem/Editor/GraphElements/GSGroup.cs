using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphSystem.Editor
{
    public class GSGroup : Group
    {
        public string ID;

        public GSGroup(string groupTitle, Vector2 position) : base()
        {
            //set title and position
            ID = System.Guid.NewGuid().ToString();
            title = groupTitle;
            SetPosition(new Rect(position, Vector2.zero));
        }
    }
}