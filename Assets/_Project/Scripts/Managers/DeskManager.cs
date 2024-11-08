using UnityEngine;
using PrimeTween;
using redd096;
using System.Collections.Generic;

/// <summary>
/// This manages UI
/// </summary>
public class DeskManager : SimpleInstance<DeskManager>
{
    [SerializeField] DeskStateMachine stateMachine;
    [SerializeField] DeskWindowsManager deskWindowsManager;
    [SerializeField] RectTransform draggedObjectsContainer;

    [Header("Put interactables animation")]
    [SerializeField] Transform leftContainer;
    [SerializeField] Transform leftStartTopPoint;
    [SerializeField] Transform leftStartBottomPoint;
    [SerializeField] Transform leftEndPoint;
    [SerializeField] Transform rightContainer;
    [SerializeField] Transform rightStartTopPoint;
    [SerializeField] Transform rightStartBottomPoint;
    [SerializeField] Transform rightEndPoint;
    [SerializeField] float putAnimationTime = 1;
    
    [Header("Documents Prefabs")]
    [SerializeField] InteractableOnTheLeft warningLeft;
    [SerializeField] WarningDraggable warningRight;
    [SerializeField] InteractableOnTheLeft idCardLeft;
    [SerializeField] IDCardDraggable idCardRight;
    [SerializeField] InteractableOnTheLeft renunciationCardLeft;
    [SerializeField] RenunciationCardDraggable renunciationCardRight;
    [SerializeField] InteractableOnTheLeft residentCardLeft;
    [SerializeField] ResidentCardDraggable residentCardRight;
    [SerializeField] InteractableOnTheLeft policeCardLeft;
    [SerializeField] PoliceCardDraggable policeCardRight;
    [SerializeField] InteractableOnTheLeft appointmentCardLeft;
    [SerializeField] AppointmentCardDraggable appointmentCardRight;

    //list of documents to give back to customer, or interactables to put back on desk
    private List<InteractableOnTheRight> documentsToGiveBack = new List<InteractableOnTheRight>();
    private List<InteractableOnTheRight> interactablesToPutBack = new List<InteractableOnTheRight>();

    //list of every interactable in scene (both documents and interactables. There are also objects to not give back, like customers' gifts)
    private List<InteractableOnTheRight> interactablesInScene = new List<InteractableOnTheRight>();

    public List<InteractableOnTheRight> InteractablesInScene => interactablesInScene;
    public RectTransform DraggedObjectsContainer => draggedObjectsContainer;

    #region instantiate document

    public void InstantiateWarning(int counter, string message)
    {
        //instantiate and initialize, then add in scene
        InstantiateGenericDocument(warningLeft, warningRight, out var left, out var right);
        ((WarningDraggable)right).InitDocument(counter, message);

        AddInteractable(left, right, true, false, false, false);
    }

    public void InstantiateDocument(IDCard doc)
    {
        //instantiate and initialize, then add in scene
        InstantiateGenericDocument(idCardLeft, idCardRight, out var left, out var right);
        ((IDCardDraggable)right).InitDocument(doc);

        AddDocument(left, right);
    }

    public void InstantiateDocument(RenunciationCard doc)
    {
        //instantiate and initialize, then add in scene
        InstantiateGenericDocument(renunciationCardLeft, renunciationCardRight, out var left, out var right);
        ((RenunciationCardDraggable)right).InitDocument(doc);

        AddDocument(left, right);
    }

    public void InstantiateDocument(ResidentCard doc)
    {
        //instantiate and initialize, then add in scene
        InstantiateGenericDocument(residentCardLeft, residentCardRight, out var left, out var right);
        ((ResidentCardDraggable)right).InitDocument(doc);

        AddDocument(left, right);
    }

    public void InstantiateDocument(PoliceCard doc)
    {
        //instantiate and initialize, then add in scene
        InstantiateGenericDocument(policeCardLeft, policeCardRight, out var left, out var right);
        ((PoliceCardDraggable)right).InitDocument(doc);

        AddDocument(left, right);
    }

    public void InstantiateDocument(AppointmentCard doc)
    {
        //instantiate and initialize, then add in scene
        InstantiateGenericDocument(appointmentCardLeft, appointmentCardRight, out var left, out var right);
        ((AppointmentCardDraggable)right).InitDocument(doc);

        AddDocument(left, right);
    }

