using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// State when player is dragging a document
/// </summary>
public class DeskDraggingDocumentState : DeskBaseState
{
    private DocumentDraggable draggedDocument;
    private Vector2 offset;

    private Transform docInteractTr;
    private Transform docSceneTr;
    private RectTransform docInteractContainer;
    private RectTransform docSceneContainer;
    private Canvas docInteractCanvas;
    private Canvas docSceneCanvas;

    //used to avoid to put document too much near the border and user can't reach it. It's a percentage based on interactContainer's size
    private Vector2 FIX_BOUNDS = new Vector2(0.03f, 0.03f);

    protected override void OnEnter()
    {
        base.OnEnter();

        //save refs
        draggedDocument = deskStateMachine.GetBlackboardElement<DocumentDraggable>(DeskStateMachine.DRAGGED_OBJECT_BLACKBOARD);
        offset = (Vector2)draggedDocument.transform.position - deskStateMachine.GetBlackboardElement<Vector2>(DeskStateMachine.START_DRAG_POSITION_BLACKBOARD);

        docInteractTr = draggedDocument.transform;
        docSceneTr = draggedDocument.DocScene.transform;
        docInteractContainer = docInteractTr.parent.GetComponent<RectTransform>();
        docSceneContainer = docSceneTr.parent.GetComponent<RectTransform>();
        docInteractCanvas = docInteractTr.GetComponentInParent<Canvas>();
        docSceneCanvas = docSceneTr.GetComponentInParent<Canvas>();
    }

    public void DocumentDrag(DocumentDraggable doc, PointerEventData eventData)
    {
        //only in correct state and if this document is the dragged one
        if (IsActive == false || draggedDocument != doc)
            return;

        //calculate limits
        Vector2 center = docInteractContainer.position;
        Vector2 size = docInteractContainer.rect.size * docInteractCanvas.scaleFactor;
        Vector2 halfSize = size * 0.5f;
        Vector2 min = center - halfSize + (size * FIX_BOUNDS);
        Vector2 max = center + halfSize - (size * FIX_BOUNDS);

        //calculate new position and clamp
        Vector2 position = eventData.position;
        position.x = Mathf.Clamp(position.x, min.x, max.x);
        position.y = Mathf.Clamp(position.y, min.y, max.y);

        //update position
        position += offset;
        docInteractTr.position = position;

        //Debug.DrawLine(new Vector2(min.x, min.y), new Vector2(min.x, max.y), Color.red, 10);
        //Debug.DrawLine(new Vector2(min.x, max.y), new Vector2(max.x, max.y), Color.red, 10);
        //Debug.DrawLine(new Vector2(max.x, max.y), new Vector2(max.x, min.y), Color.red, 10);
        //Debug.DrawLine(new Vector2(max.x, min.y), new Vector2(min.x, min.y), Color.red, 10);
        //Vector2 tempMin = center - halfSize;
        //Vector2 tempMax = center + halfSize;
        //Debug.DrawLine(new Vector2(tempMin.x, tempMin.y), new Vector2(tempMin.x, tempMax.y), Color.cyan, 10);
        //Debug.DrawLine(new Vector2(tempMin.x, tempMax.y), new Vector2(tempMax.x, tempMax.y), Color.cyan, 10);
        //Debug.DrawLine(new Vector2(tempMax.x, tempMax.y), new Vector2(tempMax.x, tempMin.y), Color.cyan, 10);
        //Debug.DrawLine(new Vector2(tempMax.x, tempMin.y), new Vector2(tempMin.x, tempMin.y), Color.cyan, 10);

        //update scene position
        Vector2 sceneCenter = docSceneContainer.position;
        Vector2 sceneSize = docSceneContainer.rect.size * docSceneCanvas.scaleFactor;
        Vector2 movement = position - center;
        Vector2 remappedMovement = movement * sceneSize / size;
        docSceneTr.position = remappedMovement + sceneCenter;

        //Vector2 tempSceneMin = sceneCenter - (sceneSize * 0.5f);
        //Vector2 tempSceneMax = sceneCenter + (sceneSize * 0.5f);
        //Debug.DrawLine(new Vector2(tempSceneMin.x, tempSceneMin.y), new Vector2(tempSceneMin.x, tempSceneMax.y), Color.cyan, 10);
        //Debug.DrawLine(new Vector2(tempSceneMin.x, tempSceneMax.y), new Vector2(tempSceneMax.x, tempSceneMax.y), Color.cyan, 10);
        //Debug.DrawLine(new Vector2(tempSceneMax.x, tempSceneMax.y), new Vector2(tempSceneMax.x, tempSceneMin.y), Color.cyan, 10);
        //Debug.DrawLine(new Vector2(tempSceneMax.x, tempSceneMin.y), new Vector2(tempSceneMin.x, tempSceneMin.y), Color.cyan, 10);
    }

    public void DocumentEndDrag(DocumentDraggable doc, PointerEventData eventData)
    {
        //only in correct state and if this document is the dragged one
        if (IsActive == false || draggedDocument != doc)
            return;

        //back to normal state
        deskStateMachine.SetState(deskStateMachine.NormalState);
    }
}
