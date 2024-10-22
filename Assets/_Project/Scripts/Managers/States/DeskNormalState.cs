using UnityEngine.EventSystems;

/// <summary>
/// State when player can start drag objects
/// </summary>
public class DeskNormalState : DeskBaseState
{
    public void DocumentBeginDrag(DocumentDraggable doc, PointerEventData eventData)
    {
        //only in correct state
        if (IsActive == false)
            return;

        //set dragging state
        deskStateMachine.SetBlackboardElement(DeskStateMachine.DRAGGED_OBJECT_BLACKBOARD, doc);
        deskStateMachine.SetBlackboardElement(DeskStateMachine.START_DRAG_POSITION_BLACKBOARD, eventData.position);
        deskStateMachine.SetState(deskStateMachine.DraggingDocumentState);
    }
}
