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

    //interactables on the left
    bool BellClick();
    bool CameraClick();

    //stamps
    bool ClickAndInstantiateInteractable(InteractableOnTheLeft clickedInteractable, InteractableOnTheRight instantiatedInScene);

    //board and journal
    bool InteractableFromTheRightBeginDrag(InteractableDragFromTheRight interactable, PointerEventData eventData);
    bool InteractableFromTheRightDrag(InteractableDragFromTheRight interactable, PointerEventData eventData);
    bool InteractableFromTheRightEndDrag(InteractableDragFromTheRight interactable, PointerEventData eventData);
}
