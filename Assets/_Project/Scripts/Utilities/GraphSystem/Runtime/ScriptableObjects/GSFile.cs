using GraphSystem.Data;
using System.Collections.Generic;
using UnityEngine;

public class GSFile : ScriptableObject
{
    public string FileName;
    [Space]
    public List<GSNodeData> Nodes;
    [HideInInspector] public List<GSGroupData> Groups;

    public void Initialize(string fileName, List<GSNodeData> nodes, List<GSGroupData> groups)
    {
        FileName = fileName;
        Nodes = nodes;
        Groups = groups;
    }

    #region public API

    /// <summary>
    /// Return the Starting Node
    /// </summary>
    /// <returns></returns>
    public GSNodeData GetStartingNode()
    {
        if (Nodes == null || Nodes.Count == 0)
            return null;

        return Nodes[0];
    }

    /// <summary>
    /// Get the Starting Node
    /// </summary>
    /// <returns>Return true if the starting node is found</returns>
    public bool GetStartingNode(out GSNodeData startingNode)
    {
        if (Nodes == null || Nodes.Count == 0)
        {
            startingNode = null;
            return false;
        }

        startingNode = Nodes[0];
        return true;
    }

    /// <summary>
    /// Return the Next Node from the list
    /// </summary>
    /// <param name="currentNodeOutput"></param>
    /// <returns></returns>
    public GSNodeData GetNextNode(GSNodeOutputData currentNodeOutput)
    {
        if (currentNodeOutput == null || Nodes == null || Nodes.Count <= currentNodeOutput.NextNodeIndex)
            return null;

        return Nodes[currentNodeOutput.NextNodeIndex];
    }

    /// <summary>
    /// Get the Next Node from the list
    /// </summary>
    /// <param name="currentNodeOutput"></param>
    /// <returns>Return true if the node is found</returns>
    public bool GetNextNode(GSNodeOutputData currentNodeOutput, out GSNodeData nextNode)
    {
        if (currentNodeOutput == null || Nodes == null || Nodes.Count <= currentNodeOutput.NextNodeIndex)
        {
            nextNode = null;
            return false;
        }

        nextNode = Nodes[currentNodeOutput.NextNodeIndex];
        return true;
    }

    #endregion
}
