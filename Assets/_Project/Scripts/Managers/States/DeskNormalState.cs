using UnityEngine.EventSystems;
using UnityEngine;

/// <summary>
/// State when player can start drag objects
/// </summary>
public class DeskNormalState : DeskBaseState
{
    public void InteractableBeginDrag(InteractableDraggable interactable, PointerEventData eventData)
    {
        if (IsActive == false)
            return;

        //set dragging state
        deskStateMachine.SetBlackboardElement(DeskStateMachine.DRAGGED_OBJECT_BLACKBOARD, interactable);
        deskStateMachine.SetBlackboardElement(DeskStateMachine.START_DRAG_POSITION_BLACKBOARD, eventData.position);
        deskStateMachine.SetState(deskStateMachine.DraggingState);
    }

    public void BellClick()
    {
        if (IsActive == false)
            return;

        //call next client
        Debug.Log("TODO - click bell and call next client");
    }

    public void ClickAndInstantiateInteractable(InteractableOnTheLeft clickedInteractable, InteractableOnTheRight instantiatedInScene)
    {
        if (IsActive == false)
            return;

        //move both interactables on desk
        DeskManager.instance.AddInteractable(clickedInteractable, instantiatedInScene);
    }
}
