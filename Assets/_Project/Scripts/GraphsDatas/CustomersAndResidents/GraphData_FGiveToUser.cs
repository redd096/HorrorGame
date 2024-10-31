using UnityEditor;

/// <summary>
/// Data to save and load a "Customer" or "Event" inside a GraphView
/// </summary>
[System.Serializable]
public class GraphData_FGiveToUser
{
    public string LeftPrefabPath;
    public string RightPrefabPath;

    public GraphData_FGiveToUser(FGiveToUser data)
    {
        LeftPrefabPath = AssetDatabase.GetAssetPath(data.LeftPrefab);
        RightPrefabPath = AssetDatabase.GetAssetPath(data.RightPrefab);
    }
}