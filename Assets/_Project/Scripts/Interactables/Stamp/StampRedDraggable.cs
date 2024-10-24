using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Use this stamp to deny client
/// </summary>
public class StampRedDraggable : StampDraggableBase
{
    protected override void OnStamp(PointerEventData eventData, DocumentDraggable hittedDocument)
    {
        hittedDocument.OnReceiveStamp(false);
    }
}
