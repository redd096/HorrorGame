using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// State when player is draggin an object from the total right of the screen, to cover the right screen
/// </summary>
public class DeskDragFromTheRightState : DeskBaseState
{
    private InteractableDragFromTheRight draggedObject;
    private Vector2 offset;

    private RectTransform draggedRectTr;

    //used to avoid to put dragged object too much to the left, and see a bit of the background
    private float FIX_BOUNDS = 5f;

    protected override void OnEnter()
    {
        base.OnEnter();

        //save refs
        draggedObject = deskStateMachine.GetBlackboardElement<InteractableDragFromTheRight>(DeskStateMachine.DRAGGED_OBJECT_BLACKBOARD);
        offset = (Vector2)draggedObject.transform.position - deskStateMachine.GetBlackboardElement<Vector2>(DeskStateMachine.START_DRAG_POSITION_BLACKBOARD);

        draggedRectTr = draggedObject.RectTr;
    }

    public void InteractableFromTheRightDrag(InteractableDragFromTheRight interactable, PointerEventData eventData)
    {
        //only if this interactable is the dragged one
        if (draggedObject != interactable)
            return;

        //can drag only in horizontal
        Vector2 position = draggedRectTr.position;
        position.x = eventData.position.x;
        position.x += offset.x;

        //update position
        draggedRectTr.position = position;

        //clamp
        Vector2 anchoredPosition = draggedRectTr.anchoredPosition;
        anchoredPosition.x = Mathf.Clamp(draggedRectTr.anchoredPosition.x, -draggedRectTr.sizeDelta.x + FIX_BOUNDS, draggedObject.StartAnchoredPosition.x);
        draggedRectTr.anchoredPosition = anchoredPosition;
    }

    public void InteractableFromTheRightEndDrag(InteractableDragFromTheRight interactable, PointerEventData eventData)
    {
        //only if this interactable is the dragged one
        if (draggedObject != interactable)
            return;

        //back to normal state
        deskStateMachine.SetState(deskStateMachine.NormalState);
    }
}
