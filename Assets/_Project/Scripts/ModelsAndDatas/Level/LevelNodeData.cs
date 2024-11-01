using UnityEngine;

/// <summary>
/// In a level there are various customers and events. This is the base class to declare one of them
/// </summary>
public abstract class LevelNodeData : ScriptableObject
{
    [Tooltip("When customer can enter or normal event occured")] public LevelNodeData NodeOnTrue;
    [Tooltip("When customer can't enter (user denyed it)")] public LevelNodeData NodeOnFalse;
}
