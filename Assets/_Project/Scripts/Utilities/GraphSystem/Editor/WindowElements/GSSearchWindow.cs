using GraphSystem.Enumerations;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphSystem.Editor
{
    public class GSSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private GSGraphView graphView;

        /// <summary>
        /// Call initialize to set the GraphView
        /// </summary>
        /// <param name="graphView"></param>
        public void Initialize(GSGraphView graphView)
        {
            this.graphView = graphView;
        }

        /// <summary>
        /// Menu to show when press Space
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),     //SearchTreeGroupEntry contains other SearchTreeEntry buttons, 0 level is the Title on the top
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 1),     //level 1 is the first list of buttons
                new SearchTreeEntry(new GUIContent("Single"))                   //this is button Entry instead of group, and level 2 is inside the previous level 1 in the list (in this case Create Node)
                {
                    level = 2,
                    userData = GSNodeType.Single                                //user data is used to identify which button is pressed (used in OnSelectEntry)
                },
                new SearchTreeEntry(new GUIContent("Multiple"))
                {
                    level = 2,
                    userData = GSNodeType.Multiple
                },
                new SearchTreeGroupEntry(new GUIContent("Create Group"), 1),    //another level 1
                new SearchTreeEntry(new GUIContent("Group"))                    //this level 2 will be inside Create Group
                {
                    level = 2,
                    userData = new Group()
                }
            };

            return entries;
        }

        /// <summary>
        /// Event called when press the button SearchTreeEntry
        /// </summary>
        /// <param name="SearchTreeEntry"></param>
        /// <param name="context"></param>
        /// <returns>Return true to close the search menu, return false to keep it open</returns>
        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);

            //find event to call
            switch (SearchTreeEntry.userData)
            {
                case GSNodeType.Single:
                    {
                        graphView.CreateNode("NodeName", GSNodeType.Single, localMousePosition);
                        return true;
                    }
                case GSNodeType.Multiple:
                    {
                        graphView.CreateNode("NodeName", GSNodeType.Multiple, localMousePosition);
                        return true;
                    }
                case Group _:
                    {
                        graphView.CreateGroup("Group", localMousePosition);
                        return true;
                    }
            }
            
            //if click other buttons, keep the menu open
            return false;
        }
    }
}