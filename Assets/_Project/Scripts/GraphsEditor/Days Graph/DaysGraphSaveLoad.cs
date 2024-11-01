#if UNITY_EDITOR
using redd096.NodesGraph.Editor;
using redd096.NodesGraph.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Save scriptable object for LevelManager and file to Load graph again
/// </summary>
public class DaysGraphSaveLoad : SaveLoadGraph
{
    Dictionary<string, LevelNodeData> loadedLevelNodes;

    #region save

    protected override NodeData CreateNodeData(GraphNode node)
    {
        NodeData data;

        //customer node
        if (node is CustomerNode customerNode)
        {
            data = new CustomerNodeData() { CustomerModel = customerNode.CustomerModel };
        }
        //events
        else
        {
            data = new NodeData();
        }

        //set default node values
        SetNodeDataValues(node, data);
        return data;

        //return base.CreateNodeData(node);
    }

    protected override void CreateSaveFolder()
    {
        //create folder for graph file
        base.CreateSaveFolder();

        //create folders for customers
        string customersPath = Path.Combine(directoryPath, "Customers");
        if (Directory.Exists(customersPath) == false)
            Directory.CreateDirectory(customersPath);

        //and events
        string eventsPath = Path.Combine(directoryPath, "Events");
        if (Directory.Exists(eventsPath) == false)
            Directory.CreateDirectory(eventsPath);
    }

    protected override async void SetValuesAndSaveFile()
    {
        //save file normally (to save nodes position and references)
        base.SetValuesAndSaveFile();

        //save also every customer and event
        string customersPath = Path.Combine(directoryPathRelativeToProject, "Customers");
        string eventsPath = Path.Combine(directoryPathRelativeToProject, "Events");

        Dictionary<string, LevelNodeData> levelNodes = new Dictionary<string, LevelNodeData>();

        for (int i = 0; i < nodes.Count; i++)
        {
            string fileName = nodes[i].NodeName;

            //customers
            if (nodes[i] is CustomerNodeData customerNodeData)
            {
                string assetPath = Path.Combine(customersPath, fileName + ".asset");
                CustomerData asset = AssetDatabase.LoadAssetAtPath<CustomerData>(assetPath);
                if (asset == null)
                {
                    asset = ScriptableObject.CreateInstance<CustomerData>();
                    AssetDatabase.CreateAsset(asset, assetPath);
                }

                asset.CustomerModel = customerNodeData.CustomerModel.Clone();
                levelNodes.Add(customerNodeData.ID, asset);
                EditorUtility.SetDirty(asset);
            }
            //events
            else
            {
                string assetPath = Path.Combine(eventsPath, fileName + ".asset");

            }

            //show progress bar and delay of one frame
            EditorUtility.DisplayProgressBar($"Creating copy of every element in graph", $"Saving... {i}/{nodes.Count}", (float)i / nodes.Count);
            if (i % 50 == 0)
                await Task.Delay((int)(Time.deltaTime * 1000));
        }
        
        //at the end, create also a scriptable object for the level
        string levelFileName = "Level_" + Path.GetFileNameWithoutExtension(assetPathRelativeToProject);
        string levelAssetPath = Path.Combine(directoryPathRelativeToProject, levelFileName + ".asset");
        LevelData levelAsset = AssetDatabase.LoadAssetAtPath<LevelData>(levelAssetPath);
        if (levelAsset == null)
        {
            levelAsset = ScriptableObject.CreateInstance<LevelData>();
            AssetDatabase.CreateAsset(levelAsset, levelAssetPath);
        }

        levelAsset.Nodes = new List<LevelNodeData>(levelNodes.Values);
        EditorUtility.SetDirty(levelAsset);
        
        EditorUtility.ClearProgressBar();

        //and connect every level node also in game data
        foreach (var node in nodes)
        {
            if (levelNodes.ContainsKey(node.ID) == false)
            {
                Debug.LogError("For some reason we didn't create a scriptable object for node with ID: " + node.ID);
                continue;
            }
            if (node.OutputsData == null || node.OutputsData.Count != 2)
            {
                Debug.LogError($"The number of outputs is wrong for node with ID: {node.ID}. They should be 2 but they are {(node.OutputsData != null ? node.OutputsData.Count : 0)}");
                continue;
            }

            LevelNodeData levelNode = levelNodes[node.ID];
            string trueID = node.OutputsData[0].ConnectedNodeID;
            string falseID = node.OutputsData[1].ConnectedNodeID;

            if (string.IsNullOrEmpty(trueID) == false && levelNodes.ContainsKey(trueID) == false)
            {
                Debug.LogError($"Node with ID [{node.ID}] error in port TRUE. Should be connected to node with ID [{trueID}] but we didn't created a node with that ID");
                continue;
            }

            if (string.IsNullOrEmpty(falseID) == false && levelNodes.ContainsKey(falseID) == false)
            {
                Debug.LogError($"Node with ID [{node.ID}] error in port FALSE. Should be connected to node with ID [{trueID}] but we didn't created a node with that ID");
                continue;
            }

            levelNode.NodeOnTrue = string.IsNullOrEmpty(trueID) ? null : levelNodes[trueID];
            levelNode.NodeOnFalse = string.IsNullOrEmpty(falseID) ? null : levelNodes[falseID];
        }
    }

    #endregion

    #region load

    protected override bool TryLoadFile()
    {
        //load file for graph
        bool loaded = base.TryLoadFile();

        //load also level data. We use game datas to popolate nodes
        if (loaded)
        {
            string levelFileName = "Level_" + Path.GetFileNameWithoutExtension(assetPathRelativeToProject);
            string levelAssetPath = Path.Combine(directoryPathRelativeToProject, levelFileName + ".asset");
            LevelData asset = AssetDatabase.LoadAssetAtPath<LevelData>(levelAssetPath);

            //error if not found
            if (asset == null)
            {
                EditorUtility.DisplayDialog(
                    "Couldn't load the Level file!",
                    "The file at the following path could not be found:\n\n" +
                    $"{levelAssetPath}\n\n" +
                    "Make sure you chose the right file and it's placed at the folder path mentioned above",
                    "OK!");

                return false;
            }

            //save level nodes by file name
            loadedLevelNodes = new Dictionary<string, LevelNodeData>();
            foreach (var levelNode in asset.Nodes)
            {
                loadedLevelNodes.Add(levelNode.name, levelNode);
            }

            return true;
        }

        return loaded;
    }

    protected override void SetNodeValues(GraphNode node, NodeData data)
    {
        base.SetNodeValues(node, data);

        //set customer values
        if (node is CustomerNode customerNode)
        {
            if (loadedLevelNodes.ContainsKey(node.NodeName) && loadedLevelNodes[node.NodeName] is CustomerData customerData)
            {
                customerNode.CustomerModel = customerData.CustomerModel.Clone();
                return;
            }
        }
        //events
        else
        {

        }

        Debug.LogError($"Error load node with ID: {node.ID}. " +
            $"Impossible to find in level data a node with name {node.NodeName} or this node isn't correct for a node of type {node.GetType()}");
    }

    #endregion
}
#endif