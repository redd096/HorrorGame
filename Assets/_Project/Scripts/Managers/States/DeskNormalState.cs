using UnityEngine.EventSystems;
using UnityEngine;

/// <summary>
/// State when player can start drag objects
/// </summary>
public class DeskNormalState : DeskBaseState
{
    public void InteractableBeginDrag(InteractableDraggable interactable, PointerEventData eventData)
    {
        //set dragging state
        deskStateMachine.SetBlackboardElement(DeskStateMachine.DRAGGED_OBJECT_BLACKBOARD, interactable);
        deskStateMachine.SetBlackboardElement(DeskStateMachine.START_DRAG_POSITION_BLACKBOARD, eventData.position);
        deskStateMachine.SetState(deskStateMachine.DraggingState);
    }

    public void BellClick()
    {
        //call next client
        Debug.Log("TODO - click bell and call next client");
    }

    public void ClickAndInstantiateInteractable(InteractableOnTheLeft clickedInteractable, InteractableOnTheRight instantiatedInScene)
    {
        //move both interactables on desk
        DeskManager.instance.AddInteractable(clickedInteractable, instantiatedInScene);
    }

    public void InteractableFromTheRightBeginDrag(InteractableDragFromTheRight interactable, PointerEventData eventData)
    {
        //set drag from the right state
        deskStateMachine.SetBlackboardElement(DeskStateMachine.DRAGGED_OBJECT_BLACKBOARD, interactable);
        deskStateMachine.SetBlackboardElement(DeskStateMachine.START_DRAG_POSITION_BLACKBOARD, eventData.position);
        deskStateMachine.SetState(deskStateMachine.DragFromTheRightState);
    }
}