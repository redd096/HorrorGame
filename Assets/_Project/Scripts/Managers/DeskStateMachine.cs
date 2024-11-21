using redd096.v2.ComponentsSystem;
using UnityEngine.EventSystems;

/// <summary>
/// Statemachine for the desk
/// </summary>
public class DeskStateMachine : BasicStateMachineComponent, IInteractablesEvents
{
    //blackboard
    public const string DRAGGED_OBJECT_BLACKBOARD = "DraggedObject";
    public const string START_DRAG_POSITION_BLACKBOARD = "StartDragPosition";

    public DeskNormalState NormalState = new DeskNormalState();
    public DeskDraggingState DraggingState = new DeskDraggingState();
    public DeskDragFromTheRightState DragFromTheRightState = new DeskDragFromTheRightState();

    #region interactables events

    public bool InteractableBeginDrag(InteractableDraggable interactable, PointerEventData eventData)
    {
        if (CurrentState == NormalState)
        {
            NormalState.InteractableBeginDrag(interactable, eventData);
            return true;
        }
        return false;
    }

    public bool InteractableDrag(InteractableDraggable interactable, PointerEventData eventData)
    {
        if (CurrentState == DraggingState)
        {
            DraggingState.InteractableDrag(interactable, eventData);
            return true;
        }
        return false;
    }

    public bool InteractableEndDrag(InteractableDraggable interactable, PointerEventData eventData)
    {
        if (CurrentState == DraggingState)
        {
            DraggingState.InteractableEndDrag(interactable, eventData);
            return true;
        }
        return false;
    }

    public bool BellClick()
    {
        //call next client
        DeskManager.instance.OnPlayerClickBell();
        return true;
    }

    public bool CameraClick()
    {
        //call camera flash event
        DeskManager.instance.OnPlayerClickCamera();
        return true;
    }

    public bool ClickAndInstantiateInteractable(InteractableOnTheLeft clickedInteractable, InteractableOnTheRight instantiatedInScene)
    {
        //move both interactables on desk
        DeskManager.instance.AddInteractableFromDesk(clickedInteractable, instantiatedInScene);
        return true;
    }

    public bool InteractableFromTheRightBeginDrag(InteractableDragFromTheRight interactable, PointerEventData eventData)
    {
        if (CurrentState == NormalState)
        {
            NormalState.InteractableFromTheRightBeginDrag(interactable, eventData);
            return true;
        }
        return false;
    }

    public bool InteractableFromTheRightDrag(InteractableDragFromTheRight interactable, PointerEventData eventData)
    {
        if (CurrentState == DragFromTheRightState)
        {
            DragFromTheRightState.InteractableFromTheRightDrag(interactable, eventData);
            return true;
        }
        return false;
    }

    public bool InteractableFromTheRightEndDrag(InteractableDragFromTheRight interactable, PointerEventData eventData)
    {
        if (CurrentState == DragFromTheRightState)
        {
            DragFromTheRightState.InteractableFromTheRightEndDrag(interactable, eventData);
            return true;
        }
        return false;
    }

    #endregion
}
