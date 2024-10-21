using UnityEngine;

/// <summary>
/// This is attached to the 2d prefab of the document
/// </summary>
public class Document2D : MonoBehaviour
{
    public void SetInteractable(bool interactable)
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = interactable;
    }
}