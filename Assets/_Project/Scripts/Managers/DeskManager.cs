using redd096.Attributes;
using UnityEngine;
using PrimeTween;
using redd096;

/// <summary>
/// This manages UI
/// </summary>
public class DeskManager : SimpleInstance<DeskManager>
{
    [SerializeField] DeskWindowsManager deskWindowsManager;

    [Header("Put interactables animation")]
    [SerializeField] Transform leftContainer;
    [SerializeField] Transform leftStartPoint;
    [SerializeField] Transform leftEndPoint;
    [SerializeField] Transform rightContainer;
    [SerializeField] Transform rightStartTopPoint;
    [SerializeField] Transform rightStartBottomPoint;
    [SerializeField] Transform rightEndPoint;
    [SerializeField] float putAnimationTime = 1;
    
    [Header("TEMP Documents Prefabs")]
    [SerializeField] InteractableOnTheLeft prefabLeft;
    [SerializeField] DocumentDraggable prefabRight;

    //counter of documents to give back to client or interactables put back on desk
    private int documentsInScene;
    private int interactablesInScene;

    /// <summary>
    /// Add documents already instantiated, both left and right
    /// </summary>
    /// <param name="isDocumentToGiveBack">After stamp, show area to give back documents</param>
    public void AddDocument(InteractableOnTheLeft docInScene, InteractableOnTheRight docDraggable, bool isDocumentToGiveBack = true)
    {
        //update documents counter
        if (isDocumentToGiveBack)
            documentsInScene++;
        
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
    /// <param name="isDocumentToGiveBack">After stamp, show area to give back documents</param>
    public bool CheckToRemoveDocument(InteractableOnTheLeft docInScene, InteractableOnTheRight docDraggable, bool isDocumentToGiveBack = true)
    {
        //check is inside area
        if (deskWindowsManager.CheckIsInGiveDocumentsArea(docDraggable.transform.position))
        {
            //update documents counter and check to hide area
            if (isDocumentToGiveBack)
            {
                documentsInScene--;
                if (documentsInScene <= 0)
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
        //update interactables counter and show area
        if (isInteractableToPutBack)
        {
            interactablesInScene++;
            if (interactablesInScene > 0)
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
            //update interactables counter and check to hide area
            if (isInteractableToPutBack)
            {
                interactablesInScene--;
                if (interactablesInScene <= 0)
                    deskWindowsManager.ShowInteractablesArea(false);
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
    public void OnDocumentReceiveStamp()
    {
        deskWindowsManager.ShowDocumentsArea(true);
    }

    private System.Collections.IEnumerator Start()
    {
        Debug.Log("TODO - example first document");

        yield return new WaitForSeconds(0.1f);
        AddDocument();
    }

    [Button("Add Document (only in Play)", ButtonAttribute.EEnableType.PlayMode)]
    void AddDocument()
    {
        var left = Instantiate(prefabLeft);
        var right = Instantiate(prefabRight);
        IInteractablesEvents callbacks = FindObjectOfType<DeskStateMachine>();
        left.Init(callbacks);
        right.Init(callbacks, left);
        AddDocument(left, right);
    }
}