using UnityEngine.EventSystems;

/// <summary>
/// Object on the total right of the screen, you can click and drag to cover the right screen where you move documents
/// </summary>
public class InteractableDragFromTheRight : InteractableBase
{
    public override void OnBeginDrag_Event(PointerEventData eventData)
    {
        base.OnBeginDrag_Event(eventData);

        callbacks.InteractableFromTheRightBeginDrag(this, eventData);
    }

    public override void OnDrag_Event(PointerEventData eventData)
    {
        base.OnDrag_Event(eventData);

        callbacks.InteractableFromTheRightDrag(this, eventData);
    }

    public override void OnEndDrag_Event(PointerEventData eventData)
    {
        base.OnEndDrag_Event(eventData);

        callbacks.InteractableFromTheRightEndDrag(this, eventData);
    }
}