    private void InstantiateGenericDocument(InteractableOnTheLeft leftPrefab, DocumentDraggable rightPrefab, out InteractableOnTheLeft instanceLeft, out DocumentDraggable instanceRight)
    {
        if (leftPrefab == null || rightPrefab == null)
            Debug.LogError($"Missing left prefab: {leftPrefab == null} - Missing right prefab: {rightPrefab == null}");

        //instantiate and initialize
        instanceLeft = Instantiate(leftPrefab);
        instanceRight = Instantiate(rightPrefab);

        instanceLeft.Init(stateMachine);
        instanceRight.Init(stateMachine, instanceLeft);
    }

    #endregion

    /// <summary>
    /// Add documents already instantiated, both left and right
    /// </summary>
    /// <param name="isDocumentToGiveBack">After stamp, show area to give back documents</param>
    private void AddDocument(InteractableOnTheLeft docInScene, InteractableOnTheRight docDraggable, bool isDocumentToGiveBack = true)
    {
        AddInteractable(docInScene, docDraggable, true, true, isDocumentToGiveBack, false);
    }

    /// <summary>
    /// Call this when click objects on the desk to the left. Move object on the left and add object already instantiated on the right
    /// </summary>
    public void AddInteractableFromDesk(InteractableOnTheLeft left, InteractableOnTheRight right)
    {
        AddInteractable(left, right, false, false, false, true);
    }

    /// <summary>
    /// Check if inside documents area. If inside, remove document from the scene
    /// </summary>
    /// <param name="isDocumentToGiveBack">After give back every document, hide area to give back documents</param>
    public bool CheckToRemoveDocument(InteractableOnTheLeft docInScene, InteractableOnTheRight docDraggable)
    {
        return CheckToRemoveInteractable(docInScene, docDraggable, true, false);
    }

    /// <summary>
    /// Check if inside interactables area. If inside, put back interactable on the desk
    /// </summary>
    /// <returns></returns>
    public bool CheckToRemoveInteractableOnDesk(InteractableOnTheLeft left, InteractableOnTheRight right)
    {
        return CheckToRemoveInteractable(left, right, false, true);
    }
    
    /// <summary>
    /// When a document receive stamp, show area to give back to client
    /// </summary>
    public void OnDocumentReceiveStamp(bool isGreen)
    {
        deskWindowsManager.ShowDocumentsArea(true);
        LevelManager.instance.OnDocumentReceiveStamp(isGreen);
    }

    /// <summary>
    /// If a document wasn't to give back but now yes, or viceversa
    /// </summary>
    /// <param name="nowIsGiveBackToClient">User must give back this document to the client</param>
    /// <param name="nowCanBePutInsideBoard">Normally you want this bool to be the opposite of nowIsGiveBackToClient</param>
    /// <param name="automaticallyShowOrHideDocumentsArea">If now this documents is to give back, show documents area. If not, if there aren't other documents to give back, hide area</param>
    public void ChangeDocumentStatus(DocumentDraggable docDraggable, bool nowIsGiveBackToClient, bool nowCanBePutInsideBoard, bool automaticallyShowOrHideDocumentsArea = true)
    {
        //set if can go inside board
        docDraggable.CanBePutInsideBoard = nowCanBePutInsideBoard;

        //if changing status
        if (docDraggable.DocumentToGiveBack != nowIsGiveBackToClient)
        {
            docDraggable.DocumentToGiveBack = nowIsGiveBackToClient;

            //add to documents to give back
            if (nowIsGiveBackToClient)
            {
                documentsToGiveBack.Add(docDraggable);
                if (automaticallyShowOrHideDocumentsArea)
                    deskWindowsManager.ShowDocumentsArea(true);
            }
            //remove from documents to give back
            else
            {
                documentsToGiveBack.Remove(docDraggable);
                if (automaticallyShowOrHideDocumentsArea && documentsToGiveBack.Count <= 0)
                    deskWindowsManager.ShowDocumentsArea(false);
            }
        }
    }

    /// <summary>
    /// Return true if this document is inside Board area
    /// </summary>
    /// <param name="rectTr"></param>
    /// <returns></returns>
    public bool CheckIsInBoardArea(RectTransform rectTr)
    {
        return deskWindowsManager.CheckIsInBoardArea(rectTr);
    }

    /// <summary>
    /// Set in or out of the board area
    /// </summary>
    /// <param name="docDraggable"></param>
    /// <param name="isInBoardArea"></param>
    public void SetInBoardArea(InteractableOnTheRight docDraggable, bool isInBoardArea)
    {
        if (isInBoardArea)
        {
            //set parent, and hide left 
            Vector2 pos = docDraggable.transform.position;
            docDraggable.transform.SetParent(deskWindowsManager.BoardArea, false);
            docDraggable.transform.position = pos;

            docDraggable.CopyInScene.ShowInScene(false);
        }
        else
        {
            //back to document container, and show left
            Vector2 pos = docDraggable.transform.position;
            docDraggable.transform.SetParent(rightContainer, false);
            docDraggable.transform.position = pos;

            docDraggable.CopyInScene.ShowInScene(true);
        }
    }

