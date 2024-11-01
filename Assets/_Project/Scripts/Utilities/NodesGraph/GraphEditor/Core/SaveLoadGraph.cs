#if UNITY_EDITOR
using redd096.NodesGraph.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

namespace redd096.NodesGraph.Editor
{
    /// <summary>
    /// This is the base class used for save and load the graph. Inherit from it and manage the files as you want
    /// </summary>
    public class SaveLoadGraph
    {
        protected NodesGraphView graph;
        protected string assetPathRelativeToProject;

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
            this.graph = graph;
            this.assetPathRelativeToProject = assetPathRelativeToProject;

            //get elements in graph and save file
            GetElementsFromGraphView();
            SaveAsset();
        }

        /// <summary>
        /// Load file data and recreate a graph with it
        /// </summary>
        /// <param name="assetPathRelativeToProject">The path to the file, but the path must starts with Assets</param>
        public virtual void Load(NodesGraphView graph, string assetPathRelativeToProject)
        {
            this.graph = graph;
            this.assetPathRelativeToProject = assetPathRelativeToProject;

            //load file and set elements in graph
            if (TryLoadFile())
            {
                SetElementsInGraphView();
            }
        }

        #region save API - get elements in graph

        protected virtual void GetElementsFromGraphView()
        {
            //initialize lists
            InitializeBeforeGetElementsFromGraph();

            //add starting node as first node
            BeforeAddGraphElements();

            graph.graphElements.ForEach(graphElement =>
            {
                //add every other node
                if (AddNode(graphElement))
                    return;

                //add groups
                if (AddGroup(graphElement))
                    return;

                //add other elements
                if (AddOtherGraphElements(graphElement))
                    return;
            });
        }

        protected virtual void InitializeBeforeGetElementsFromGraph()
        {
            //initialize lists
            nodes = new List<NodeData>();
            groups = new List<GroupData>();
        }

        protected virtual void BeforeAddGraphElements()
        {
            //add starting node as first node
            if (graph.StartingNode != null)
                nodes.Add(CreateNodeData(graph.StartingNode));
        }

        protected virtual bool AddNode(GraphElement graphElement)
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

        protected virtual bool AddGroup(GraphElement graphElement)
        {
            //add groups
            if (graphElement is GraphGroup group)
            {
                groups.Add(CreateGroupData(group));
                return true;
            }

            return false;
        }

        protected virtual bool AddOtherGraphElements(GraphElement graphElement)
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

        #region save API - save file

        protected virtual void SaveAsset()
        {
            //create folder if not exists
            CreateSaveFolder();

            //save file
            SetValuesAndSaveFile();
            RefreshEditor();
        }

        protected virtual void CreateSaveFolder()
        {
            string pathToProject = Application.dataPath.Replace("Assets", string.Empty);    //remove "Assets" because it should already be in assetPath
            string projectDirectories = Path.GetDirectoryName(assetPathRelativeToProject);  //remove file from path and keep only directories

            //create folder if not exists
            string path = Path.Combine(pathToProject, projectDirectories);
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
        }

        protected virtual void SetValuesAndSaveFile()
        {
            //try load file
            FileData asset = AssetDatabase.LoadAssetAtPath<FileData>(assetPathRelativeToProject);
            if (asset == null)
            {
                //else create it
                asset = ScriptableObject.CreateInstance<FileData>();
                AssetDatabase.CreateAsset(asset, assetPathRelativeToProject);
            }

            //set values
            string fileName = Path.GetFileNameWithoutExtension(assetPathRelativeToProject);
            asset.Initialize(fileName, nodes, groups);

            //set dirty in editor
            EditorUtility.SetDirty(asset);
        }

        protected virtual void RefreshEditor()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion

        #region load API - load file

