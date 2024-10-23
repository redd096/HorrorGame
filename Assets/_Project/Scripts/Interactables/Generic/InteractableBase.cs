using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Every interactable in scene
/// </summary>
public abstract class InteractableBase : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    protected IInteractablesEvents callbacks;
    protected bool interactable;

    public bool Interactable => interactable;

    /// <summary>
    /// Initialize and set interactable
    /// </summary>
    /// <param name="callbacks"></param>
    public void Init(IInteractablesEvents callbacks)
    {
        this.callbacks = callbacks;
        interactable = true;
    }

    /// <summary>
    /// Set if user can interact/drag this object
    /// </summary>
    /// <param name="interactable"></param>
    public void SetInteractable(bool interactable)
    {
        this.interactable = interactable;
    }

    #region interfaces events

    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactable == false)
            return;

        OnPointerClick_Event(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (interactable == false)
            return;

        OnBeginDrag_Event(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (interactable == false)
            return;

        OnDrag_Event(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (interactable == false)
            return;

        OnEndDrag_Event(eventData);
    }

    #endregion

    #region virtual functions

    public virtual void OnPointerClick_Event(PointerEventData eventData)
    { }
    public virtual void OnBeginDrag_Event(PointerEventData eventData)
    { }
    public virtual void OnDrag_Event(PointerEventData eventData)
    { }
    public virtual void OnEndDrag_Event(PointerEventData eventData)
    { }

    #endregion
}
