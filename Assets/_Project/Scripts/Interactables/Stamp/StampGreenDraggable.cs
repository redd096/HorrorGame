using UnityEngine.EventSystems;

/// <summary>
/// Use this stamp to accept client
/// </summary>
public class StampGreenDraggable : StampDraggableBase
{
    protected override void OnStamp(PointerEventData eventData, DocumentDraggable hittedDocument)
    {
        hittedDocument.OnReceiveStamp(true);
    }
}
