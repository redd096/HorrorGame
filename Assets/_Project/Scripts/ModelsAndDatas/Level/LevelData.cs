using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In a level there are various customers and events. This is the data that contains the flow of customers and events in a level
/// </summary>
public class LevelData : ScriptableObject
{
    public List<LevelNodeData> Nodes = new List<LevelNodeData>();
}
