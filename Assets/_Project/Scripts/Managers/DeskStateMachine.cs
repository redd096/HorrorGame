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
    public DeskDraggingState DraggingState = new DeskDraggingState();

    private void Awake()
    {
        //set callbacks for every element in scene
        InteractableBase[] interactablesInScene = FindObjectsOfType<InteractableBase>();
        foreach (var interactable in interactablesInScene)
            interactable.Init(this);

        //set start state
        SetState(NormalState);
    }

    #region interactables events

    public bool InteractableBeginDrag(InteractableDraggable interactable, PointerEventData eventData)
    {
        bool result = CurrentState == NormalState;
        NormalState.InteractableBeginDrag(interactable, eventData);
        return result;
    }

    public bool InteractableDrag(InteractableDraggable interactable, PointerEventData eventData)
    {
        bool result = CurrentState == DraggingState;
        DraggingState.InteractableDrag(interactable, eventData);
        return result;
    }

    public bool InteractableEndDrag(InteractableDraggable interactable, PointerEventData eventData)
    {
        bool result = CurrentState == DraggingState;
        DraggingState.InteractableEndDrag(interactable, eventData);
        return result;
    }

    public bool BellClick()
    {
        bool result = CurrentState == NormalState;
        NormalState.BellClick();
        return result;
    }

    public bool ClickAndInstantiateInteractable(InteractableOnTheLeft clickedInteractable, InteractableOnTheRight instantiatedInScene)
    {
        bool result = CurrentState == NormalState;
        NormalState.ClickAndInstantiateInteractable(clickedInteractable, instantiatedInScene);
        return result;
    }

    #endregion
}
