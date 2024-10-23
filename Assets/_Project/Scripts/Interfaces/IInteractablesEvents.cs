using UnityEngine.EventSystems;

/// <summary>
/// Interface for interactables to send events (e.g. Document, Bell...)
/// </summary>
public interface IInteractablesEvents
{
    //documents
    bool InteractableBeginDrag(InteractableDraggable interactable, PointerEventData eventData);
    bool InteractableDrag(InteractableDraggable interactable, PointerEventData eventData);
    bool InteractableEndDrag(InteractableDraggable interactable, PointerEventData eventData);

    //bell
    bool BellClick();

    //stamps
    bool ClickAndInstantiateInteractable(InteractableOnTheLeft clickedInteractable, InteractableOnTheRight instantiatedInScene);
}
