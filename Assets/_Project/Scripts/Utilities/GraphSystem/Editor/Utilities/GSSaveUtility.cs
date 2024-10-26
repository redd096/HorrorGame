using GraphSystem.Data;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphSystem.Editor
{
    public static class GSSaveUtility
    {
        private static GSGraphView graphView;
        private static string fileName;

        private static List<GSNodeData> nodes;
        private static List<GSGroupData> groups;

        //used on save
        private static Dictionary<string, GSNodeData> createdNodes;
        private static Dictionary<string, int> createdNodesIndex;

        //used on load
        private static Dictionary<string, GSGroup> loadedGroups;
        private static Dictionary<string, GSNode> loadedNodes;

        public static void Initialize(GSGraphView gsGraphView, string gsFileName)
        {
            graphView = gsGraphView;
            fileName = gsFileName;

            nodes = new List<GSNodeData>();
            groups = new List<GSGroupData>();
            createdNodes = new Dictionary<string, GSNodeData>();
            createdNodesIndex = new Dictionary<string, int>();
            loadedGroups = new Dictionary<string, GSGroup>();
            loadedNodes = new Dictionary<string, GSNode>();
        }

        public static void Save()
        {
            CreateSaveFolder();
            GetElementsFromGraphView();
            LinkNodesPorts();
            LinkNodesAndGroups();
            SaveAsset();
        }

        public static void Load()
        {
            //try load
            GSFile asset = AssetDatabase.LoadAssetAtPath<GSFile>(Path.Combine(GSConstUtility.SavedFolderPath, fileName) + ".asset");
            if (asset == null)
            {
                EditorUtility.DisplayDialog(
                    "Couldn't load the file!",
                    "The file at the following path could not be found:\n\n" +
                    $"{Path.Combine(GSConstUtility.SavedFolderPath, fileName) + ".asset"}\n\n" +
                    "Make sure you chose the right file and it's placed at the folder path mentioned above",
                    "OK!");

                return;
            }

            //get elements from asset
            nodes = asset.Nodes;
            groups = asset.Groups;

            LoadGroups();
            LoadNodes();
            LoadNodesConnections();
        }

        private static void SaveAsset()
        {
            //try load file, else create it
            GSFile asset = AssetDatabase.LoadAssetAtPath<GSFile>(Path.Combine(GSConstUtility.SavedFolderPath, fileName) + ".asset");
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<GSFile>();
                AssetDatabase.CreateAsset(asset, Path.Combine(GSConstUtility.SavedFolderPath, fileName) + ".asset");
            }

            //set values
            asset.Initialize(fileName, nodes, groups);

            //save asset
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #region save API

        private static void CreateSaveFolder()
        {
            //if already exists, return
            if (AssetDatabase.IsValidFolder(GSConstUtility.SavedFolderPath))
                return;

            //else create every folder
            string[] folders = GSConstUtility.SavedFolderPath.Split(Path.DirectorySeparatorChar);
            string path = "";
            for (int i = 0; i < folders.Length; i++)
            {
                string newPath = Path.Combine(path, folders[i]);

                //if missing, create folder
                if (AssetDatabase.IsValidFolder(newPath) == false)
                {
                    AssetDatabase.CreateFolder(path, folders[i]);
                }

                path = newPath;
            }
        }

        private static void GetElementsFromGraphView()
        {
            //add starting node as first node
            if (graphView.StartingNode != null)
                nodes.Add(CreateNodeData(graphView.StartingNode));

            graphView.graphElements.ForEach(graphElement =>
            {
                //add every other node
                if (graphElement is GSNode node)
                {
                    if (node != graphView.StartingNode)
                        nodes.Add(CreateNodeData(node));

                    return;
                }

                //add groups
                if (graphElement is GSGroup group)
                {
                    groups.Add(CreateGroupData(group));
                    return;
                }
            });
        }

        private static GSNodeData CreateNodeData(GSNode node)
        {
            GSNodeData data = new GSNodeData()
            {
                NodeName = node.NodeName,
                OutputsData = CloneNodeOutputs(node.OutputsData),
                ContentText = node.ContentText,
                NodeType = node.NodeType,
                GroupIndex = -1,
                ID = node.ID,
                GroupID = node.Group != null ? node.Group.ID : "",
                Position = node.GetPosition().position,
            };

            createdNodes.Add(data.ID, data);
            createdNodesIndex.Add(data.ID, createdNodesIndex.Count);
            return data;
        }

        private static GSGroupData CreateGroupData(GSGroup group)
        {
            GSGroupData data = new GSGroupData()
            {
                GroupName = group.title,
                ID = group.ID,
                Position = group.GetPosition().position,
                ContentNodesIndex = new List<int>(),
                ContentNodeIDs = GetNodesID(group.containedElements),
            };

            return data;
        }

        private static void LinkNodesPorts()
        {
            //foreach node output
            foreach (GSNodeData node in nodes)
            {
                //link with next node
                foreach (GSNodeOutputData data in node.OutputsData)
                {
                    if (string.IsNullOrEmpty(data.NextNodeID) == false && createdNodesIndex.ContainsKey(data.NextNodeID))
                        data.NextNodeIndex = createdNodesIndex[data.NextNodeID];
                }
            }
        }

        private static void LinkNodesAndGroups()
        {
            //foreach group
            for (int i = 0; i < groups.Count; i++)
            {
                //link with its nodes
                GSGroupData group = groups[i];
                foreach (string nodeID in group.ContentNodeIDs)
                {
                    GSNodeData node = createdNodes[nodeID];
                    group.ContentNodesIndex.Add(createdNodesIndex[nodeID]);
                    node.GroupIndex = i;
                }
            }
        }

        #endregion

        #region load API

        private static void LoadGroups()
        {
            if (groups == null)
                return;

            //instantiate every group and set values
            foreach (GSGroupData data in groups)
            {
                GSGroup group = graphView.CreateGroup(data.GroupName, data.Position);
                group.ID = data.ID;

                loadedGroups.Add(group.ID, group);
            }
        }

        private static void LoadNodes()
        {
            if (nodes == null)
                return;

            //instantiate every node and set values
            foreach (GSNodeData data in nodes)
            {
                GSNode node = graphView.CreateNode(data.NodeName, data.NodeType, data.Position, false);
                node.ID = data.ID;
                node.OutputsData = CloneNodeOutputs(data.OutputsData);
                node.ContentText = data.ContentText;

                //and draw only when is setted (to show correct values)
                node.Draw();
                node.FinishDraw();

                //if the node is inside a group, find the group in the dictionary and add to it
                if (string.IsNullOrEmpty(data.GroupID) == false)
                {
                    GSGroup group = loadedGroups[data.GroupID];
                    node.Group = group;

                    group.AddElement(node);
                }

                loadedNodes.Add(node.ID, node);
            }
        }

        private static void LoadNodesConnections()
        {
            if (loadedNodes == null)
                return;

            //foreach node
            foreach (KeyValuePair<string, GSNode> loadedNode in loadedNodes)
            {
                //find every output port
                foreach (VisualElement element in loadedNode.Value.outputContainer.Children())
                {
                    if (element is Port outputPort)
                    {
                        GSNodeOutputData data = outputPort.userData as GSNodeOutputData;

                        //and connect to the next node
                        if (string.IsNullOrEmpty(data.NextNodeID) == false)
                        {
                            GSNode nextNode = loadedNodes[data.NextNodeID];
                            Port nextNodeInputPort = System.Linq.Enumerable.First(nextNode.inputContainer.Children()) as Port;

                            Edge edge = outputPort.ConnectTo(nextNodeInputPort);

                            graphView.AddElement(edge);
                            loadedNode.Value.RefreshPorts();
                        }
                    }
                }
            }
        }

        #endregion

        #region utilities

        private static List<GSNodeOutputData> CloneNodeOutputs(List<GSNodeOutputData> nodeOutputs)
        {
            //copy every node output to avoid references problems
            List<GSNodeOutputData> clonedOutputs = new List<GSNodeOutputData>();
            foreach (var nodeOutput in nodeOutputs)
            {
                GSNodeOutputData data = new GSNodeOutputData(nodeOutput.Text, nodeOutput.NextNodeID);
                clonedOutputs.Add(data);
            }
            return clonedOutputs;
        }

        private static List<string> GetNodesID(IEnumerable<GraphElement> elements)
        {
            //from group elements, get nodes ID
            List<string> nodesID = new List<string>();
            foreach (var element in elements)
            {
                if (element is GSNode node)
                    nodesID.Add(node.ID);
            }
            return nodesID;
        }

        #endregion
    }
}