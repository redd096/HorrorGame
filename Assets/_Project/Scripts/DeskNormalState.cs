using redd096.v2.ComponentsSystem;
using UnityEngine;

/// <summary>
/// This is the state when player an click to start drag objects
/// </summary>
[System.Serializable]
public class DeskNormalState : State
{
    DeskManager deskManager;

    protected override void OnInit()
    {
        base.OnInit();

        deskManager = StateMachine as DeskManager;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        //check if click a document
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = deskManager.DeskCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 2))
            {
                Document2D doc2d = hitInfo.transform.GetComponentInParent<Document2D>();
                if (doc2d)
                {
                    //set dragging state
                    deskManager.SetBlackboardElement(DeskManager.DRAGGED_OBJECT_BLACKBOARD, doc2d);
                    deskManager.SetState(deskManager.DraggingState);
                }
            }
        }
    }
}
