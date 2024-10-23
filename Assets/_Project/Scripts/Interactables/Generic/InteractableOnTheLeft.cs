using UnityEngine;

/// <summary>
/// This is the interactable in "scene". 
/// Some interactables user can click to interact, others like documents are just to view
/// </summary>
public class InteractableOnTheLeft : InteractableBase
{
    private Vector2 startPosition;
    public Vector2 StartPosition => startPosition;

    protected virtual void Start()
    {
        startPosition = transform.position;
    }
}
