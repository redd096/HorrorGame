using UnityEngine.EventSystems;

/// <summary>
/// Interface for DocumentInteract to send events
/// </summary>
public interface IDocumentEvents
{
    void DocumentBeginDrag(DocumentInteract doc, PointerEventData eventData);
    void DocumentDrag(DocumentInteract doc, PointerEventData eventData);
    void DocumentEndDrag(DocumentInteract doc, PointerEventData eventData);
}
