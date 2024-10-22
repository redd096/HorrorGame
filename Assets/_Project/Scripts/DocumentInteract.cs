using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Document prefab for the Right Canvas. User can interact with this document
/// This know if is interactable and have callbacks when user interact with it
/// </summary>
public class DocumentInteract : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private DocumentScene docScene;
    private IDocumentEvents callbacks;

    private bool interactable;

    //public
    public DocumentScene DocScene => docScene;
    public bool Interactable => interactable;

    /// <summary>
    /// Initialize document with a reference to its copy used in Left side just for the scene
    /// </summary>
    /// <param name="docScene"></param>
    public void Init(DocumentScene docScene, IDocumentEvents callbacks)
    {
        this.docScene = docScene;
        this.callbacks = callbacks;
    }

    /// <summary>
    /// Set if user can interact/drag this document 
    /// </summary>
    /// <param name="interactable"></param>
    public void SetInteractable(bool interactable)
    {
        this.interactable = interactable;
    }

    #region interface events

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

    #endregion
}