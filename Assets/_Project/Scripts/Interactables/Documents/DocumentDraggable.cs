using redd096.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This is for every document, to drag and give back to clients
/// </summary>
public class DocumentDraggable : InteractableDraggable
{
    [Tooltip("When user put a stamp on this document, it tells to the game if user accept or deny the client")][SerializeField] private bool canReceiveStamp = true;
    [Tooltip("Used when put this document on the desk, to know if user has to give it back to the client before call another one")][SerializeField] private bool documentToGiveBack = true;
    [Tooltip("If true, user can put this document in the board")][SerializeField] private bool canBePutInsideBoard = false;

    protected bool isInBoardArea;

    public bool DocumentToGiveBack { get => documentToGiveBack; set => documentToGiveBack = value; }
    public bool CanBePutInsideBoard { get => canBePutInsideBoard; set => canBePutInsideBoard = value; }

    public System.Action onEnterBoard;
    public System.Action onExitBoard;

    [Button(ButtonAttribute.EEnableType.PlayMode)]
    void SetDocumentToGiveBack()
    {
        DeskManager.instance.ChangeDocumentStatus(this, true, canBePutInsideBoard);
    }

    [Button(ButtonAttribute.EEnableType.PlayMode)]
    void SetDocumentToNOTGiveBack()
    {
        DeskManager.instance.ChangeDocumentStatus(this, false, canBePutInsideBoard);
    }

    public void OnReceiveStamp(bool isGreen)
    {
        //if this document can receive stamp, tell to DeskManager to show area to give documents back to client
        if (canReceiveStamp && documentToGiveBack)
        {
            DeskManager.instance.OnDocumentReceiveStamp(isGreen);
        }
    }
    
    public override void OnEndDrag_Event(PointerEventData eventData)
    {
        //base.OnEndDrag_Event(eventData);

        if (callbacks.InteractableEndDrag(this, eventData))
        {
            onEndDrag?.Invoke();

            //if this document is inside board area
            if (DeskManager.instance.CheckIsInBoardArea(rectTr))
            {
                //if can be put inside board, then set parent and call event
                if (canBePutInsideBoard)
                {
                    isInBoardArea = true;
                    DeskManager.instance.SetInBoardArea(this, true);
                    onEnterBoard?.Invoke();
                }
            }
            //if not inside board area
            else
            {
                //if before was inside board area, now reset parent and call event
                if (isInBoardArea)
                {
                    isInBoardArea = false;
                    DeskManager.instance.SetInBoardArea(this, false);
                    onExitBoard?.Invoke();
                }

                //check if this is a document you can give back, and give back if now is in documents area
                if (documentToGiveBack)
                {
                    DeskManager.instance.CheckToRemoveDocument(CopyInScene, this, documentToGiveBack);
                }
            }
        }
    }
}
