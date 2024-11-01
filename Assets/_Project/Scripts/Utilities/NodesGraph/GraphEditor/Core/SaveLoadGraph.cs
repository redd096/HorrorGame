#if UNITY_EDITOR
using redd096.NodesGraph.Runtime;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace redd096.NodesGraph.Editor
{
    /// <summary>
    /// This is the base class used for save and load the graph. Inherit from it and manage the files as you want
    /// </summary>
    public class SaveLoadGraph
    {
        protected List<NodeData> nodes;
        protected List<GroupData> groups;

        //used on load
        protected Dictionary<string, GraphGroup> loadedGroups;
        protected Dictionary<string, GraphNode> loadedNodes;

        /// <summary>
        /// Save a scriptable object with a reference to every node and group in the graph
        /// </summary>
        /// <param name="assetPathRelativeToProject">The path to the file, but the path must starts with Assets</param>
        public virtual void Save(NodesGraphView graph, string assetPathRelativeToProject)
        {
            InitializeBeforeSave();
            GetElementsFromGraphView(graph);
        }

        /// <summary>
        /// Load file data and recreate a graph with it
        /// </summary>
        /// <param name="assetPathRelativeToProject">The path to the file, but the path must starts with Assets</param>
        public virtual void Load(NodesGraphView graph, string assetPathRelativeToProject)
        {

        }

        #region save API

        protected virtual void InitializeBeforeSave()
        {
            nodes = new List<NodeData>();
            groups = new List<GroupData>();
        }

        protected virtual void GetElementsFromGraphView(NodesGraphView graph)
        {
            //add starting node as first node
            BeforeAddGraphElements(graph);

            graph.graphElements.ForEach(graphElement =>
            {
                //add every other node
                if (AddNode(graph, graphElement))
                    return;

                //add groups
                if (AddGroup(graph, graphElement))
                    return;

                //add other elements
                if (AddOtherGraphElements(graph, graphElement))
                    return;
            });
        }

        protected virtual void BeforeAddGraphElements(NodesGraphView graph)
        {
            //add starting node as first node
            if (graph.StartingNode != null)
                nodes.Add(CreateNodeData(graph.StartingNode));
        }

        protected virtual bool AddNode(NodesGraphView graph, GraphElement graphElement)
        {
            //add every node
            if (graphElement is GraphNode node)
            {
                //not Starting Node, because we already added it as first node
                if (node != graph.StartingNode)
                    nodes.Add(CreateNodeData(node));

                return true;
            }

            return false;
        }

        protected virtual bool AddGroup(NodesGraphView graph, GraphElement graphElement)
        {
            //add groups
            if (graphElement is GraphGroup group)
            {
                groups.Add(CreateGroupData(group));
                return true;
            }

            return false;
        }

        protected virtual bool AddOtherGraphElements(NodesGraphView graph, GraphElement graphElement)
        {
            return false;
        }

        protected virtual NodeData CreateNodeData(GraphNode node)
        {
            //create data from GraphNode
            NodeData data = new NodeData()
            {
                NodeName = node.NodeName,
                ID = node.ID,
                NodeType = node.GetType().FullName,
                OutputsData = CreateOutputData(node),
                GroupID = node.Group != null ? node.Group.ID : "",
                Position = node.GetPosition().position,
            };

            return data;
        }

        protected virtual List<NodeOutputData> CreateOutputData(GraphNode node)
        {
            List<NodeOutputData> datas = new List<NodeOutputData>();

            //foreach output port
            List<Port> list = node.outputContainer.Query<Port>().ToList();
            foreach (Port port in list)
            {
                //save type
                NodeOutputData data = new NodeOutputData();
                data.OutputType = port.portType.Name;
                
                //and connected node
                if (port.connected)
                {
                    foreach (var edge in port.connections)
                    {
                        if (edge.input != null && edge.input.node is GraphNode connectedNode)
                        {
                            data.ConnectedNodeID = connectedNode.ID;
                            break;
                        }
                    }
                }

                //and add to list
                datas.Add(data);
            }

            return datas;
        }

        protected virtual GroupData CreateGroupData(GraphGroup group)
        {
            //create data from GraphGroup
            GroupData data = new GroupData()
            {
                Title = group.title,
                ID = group.ID,
                Position = group.GetPosition().position,
                ContainedNodesID = group.containedElements.Where(x => x is GraphNode).Select(x => (x as GraphNode).ID).ToList(),    //for every contained node, save ID
            };

            return data;
        }

        #endregion

        #region load API

        #endregion
    }
}
#endif