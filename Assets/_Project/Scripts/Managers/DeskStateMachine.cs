using redd096.v2.ComponentsSystem;
using UnityEngine.EventSystems;
using UnityEngine;

/// <summary>
/// Statemachine for the desk
/// </summary>
public class DeskStateMachine : BasicStateMachineComponent, IInteractablesEvents
{
    //blackboard
    public const string DRAGGED_OBJECT_BLACKBOARD = "DraggedObject";
    public const string START_DRAG_POSITION_BLACKBOARD = "StartDragPosition";

    public DeskNormalState NormalState = new DeskNormalState();
    public DeskDraggingDocumentState DraggingDocumentState = new DeskDraggingDocumentState();

    private void Awake()
    {
        IInteractablesEvents.Instance = this;

        //set start state
        SetState(NormalState);
    }

    #region interactables events

    public void DocumentBeginDrag(DocumentDraggable doc, PointerEventData eventData)
    {
        NormalState.DocumentBeginDrag(doc, eventData);
    }

    public void DocumentDrag(DocumentDraggable doc, PointerEventData eventData)
    {
        DraggingDocumentState.DocumentDrag(doc, eventData);
    }

    public void DocumentEndDrag(DocumentDraggable doc, PointerEventData eventData)
    {
        DraggingDocumentState.DocumentEndDrag(doc, eventData);
    }

    public void BellClick()
    {
        Debug.Log("TODO - click bell and call next client");
    }

    public void InstantiatedDraggableClick(InteractableBase objectInstanceInScene)
    {
        DeskManager.instance.AddInteractable(objectInstanceInScene);
    }

    #endregion
}
