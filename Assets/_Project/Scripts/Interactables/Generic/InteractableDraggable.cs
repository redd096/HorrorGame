using UnityEngine.EventSystems;

/// <summary>
/// Interactables in the right screen that can be dragged (documents, stamps, etc...)
/// </summary>
public class InteractableDraggable : InteractableOnTheRight
{
    public System.Action onBeginDrag;
    public System.Action onDrag;
    public System.Action onEndDrag;

    public override void OnBeginDrag_Event(PointerEventData eventData)
    {
        base.OnBeginDrag_Event(eventData);

        if (callbacks.InteractableBeginDrag(this, eventData))
        {
            //set last sibling to show in front of every other element
            transform.SetAsLastSibling();
            CopyInScene.transform.SetAsLastSibling();

            onBeginDrag?.Invoke();
        }
    }

    public override void OnDrag_Event(PointerEventData eventData)
    {
        base.OnDrag_Event(eventData);

        if (callbacks.InteractableDrag(this, eventData))
            onDrag?.Invoke();
    }

    public override void OnEndDrag_Event(PointerEventData eventData)
    {
        base.OnEndDrag_Event(eventData);

        if (callbacks.InteractableEndDrag(this, eventData))
        {
            onEndDrag?.Invoke();
        }
    }
}
