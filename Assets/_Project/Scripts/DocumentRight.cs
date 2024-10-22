using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Document prefab for the Right Canvas. 
/// This know if is interactable and tell to DeskManager when user interact with it
/// </summary>
public class DocumentRight : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private DocumentLeft docLeft;
    private bool interactable;

    /// <summary>
    /// Initialize document
    /// </summary>
    /// <param name="docLeft"></param>
    public void Init(DocumentLeft docLeft)
    {
        this.docLeft = docLeft;
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

        DeskManager.instance.DocumentBeginDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (interactable == false)
            return;

        DeskManager.instance.DocumentDrag(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (interactable == false)
            return;

        DeskManager.instance.DocumentEndDrag(this);
    }

    #endregion
}