    #region private API

    /// <summary>
    /// Add an object already instantiated, both left and right
    /// </summary>
    /// <param name="isDocumentToGiveBack">After stamp, show area to give back documents</param>
    /// <param name="isInteractableToPutBack">Show area to put back object on the desk</param>
    private void AddInteractable(InteractableOnTheLeft left, InteractableOnTheRight right, bool setStartPositionAlsoOnLeft, bool fromTop, bool isDocumentToGiveBack, bool isInteractableToPutBack)
    {
        interactablesInScene.Add(right);

        //update documents counter
        if (isDocumentToGiveBack)
            documentsToGiveBack.Add(right);

        //update interactables counter and show area
        if (isInteractableToPutBack)
        {
            interactablesToPutBack.Add(right);
            if (interactablesToPutBack.Count > 0)
                deskWindowsManager.ShowInteractablesArea(true);
        }

        right.gameObject.SetActive(true);

        //set parent
        Vector2 posLeft = left.transform.position;
        Vector2 posRight = right.transform.position;
        left.transform.SetParent(leftContainer, false);
        right.transform.SetParent(rightContainer, false);
        left.transform.position = posLeft;
        right.transform.position = posRight;

        left.SetInteractable(false);
        right.SetInteractable(false);

        //move left
        Transform leftStartPoint = fromTop ? leftStartTopPoint : leftStartBottomPoint;
        if (setStartPositionAlsoOnLeft)
            Tween.Position(left.transform, leftStartPoint.position, leftEndPoint.position, putAnimationTime);
        else
            Tween.Position(left.transform, leftEndPoint.position, putAnimationTime);

        //move right
        Transform rightStartPoint = fromTop ? rightStartTopPoint : rightStartBottomPoint;
        Tween.Position(right.transform, rightStartPoint.position, rightEndPoint.position, putAnimationTime)
            .OnComplete(() => right.SetInteractable(true));
    }

    /// <summary>
    /// Check if inside documents or interactables area. If inside, give back document to the customer or put back interactable
    /// </summary>
    private bool CheckToRemoveInteractable(InteractableOnTheLeft left, InteractableOnTheRight right, bool isDocumentToGiveBack, bool isInteractableToPutBack)
    {
        bool removeInteractable = false;

        //check document or interactable to put on the desk
        if (isDocumentToGiveBack)
            removeInteractable = deskWindowsManager.CheckIsInGiveDocumentsArea(right.transform.position);
        else if (isInteractableToPutBack)
            removeInteractable = deskWindowsManager.CheckIsInPutBackInteractablesArea(right.transform.position);

        if (removeInteractable)
        {
            interactablesInScene.Remove(right);

            left.SetInteractable(false);
            right.SetInteractable(false);

            //update documents counter and check to hide area
            if (isDocumentToGiveBack)
            {
                documentsToGiveBack.Remove(right);
                if (documentsToGiveBack.Count <= 0)
                {
                    deskWindowsManager.ShowDocumentsArea(false);
                    LevelManager.instance.OnGiveBackAllDocuments();
                }

                //move out of the desk and destroy
                Tween.Position(left.transform, leftStartTopPoint.position, putAnimationTime).OnComplete(() => Destroy(left.gameObject));
                Tween.Position(right.transform, rightStartTopPoint.position, putAnimationTime).OnComplete(() => Destroy(right.gameObject));
            }
            //update interactables counter and check to hide area
            else if (isInteractableToPutBack)
            {
                interactablesToPutBack.Remove(right);
                if (interactablesToPutBack.Count <= 0)
                {
                    deskWindowsManager.ShowInteractablesArea(false);
                }

                //set parent
                Vector2 posLeft = left.transform.position;
                left.transform.SetParent(left.StartParent, false);
                left.transform.position = posLeft;

                //move out of the desk
                Tween.UIAnchoredPosition(left.RectTr, left.StartAnchoredPosition, putAnimationTime);
                Tween.Position(right.transform, rightStartBottomPoint.position, putAnimationTime)
                    .OnComplete(() =>
                    {
                        right.gameObject.SetActive(false);
                        left.SetInteractable(true);
                    });
            }

            return true;
        }

        return false;
    }

    #endregion
}