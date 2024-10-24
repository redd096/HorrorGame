using UnityEngine;

/// <summary>
/// This is the interactable in "scene". 
/// Some interactables user can click to interact, others like documents are just to view
/// </summary>
public class InteractableOnTheLeft : InteractableBase
{
    private Vector2 startPosition;
    private Transform startParent;
    public Vector2 StartPosition => startPosition;
    public Transform StartParent => startParent;

    public void SaveStartPositionAndParent()
    {
        startPosition = transform.position;
        startParent  = transform.parent;
    }
}
