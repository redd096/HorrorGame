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
    const string FOLDER_CUSTOMERS = "Customers";
    const string FOLDER_SAVE_CHOICE = "SaveChoice";
    const string FOLDER_GET_CHOICE = "GetChoice";
    const string FOLDER_EVENTS = "Events";

    Dictionary<string, LevelNodeData> levelNodes;
    Dictionary<string, LevelNodeData> loadedLevelNodes;

    protected override void Initialize(NodesGraphView graph, string assetPathRelativeToProject)
    {
        base.Initialize(graph, assetPathRelativeToProject);

        levelNodes = new Dictionary<string, LevelNodeData>();
        loadedLevelNodes = new Dictionary<string, LevelNodeData>();
    }

    #region save

    protected override void SetNodeDataValues(GraphNode node, NodeData data)
    {
        //save data
        base.SetNodeDataValues(node, data);

        //customer node
        if (node is CustomerNode customerNode)
            data.UserData = customerNode.Customer;
        //save choice node
        else if (node is SaveChoiceNode saveChoiceNode)
            data.UserData = saveChoiceNode.SaveChoice;
        //get choice node
        else if (node is GetChoiceNode getChoiceNode)
            data.UserData = getChoiceNode.GetChoice;
        //events
        else if (node is EventNewspaperNode eventNewspaperNode)
            data.UserData = eventNewspaperNode.EventNewspaper;

        //be sure there aren't nodes with same NodeName, because we use it to create a ScriptableObject
        if (nodes.Find(x => x.NodeName == data.NodeName) != null)
        {
            data.NodeName += "(Clone)";
        }
    }

    protected override void CreateSaveFolder()
    {
        //create folder for graph file
        base.CreateSaveFolder();

        //create folder for every node
        CreateFolder(FOLDER_CUSTOMERS);
        CreateFolder(FOLDER_SAVE_CHOICE);
        CreateFolder(FOLDER_GET_CHOICE);
        CreateFolder(FOLDER_EVENTS);
    }

    void CreateFolder(string folderName)
    {
        string path = Path.Combine(directoryPath, folderName);
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);
    }

    protected override async void SetValuesAndSaveFile()
    {
        //save file normally (to save nodes position and references)
        base.SetValuesAndSaveFile();

        //save also every node UserData
        string customersPath = Path.Combine(directoryPathRelativeToProject, FOLDER_CUSTOMERS);
        string saveChoicePath = Path.Combine(directoryPathRelativeToProject, FOLDER_SAVE_CHOICE);
        string getChoicePath = Path.Combine(directoryPathRelativeToProject, FOLDER_GET_CHOICE);
        string eventsPath = Path.Combine(directoryPathRelativeToProject, FOLDER_EVENTS);

        for (int i = 0; i < nodes.Count; i++)
        {
            NodeData nodeData = nodes[i];

            //customers
            if (nodeData.UserData is Customer customer)
            {
                CustomerData asset = CreateLevelNodeData<CustomerData>(customersPath, nodeData);
                asset.Customer = customer.Clone();
                EditorUtility.SetDirty(asset);
            }
            //save choice
            else if (nodeData.UserData is SaveChoice saveChoice)
            {
                SaveChoiceData asset = CreateLevelNodeData<SaveChoiceData>(saveChoicePath, nodeData);
                asset.SaveChoice = saveChoice.Clone();
                EditorUtility.SetDirty(asset);
            }
            //get choice
            else if (nodeData.UserData is GetChoice getChoice)
            {
                GetChoiceData asset = CreateLevelNodeData<GetChoiceData>(getChoicePath, nodeData);
                asset.GetChoice = getChoice.Clone();
                EditorUtility.SetDirty(asset);
            }
            //events
            else if (nodeData.UserData is EventNewspaper eventNewspaper)
            {
                EventNewspaperData asset = CreateLevelNodeData<EventNewspaperData>(eventsPath, nodeData);
                asset.EventNewspaper = eventNewspaper.Clone();
                EditorUtility.SetDirty(asset);
            }

            //show progress bar and delay of one frame
            EditorUtility.DisplayProgressBar($"Creating copy of every element in graph", $"Saving... {i}/{nodes.Count}", (float)i / nodes.Count);
            if (i % 50 == 0)
                await Task.Delay((int)(Time.deltaTime * 1000));
        }

        //at the end, create also a scriptable object for the level. And connect every Levelnode inside it
        CreateLevelData();
        ConnectEveryAsset();
        
        EditorUtility.ClearProgressBar();
    }

    T CreateLevelNodeData<T>(string folderPath, NodeData nodeData) where T : LevelNodeData
    {
        string assetPath = Path.Combine(folderPath, nodeData.NodeName + ".asset");
        T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, assetPath);
        }

        levelNodes.Add(nodeData.ID, asset);

        return asset;
    }

    void CreateLevelData()
    {
        //create a scriptable object for the level
        string levelFileNameWithExtension = "Level_" + Path.GetFileName(assetPathRelativeToProject);
        string levelAssetPath = Path.Combine(directoryPathRelativeToProject, levelFileNameWithExtension);
        LevelData levelAsset = AssetDatabase.LoadAssetAtPath<LevelData>(levelAssetPath);
        if (levelAsset == null)
        {
            levelAsset = ScriptableObject.CreateInstance<LevelData>();
            AssetDatabase.CreateAsset(levelAsset, levelAssetPath);
        }

        //and set every levelNode
        levelAsset.Nodes = new List<LevelNodeData>(levelNodes.Values);
        EditorUtility.SetDirty(levelAsset);
    }

    void ConnectEveryAsset()
    {
        //and connect every level node also in game data
        foreach (var nodeData in nodes)
        {
            if (levelNodes.ContainsKey(nodeData.ID) == false)
            {
                Debug.LogError("For some reason we didn't create a scriptable object for node with ID: " + nodeData.ID);
                continue;
            }
            if (nodeData.OutputsData == null || nodeData.OutputsData.Count < 1)
            {
                Debug.LogError($"The number of outputs is wrong for node with ID: {nodeData.ID}. They should be at least one but they are {(nodeData.OutputsData != null ? nodeData.OutputsData.Count : 0)}");
                continue;
            }

            LevelNodeData levelNode = levelNodes[nodeData.ID];

            //true door
            string trueID = nodeData.OutputsData[0].ConnectedNodeID;
            if (string.IsNullOrEmpty(trueID) == false && levelNodes.ContainsKey(trueID) == false)
            {
                Debug.LogError($"Node with ID [{nodeData.ID}] error in port TRUE. Should be connected to node with ID [{trueID}] but we didn't created a node with that ID");
                continue;
            }
            levelNode.NodeOnTrue = string.IsNullOrEmpty(trueID) ? null : levelNodes[trueID];

            //false door (if there isn't the second OutputData, is a node with only one output door. So connect to same levelNode as true door)
            string falseID = nodeData.OutputsData.Count >= 2 ? nodeData.OutputsData[1].ConnectedNodeID : trueID;
            if (string.IsNullOrEmpty(falseID) == false && levelNodes.ContainsKey(falseID) == false)
            {
                Debug.LogError($"Node with ID [{nodeData.ID}] error in port FALSE. Should be connected to node with ID [{trueID}] but we didn't created a node with that ID");
                continue;
            }
            levelNode.NodeOnFalse = string.IsNullOrEmpty(falseID) ? null : levelNodes[falseID];

            EditorUtility.SetDirty(levelNode);
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
            string levelFileNameWithExtension = "Level_" + Path.GetFileName(assetPathRelativeToProject);
            string levelAssetPath = Path.Combine(directoryPathRelativeToProject, levelFileNameWithExtension);
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
                customerNode.Customer = customerData.Customer.Clone();
                return;
            }
        }
        //save choice
        else if (node is SaveChoiceNode saveChoiceNode)
        {
            if (loadedLevelNodes.ContainsKey(node.NodeName) && loadedLevelNodes[node.NodeName] is SaveChoiceData saveChoiceData)
            {
                saveChoiceNode.SaveChoice = saveChoiceData.SaveChoice.Clone();
                return;
            }
        }
        //get choice
        else if (node is GetChoiceNode getChoiceNode)
        {
            if (loadedLevelNodes.ContainsKey(node.NodeName) && loadedLevelNodes[node.NodeName] is GetChoiceData getChoiceData)
            {
                getChoiceNode.GetChoice = getChoiceData.GetChoice.Clone();
                return;
            }
        }
        //events
        else if (node is EventNewspaperNode eventNewspaperNode)
        {
            if (loadedLevelNodes.ContainsKey(node.NodeName) && loadedLevelNodes[node.NodeName] is EventNewspaperData eventNewspaperData)
            {
                eventNewspaperNode.EventNewspaper = eventNewspaperData.EventNewspaper.Clone();
                return;
            }
        }
        else
        {
            return;
        }

        Debug.LogError($"Error load node with ID: {node.ID}. " +
            $"Impossible to find in level data a node with name {node.NodeName} or this node isn't correct for a node of type {node.GetType()}");
    }

    #endregion
}
#endif