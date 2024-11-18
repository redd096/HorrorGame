using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Object on the total right of the screen, you can click and drag to cover the right screen where you move documents
/// </summary>
public class InteractableDragFromTheRight : InteractableBase
{
    //used to avoid to put dragged object too much to the left and see a bit of the background
    private const float FIX_BOUNDS = 5f;
    
    //position on open or close
    public float OpenPosition => -rectTr.sizeDelta.x + FIX_BOUNDS;
    public float ClosePosition => startAnchoredPosition.x;
    
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

    /// <summary>
    /// Move RectTransform to ClosePosition
    /// </summary>
    public void ForceClose()
    {
        Vector2 anchoredPosition = rectTr.anchoredPosition;
        anchoredPosition.x = ClosePosition;
        rectTr.anchoredPosition = anchoredPosition;
        
    }

    /// <summary>
    /// Move RectTransform to OpenPosition
    /// </summary>
    public void ForceOpen()
    {
        Vector2 anchoredPosition = rectTr.anchoredPosition;
        anchoredPosition.x = OpenPosition;
        rectTr.anchoredPosition = anchoredPosition;
    }
}
