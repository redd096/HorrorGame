using UnityEngine.EventSystems;

/// <summary>
/// Document prefab for the Right Canvas. User can interact with this document
/// This know if is interactable and have callbacks when user interact with it
/// </summary>
public class DocumentDraggable : InteractableBase
{
    private DocumentScene docScene;
    public DocumentScene DocScene => docScene;

    /// <summary>
    /// Initialize document with a reference to its copy used in Left side just for the scene
    /// </summary>
    /// <param name="docScene"></param>
    public void Init(DocumentScene docScene)
    {
        this.docScene = docScene;
    }

    public override void OnBeginDrag_Event(PointerEventData eventData)
    {
        base.OnBeginDrag_Event(eventData);

        callbacks.DocumentBeginDrag(this, eventData);
    }

    public override void OnDrag_Event(PointerEventData eventData)
    {
        base.OnDrag_Event(eventData);

        callbacks.DocumentDrag(this, eventData);
    }

    public override void OnEndDrag_Event(PointerEventData eventData)
    {
        base.OnEndDrag_Event(eventData);

        callbacks.DocumentEndDrag(this, eventData);
    }
}