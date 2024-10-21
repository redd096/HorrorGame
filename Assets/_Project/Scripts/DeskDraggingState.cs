using redd096.v2.ComponentsSystem;
using UnityEngine;

/// <summary>
/// This is the state when player is dragging a document
/// </summary>
[System.Serializable]
public class DeskDraggingState : State
{
    DeskManager deskManager;
    Transform draggedDocument;

    Vector3 offset;
    private const float FIX_BOUNDS = 0.5f;  //used to avoid to put document too much near the border and user can't reach it 

    protected override void OnInit()
    {
        base.OnInit();

        deskManager = StateMachine as DeskManager;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        //get dragged object and calculate offset from where user start drag it
        draggedDocument = deskManager.GetBlackboardElement<Document2D>(DeskManager.DRAGGED_OBJECT_BLACKBOARD).transform;
        offset = draggedDocument.position - deskManager.DeskCamera.ScreenToWorldPoint(Input.mousePosition);
        offset.z = 0;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        //move dragged object
        DragObject();

        //on release, back to normal state
        if (Input.GetMouseButtonUp(0))
        {
            deskManager.SetState(deskManager.NormalState);
        }
    }

    void DragObject()
    {
        //calculate limits camera
        Vector3 min = deskManager.DeskCamera.ViewportToWorldPoint(Vector3.zero);
        Vector3 max = deskManager.DeskCamera.ViewportToWorldPoint(Vector3.one);

        //calculate new position and clamp to avoid put object outside of the screen
        Vector3 worldPosition = deskManager.DeskCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.x = Mathf.Clamp(worldPosition.x, min.x + FIX_BOUNDS, max.x - FIX_BOUNDS);
        worldPosition.y = Mathf.Clamp(worldPosition.y, min.y + FIX_BOUNDS, max.y - FIX_BOUNDS);
        worldPosition += offset;

        //set position
        worldPosition.z = draggedDocument.position.z;
        draggedDocument.position = worldPosition;
    }
}