        protected virtual bool TryLoadFile()
        {
            //try load
            FileData asset = AssetDatabase.LoadAssetAtPath<FileData>(assetPathRelativeToProject);

            //error if not found
            if (asset == null)
            {
                EditorUtility.DisplayDialog(
                    "Couldn't load the file!",
                    "The file at the following path could not be found:\n\n" +
                    $"{assetPathRelativeToProject}\n\n" +
                    "Make sure you chose the right file and it's placed at the folder path mentioned above",
                    "OK!");

                return false;
            }

            //get elements from asset
            nodes = asset.Nodes;
            groups = asset.Groups;

            return true;
        }

        #endregion

        #region load API - set elements in graph

        protected virtual void SetElementsInGraphView()
        {
            InitializeBeforeSetElementsInGraph();
            LoadGroups();
            LoadNodes();
            LoadNodesConnections();
        }

        protected virtual void InitializeBeforeSetElementsInGraph()
        {
            //initialize dictionaries
            loadedGroups = new Dictionary<string, GraphGroup>();
            loadedNodes = new Dictionary<string, GraphNode>();
        }

        protected virtual void LoadGroups()
        {
            if (groups == null)
                return;

            //instantiate every group and set values
            foreach (GroupData data in groups)
            {
                GraphGroup group = graph.CreateGroup(data.Title, data.Position);
                group.ID = data.ID;

                //add to dictionary
                loadedGroups.Add(group.ID, group);
            }
        }

        protected virtual void LoadNodes()
        {
            if (nodes == null)
                return;

            //instantiate every node and set values
            foreach (NodeData data in nodes)
            {
                System.Type nodeType = System.Type.GetType(data.NodeType);
                GraphNode node = graph.CreateNode(data.NodeName, nodeType, data.Position);
                node.ID = data.ID;

                //if the node is inside a group, find the group in the dictionary and add to it
                if (string.IsNullOrEmpty(data.GroupID) == false)
                {
                    if (loadedGroups.ContainsKey(data.GroupID))
                    {
                        GraphGroup group = loadedGroups[data.GroupID];
                        node.Group = group;

                        group.AddElement(node);
                    }
                    else
                    {
                        Debug.LogError($"Node with id: {data.ID}. Impossible to find group with id: {data.GroupID}");
                    }
                }

                //add to dictionary
                loadedNodes.Add(node.ID, node);
            }
        }

        protected virtual void LoadNodesConnections()
        {
            if (nodes == null || loadedNodes == null)
                return;

            //foreach node
            foreach (NodeData data in nodes)
            {
                //check if we created this node
                if (loadedNodes.ContainsKey(data.ID) == false)
                {
                    Debug.LogError($"Impossible to find node with ID: {data.ID}");
                    continue;
                }

                //check if there are output ports
                GraphNode node = loadedNodes[data.ID];
                List<Port> list = node.outputContainer.Query<Port>().ToList();
                if (list.Count != data.OutputsData.Count)
                {
                    Debug.LogError($"Error: Node has {list.Count} output doors, but in data are saved {data.OutputsData.Count} outputs");
                    continue;
                }

                //foreach output
                for (int i = 0; i < data.OutputsData.Count; i++)
                {
                    Port outputPort = list[i];
                    string connectedNodeID = data.OutputsData[i].ConnectedNodeID;

                    //connect to other node
                    if (string.IsNullOrEmpty(connectedNodeID) == false)
                    {
                        //check if we created a node with this ID
                        if (loadedNodes.ContainsKey(connectedNodeID) == false)
                        {
                            Debug.LogError($"Error: Node with ID {data.ID} should be connected to a node with ID {connectedNodeID}, but there isn't a node with ID {connectedNodeID}");
                            continue;
                        }

                        //find first input port still not connected to other nodes and with correct Port Type
                        GraphNode connectedNode = loadedNodes[connectedNodeID];
                        List<Port> inputPorts = connectedNode.inputContainer.Query<Port>().ToList();
                        Port connectedPort = inputPorts.Where(port => port.connected == false && port.portType.IsAssignableFrom(outputPort.portType)).FirstOrDefault();

                        //connect ports
                        Edge edge = outputPort.ConnectTo(connectedPort);
                        graph.AddElement(edge);
                        node.RefreshPorts();
                    }
                }
            }
        }

        #endregion
    }
}
#endif