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
    [SerializeField] Transform leftStartPoint;
    [SerializeField] Transform leftEndPoint;
    [SerializeField] Transform rightContainer;
    [SerializeField] Transform rightStartTopPoint;
    [SerializeField] Transform rightStartBottomPoint;
    [SerializeField] Transform rightEndPoint;
    [SerializeField] float putAnimationTime = 1;
    
    [Header("Documents Prefabs")]
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

    //counter of documents to give back to client or interactables to put back on desk
    private int documentsToGiveBack;
    private int interactablesToPutBack;

    private List<InteractableOnTheRight> interactablesInScene = new List<InteractableOnTheRight>();

    public List<InteractableOnTheRight> InteractablesInScene => interactablesInScene;
    public RectTransform DraggedObjectsContainer => draggedObjectsContainer;

    #region instantiate document

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
        interactablesInScene.Add(docDraggable);

        //update documents counter
        if (isDocumentToGiveBack)
            documentsToGiveBack++;
        
        //set parent
        Vector2 posLeft = docInScene.transform.position;
        Vector2 posRight = docDraggable.transform.position;
        docInScene.transform.SetParent(leftContainer, false);
        docDraggable.transform.SetParent(rightContainer, false);
        docInScene.transform.position = posLeft;
        docDraggable.transform.position = posRight;

        docInScene.SetInteractable(false);
        docDraggable.SetInteractable(false);

        //and move on the desk
        Tween.Position(docInScene.transform, leftStartPoint.position, leftEndPoint.position, putAnimationTime);
        Tween.Position(docDraggable.transform, rightStartTopPoint.position, rightEndPoint.position, putAnimationTime)
            .OnComplete(() => docDraggable.SetInteractable(true));
    }

    /// <summary>
    /// Check if inside documents area. If inside, remove document from the scene
    /// </summary>
    /// <param name="isDocumentToGiveBack">After give back every document, hide area to give back documents</param>
    public bool CheckToRemoveDocument(InteractableOnTheLeft docInScene, InteractableOnTheRight docDraggable, bool isDocumentToGiveBack = true)
    {
        //check is inside area
        if (deskWindowsManager.CheckIsInGiveDocumentsArea(docDraggable.transform.position))
        {
            interactablesInScene.Remove(docDraggable);

            //update documents counter and check to hide area
            if (isDocumentToGiveBack)
            {
                documentsToGiveBack--;
                if (documentsToGiveBack <= 0)
                    deskWindowsManager.ShowDocumentsArea(false);
            }
            
            docInScene.SetInteractable(false);
            docDraggable.SetInteractable(false);
        
            //move out of the desk
            Tween.Position(docInScene.transform, leftStartPoint.position, putAnimationTime);
            Tween.Position(docDraggable.transform, rightStartTopPoint.position, putAnimationTime);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Add an object already instantiated
    /// </summary>
    public void AddInteractable(InteractableOnTheLeft clickedInteractable, InteractableOnTheRight instantiatedInScene, bool isInteractableToPutBack = true)
    {
        interactablesInScene.Add(instantiatedInScene);

        //update interactables counter and show area
        if (isInteractableToPutBack)
        {
            interactablesToPutBack++;
            if (interactablesToPutBack > 0)
                deskWindowsManager.ShowInteractablesArea(true);
        }
        
        instantiatedInScene.gameObject.SetActive(true);
        
        //set parent
        Vector2 posLeft = clickedInteractable.transform.position;
        Vector2 posRight = instantiatedInScene.transform.position;
        clickedInteractable.transform.SetParent(leftContainer, false);
        instantiatedInScene.transform.SetParent(rightContainer, false);
        clickedInteractable.transform.position = posLeft;
        instantiatedInScene.transform.position = posRight;

        clickedInteractable.SetInteractable(false);
        instantiatedInScene.SetInteractable(false);

        //and move
        Tween.Position(clickedInteractable.transform, leftEndPoint.position, putAnimationTime);
        Tween.Position(instantiatedInScene.transform, rightStartBottomPoint.position, rightEndPoint.position, putAnimationTime)
            .OnComplete(() => instantiatedInScene.SetInteractable(true));
    }

    /// <summary>
    /// Check if inside interactables area. If inside, put back interactable
    /// </summary>
    public bool CheckToRemoveInteractable(InteractableOnTheLeft clickedInteractable, InteractableOnTheRight instantiatedInScene, bool isInteractableToPutBack = true)
    {
        //check is inside area
        if (deskWindowsManager.CheckIsInPutBackInteractablesArea(instantiatedInScene.transform.position))
        {
            interactablesInScene.Remove(instantiatedInScene);

            //update interactables counter and check to hide area
            if (isInteractableToPutBack)
            {
                interactablesToPutBack--;
                if (interactablesToPutBack <= 0)
                {
                    deskWindowsManager.ShowInteractablesArea(false);
                    LevelManager.instance.OnGiveBackAllDocuments();
                }
            }
            
            //set parent
            Vector2 posLeft = clickedInteractable.transform.position;
            clickedInteractable.transform.SetParent(clickedInteractable.StartParent, false);
            clickedInteractable.transform.position = posLeft;
            
            clickedInteractable.SetInteractable(false);
            instantiatedInScene.SetInteractable(false);
        
            //move out of the desk
            Tween.UIAnchoredPosition(clickedInteractable.RectTr, clickedInteractable.StartAnchoredPosition, putAnimationTime);
            Tween.Position(instantiatedInScene.transform, rightStartBottomPoint.position, putAnimationTime)
                .OnComplete(() =>
                {
                    instantiatedInScene.gameObject.SetActive(false);
                    clickedInteractable.SetInteractable(true);
                });

            return true;
        }

        return false;
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
                documentsToGiveBack++;
                if (automaticallyShowOrHideDocumentsArea)
                    deskWindowsManager.ShowDocumentsArea(true);
            }
            //remove from documents to give back
            else
            {
                documentsToGiveBack--;
                if (automaticallyShowOrHideDocumentsArea && documentsToGiveBack <= 0)
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
}