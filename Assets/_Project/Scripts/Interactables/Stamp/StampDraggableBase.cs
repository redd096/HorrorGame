using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

/// <summary>
/// Drag this stamp on documents to approve or deny clients
/// </summary>
public abstract class StampDraggableBase : InteractableDraggable
{
    protected abstract void OnStamp(PointerEventData eventData, InteractableOnTheRight hittedInteractable);

    public override void OnBeginDrag_Event(PointerEventData eventData)
    {
        //base.OnBeginDrag_Event(eventData);

        //set transform.position to put always stamp where is the mouse (without offset)
        eventData.position = transform.position;

        if (callbacks.InteractableBeginDrag(this, eventData))
        {
            //set last sibling to show in front of every other element
            transform.SetAsLastSibling();
            CopyInScene.transform.SetAsLastSibling();

            onBeginDrag?.Invoke();
        }
    }

    public override void OnEndDrag_Event(PointerEventData eventData)
    {
        //base.OnEndDrag_Event(eventData);

        if (callbacks.InteractableEndDrag(this, eventData))
        {
            onEndDrag?.Invoke();
            
            //check to remove interactable
            if (DeskManager.instance.CheckToRemoveInteractable(CopyInScene, this))
                return;

            //else, check what hit where there is the shadow
            PointerEventData data = new PointerEventData(EventSystem.current) { position = transform.position };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);

            for (int i = 0; i < results.Count; i++)
            {
                var hit = results[i].gameObject.GetComponent<InteractableOnTheRight>();
                if (hit && hit != this)
                {
                    OnStamp(eventData, hit);
                    break;
                }
            }
        }
    }
}
