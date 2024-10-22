using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Interface for interactables to send events (e.g. DocumentInteract, Bell...)
/// </summary>
public interface IInteractablesEvents
{
    //documents
    void DocumentBeginDrag(DocumentDraggable doc, PointerEventData eventData);
    void DocumentDrag(DocumentDraggable doc, PointerEventData eventData);
    void DocumentEndDrag(DocumentDraggable doc, PointerEventData eventData);

    //bell
    void BellClick();

    //stamps
    void InstantiatedDraggableClick(GameObject objectInstanceInScene);
}
