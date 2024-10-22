using UnityEngine.EventSystems;

/// <summary>
/// Document prefab for the Right Canvas. User can interact with this document
/// This know if is interactable and have callbacks when user interact with it
/// </summary>
public class DocumentDraggable : InteractableBase, IBeginDragHandler, IEndDragHandler, IDragHandler
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (interactable == false)
            return;

        callbacks.DocumentBeginDrag(this, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (interactable == false)
            return;

        callbacks.DocumentDrag(this, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (interactable == false)
            return;

        callbacks.DocumentEndDrag(this, eventData);
    }
}