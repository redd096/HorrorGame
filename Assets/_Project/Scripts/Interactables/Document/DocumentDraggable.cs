using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This is for every document, to drag and give back to clients
/// </summary>
public class DocumentDraggable : InteractableDraggable
{
    [SerializeField] private bool canReceiveStamp = true;

    public void OnReceiveStamp(bool isGreen)
    {
        //if this document can receive stamp, tell to DeskManager to show area to give documents back to client
        if (canReceiveStamp)
        {
            DeskManager.instance.OnDocumentReceiveStamp();
            
            //TODO
            //tell to another manager to save the stamp color (call always this function, to keep last one as response)
        }
    }
    
    public override void OnEndDrag_Event(PointerEventData eventData)
    {
        //base.OnEndDrag_Event(eventData);

        if (callbacks.InteractableEndDrag(this, eventData))
        {
            onEndDrag?.Invoke();
            
            //check to remove document
            DeskManager.instance.CheckToRemoveDocument(CopyInScene, this);
        }
    }
}
