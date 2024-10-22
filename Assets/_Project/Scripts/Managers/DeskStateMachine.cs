using redd096.v2.ComponentsSystem;
using UnityEngine.EventSystems;

/// <summary>
/// Statemachine for the desk
/// </summary>
public class DeskStateMachine : BasicStateMachineComponent, IDocumentEvents
{
    //blackboard
    public const string DRAGGED_OBJECT_BLACKBOARD = "DraggedObject";
    public const string START_DRAG_POSITION_BLACKBOARD = "StartDragPosition";

    public DeskNormalState NormalState = new DeskNormalState();
    public DeskDraggingDocumentState DraggingDocumentState = new DeskDraggingDocumentState();

    private void Awake()
    {
        //set start state
        SetState(NormalState);
    }

    #region documents events

    public void DocumentBeginDrag(DocumentInteract doc, PointerEventData eventData)
    {
        NormalState.DocumentBeginDrag(doc, eventData);
    }

    public void DocumentDrag(DocumentInteract doc, PointerEventData eventData)
    {
        DraggingDocumentState.DocumentDrag(doc, eventData);
    }

    public void DocumentEndDrag(DocumentInteract doc, PointerEventData eventData)
    {
        DraggingDocumentState.DocumentEndDrag(doc, eventData);
    }

    #endregion
}
