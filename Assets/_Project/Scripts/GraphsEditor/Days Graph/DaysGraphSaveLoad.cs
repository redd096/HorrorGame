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
    const string FOLDER_OTHER = "Other";

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
        else if (node is EventKillResidentNode eventKillResidentNode)
            data.UserData = eventKillResidentNode.EventKillResident;
        else if (node is EventStartBackgroundAnimationNode eventStartBackgroundAnimationNode)
            data.UserData = eventStartBackgroundAnimationNode.EventStartBackgroundAnimation;
        else if (node is EventStopBackgroundAnimationNode eventStopBackgroundAnimationNode)
            data.UserData = eventStopBackgroundAnimationNode.EventStopBackgroundAnimation;
        else if (node is EventBloodNode eventBloodNode)
            data.UserData = eventBloodNode.EventBlood;
        else if (node is EventStartRedEventNode eventStartRedEventNode)
            data.UserData = eventStartRedEventNode.EventStartRedEvent;
        else if (node is EventStopRedEventNode eventStopRedEventNode)
            data.UserData = eventStopRedEventNode.EventStopRedEvent;
        //other
        else if (node is IsResidentAliveNode isResidentAliveNode)
            data.UserData = isResidentAliveNode.IsResidentAlive;
        else if (node is DelayForSecondsNode delayForSecondsNode)
            data.UserData = delayForSecondsNode.DelayForSeconds;
        else if (node is DebugLogTextNode debugLogTextNode)
            data.UserData = debugLogTextNode.DebugLogText;

        //be sure there aren't nodes with same NodeName, because we use it to create a ScriptableObject
        while (nodes.Find(x => x.NodeName == data.NodeName) != null)
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
        CreateFolder(FOLDER_OTHER);
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
        for (int i = 0; i < nodes.Count; i++)
        {
            NodeData nodeData = nodes[i];

            //customers
            if (nodeData.UserData is Customer customer)
            {
                string path = Path.Combine(directoryPathRelativeToProject, FOLDER_CUSTOMERS);
                CustomerData asset = CreateLevelNodeData<CustomerData>(path, nodeData);
                asset.Customer = customer.Clone();
                EditorUtility.SetDirty(asset);
            }
            //save choice
            else if (nodeData.UserData is SaveChoice saveChoice)
            {
                string path = Path.Combine(directoryPathRelativeToProject, FOLDER_SAVE_CHOICE);
                SaveChoiceData asset = CreateLevelNodeData<SaveChoiceData>(path, nodeData);
                asset.SaveChoice = saveChoice.Clone();
                EditorUtility.SetDirty(asset);
            }
            //get choice
            else if (nodeData.UserData is GetChoice getChoice)
            {
                string path = Path.Combine(directoryPathRelativeToProject, FOLDER_GET_CHOICE);
                GetChoiceData asset = CreateLevelNodeData<GetChoiceData>(path, nodeData);
                asset.GetChoice = getChoice.Clone();
                EditorUtility.SetDirty(asset);
            }
            //events
            else if (nodeData.UserData is EventNewspaper eventNewspaper)
            {
                string path = Path.Combine(directoryPathRelativeToProject, FOLDER_EVENTS);
                EventNewspaperData asset = CreateLevelNodeData<EventNewspaperData>(path, nodeData);
                asset.EventNewspaper = eventNewspaper.Clone();
                EditorUtility.SetDirty(asset);
            }
            else if (nodeData.UserData is EventKillResident eventKillResident)
            {
                string path = Path.Combine(directoryPathRelativeToProject, FOLDER_EVENTS);
                EventKillResidentData asset = CreateLevelNodeData<EventKillResidentData>(path, nodeData);
                asset.EventKillResident = eventKillResident.Clone();
                EditorUtility.SetDirty(asset);
            }
            else if (nodeData.UserData is EventStartBackgroundAnimation eventStartBackgroundAnimation)
            {
                string path = Path.Combine(directoryPathRelativeToProject, FOLDER_EVENTS);
                EventStartBackgroundAnimationData asset = CreateLevelNodeData<EventStartBackgroundAnimationData>(path, nodeData);
                asset.EventStartBackgroundAnimation = eventStartBackgroundAnimation.Clone();
                EditorUtility.SetDirty(asset);
            }
            else if (nodeData.UserData is EventStopBackgroundAnimation eventStopBackgroundAnimation)
            {
                string path = Path.Combine(directoryPathRelativeToProject, FOLDER_EVENTS);
                EventStopBackgroundAnimationData asset = CreateLevelNodeData<EventStopBackgroundAnimationData>(path, nodeData);
                asset.EventStopBackgroundAnimation = eventStopBackgroundAnimation.Clone();
                EditorUtility.SetDirty(asset);
            }
            else if (nodeData.UserData is EventBlood eventBlood)
            {
                string path = Path.Combine(directoryPathRelativeToProject, FOLDER_EVENTS);
                EventBloodData asset = CreateLevelNodeData<EventBloodData>(path, nodeData);
                asset.EventBlood = eventBlood.Clone();
                EditorUtility.SetDirty(asset);
            }
            else if (nodeData.UserData is EventStartRedEvent eventStartRedEvent)
            {
                string path = Path.Combine(directoryPathRelativeToProject, FOLDER_EVENTS);
                EventStartRedEventData asset = CreateLevelNodeData<EventStartRedEventData>(path, nodeData);
                asset.EventStartRedEvent = eventStartRedEvent.Clone();
                EditorUtility.SetDirty(asset);
            }
            else if (nodeData.UserData is EventStopRedEvent eventStopRedEvent)
            {
                string path = Path.Combine(directoryPathRelativeToProject, FOLDER_EVENTS);
                EventStopRedEventData asset = CreateLevelNodeData<EventStopRedEventData>(path, nodeData);
                asset.EventStopRedEvent = eventStopRedEvent.Clone();
                EditorUtility.SetDirty(asset);
            }
            //other
            else if (nodeData.UserData is IsResidentAlive isResidentAlive)
            {
                string path = Path.Combine(directoryPathRelativeToProject, FOLDER_OTHER);
                IsResidentAliveData asset = CreateLevelNodeData<IsResidentAliveData>(path, nodeData);
                asset.IsResidentAlive = isResidentAlive.Clone();
                EditorUtility.SetDirty(asset);
            }
            else if (nodeData.UserData is DelayForSeconds delayForSeconds)
            {
                string path = Path.Combine(directoryPathRelativeToProject, FOLDER_OTHER);
                DelayForSecondsData asset = CreateLevelNodeData<DelayForSecondsData>(path, nodeData);
                asset.DelayForSeconds = delayForSeconds.Clone();
                EditorUtility.SetDirty(asset);
            }
            else if (nodeData.UserData is DebugLogText debugLogText)
            {
                string path = Path.Combine(directoryPathRelativeToProject, FOLDER_OTHER);
                DebugLogTextData asset = CreateLevelNodeData<DebugLogTextData>(path, nodeData);
                asset.DebugLogText = debugLogText.Clone();
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
        string levelFileNameWithExtension = "LevelData_" + Path.GetFileName(assetPathRelativeToProject);
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
            string levelFileNameWithExtension = "LevelData_" + Path.GetFileName(assetPathRelativeToProject);
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
        else if (node is EventKillResidentNode eventKillResidentNode)
        {
            if (loadedLevelNodes.ContainsKey(node.NodeName) && loadedLevelNodes[node.NodeName] is EventKillResidentData eventKillResidentData)
            {
                eventKillResidentNode.EventKillResident = eventKillResidentData.EventKillResident.Clone();
                return;
            }
        }
        else if (node is EventStartBackgroundAnimationNode eventStartBackgroundAnimationNode)
        {
            if (loadedLevelNodes.ContainsKey(node.NodeName) && loadedLevelNodes[node.NodeName] is EventStartBackgroundAnimationData eventStartBackgroundAnimationData)
            {
                eventStartBackgroundAnimationNode.EventStartBackgroundAnimation = eventStartBackgroundAnimationData.EventStartBackgroundAnimation.Clone();
                return;
            }
        }
        else if (node is EventStopBackgroundAnimationNode eventStopBackgroundAnimationNode)
        {
            if (loadedLevelNodes.ContainsKey(node.NodeName) && loadedLevelNodes[node.NodeName] is EventStopBackgroundAnimationData eventStopBackgroundAnimationData)
            {
                eventStopBackgroundAnimationNode.EventStopBackgroundAnimation = eventStopBackgroundAnimationData.EventStopBackgroundAnimation.Clone();
                return;
            }
        }
        else if (node is EventBloodNode eventBloodNode)
        {
            if (loadedLevelNodes.ContainsKey(node.NodeName) && loadedLevelNodes[node.NodeName] is EventBloodData eventBloodData)
            {
                eventBloodNode.EventBlood = eventBloodData.EventBlood.Clone();
                return;
            }
        }
        else if (node is EventStartRedEventNode eventStartRedEventNode)
        {
            if (loadedLevelNodes.ContainsKey(node.NodeName) && loadedLevelNodes[node.NodeName] is EventStartRedEventData eventStartRedEventData)
            {
                eventStartRedEventNode.EventStartRedEvent = eventStartRedEventData.EventStartRedEvent.Clone();
                return;
            }
        }
        else if (node is EventStopRedEventNode eventStopRedEventNode)
        {
            if (loadedLevelNodes.ContainsKey(node.NodeName) && loadedLevelNodes[node.NodeName] is EventStopRedEventData eventStopRedEventData)
            {
                eventStopRedEventNode.EventStopRedEvent = eventStopRedEventData.EventStopRedEvent.Clone();
                return;
            }
        }
        //other
        else if (node is IsResidentAliveNode isResidentAliveNode)
        {
            if (loadedLevelNodes.ContainsKey(node.NodeName) && loadedLevelNodes[node.NodeName] is IsResidentAliveData isResidentAliveData)
            {
                isResidentAliveNode.IsResidentAlive = isResidentAliveData.IsResidentAlive.Clone();
                return;
            }
        }
        else if (node is DelayForSecondsNode delayForSecondsNode)
        {
            if (loadedLevelNodes.ContainsKey(node.NodeName) && loadedLevelNodes[node.NodeName] is DelayForSecondsData delayForSecondsData)
            {
                delayForSecondsNode.DelayForSeconds = delayForSecondsData.DelayForSeconds.Clone();
                return;
            }
        }
        else if (node is DebugLogTextNode debugLogTextNode)
        {
            if (loadedLevelNodes.ContainsKey(node.NodeName) && loadedLevelNodes[node.NodeName] is DebugLogTextData debugLogTextData)
            {
                debugLogTextNode.DebugLogText = debugLogTextData.DebugLogText.Clone();
                return;
            }
        }
        //else error
        else
        {
            Debug.LogError($"Error load node with ID: {node.ID}. " +
                $"Impossible to find in level data a node with name {node.NodeName} or this node isn't correct for a node of type {node.GetType()}");

            return;
        }
    }

    #endregion
}
#endif