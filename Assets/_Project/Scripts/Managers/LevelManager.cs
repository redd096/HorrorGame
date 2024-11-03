using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This manage the level: show customers or events, save player's choice and so on
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelData levelData;

    private LevelNodeData currentNode;

    //used by nodes SaveChoice and GetChoice
    private Dictionary<string, bool> savedChoices = new Dictionary<string, bool>();

    public void CheckNextNode()
    {

    }
}
