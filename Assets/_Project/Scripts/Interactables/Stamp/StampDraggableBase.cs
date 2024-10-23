using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Drag this stamp on documents to approve or deny clients
/// </summary>
public abstract class StampDraggableBase : InteractableBase
{
    [SerializeField] GameObject shadow;

    protected virtual void ShowShadow(bool show)
    {
        shadow.SetActive(show);
    }

    protected abstract void OnStamp(PointerEventData eventData);

    public override void OnBeginDrag_Event(PointerEventData eventData)
    {
        base.OnBeginDrag_Event(eventData);

        Debug.Log("TODO - draggare stampino");
    }

    public override void OnDrag_Event(PointerEventData eventData)
    {
        base.OnDrag_Event(eventData);
    }

    public override void OnEndDrag_Event(PointerEventData eventData)
    {
        base.OnEndDrag_Event(eventData);
    }
}
