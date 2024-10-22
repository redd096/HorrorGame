using UnityEngine;

/// <summary>
/// Document prefab for the Left Canvas
/// </summary>
public class DocumentLeft : MonoBehaviour
{
    private DocumentRight docRight;

    /// <summary>
    /// Initialize document
    /// </summary>
    /// <param name="docRight"></param>
    public void Init(DocumentRight docRight)
    {
        this.docRight = docRight;
    }
}