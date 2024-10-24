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
    [SerializeField] InteractableDraggable prefabRight;

    /// <summary>
    /// Add documents already instantiated, both left and right
    /// </summary>
    public void AddDocument(InteractableOnTheLeft docInScene, InteractableOnTheRight docDraggable)
    {
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
    public bool CheckToRemoveDocument(InteractableOnTheLeft docInScene, InteractableOnTheRight docDraggable)
    {
        //check is inside area
        if (deskWindowsManager.CheckIsInGiveDocumentsArea(docDraggable.transform.position))
        {
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
    public void AddInteractable(InteractableOnTheLeft clickedInteractable, InteractableOnTheRight instantiatedInScene)
    {
        //tell windowsManager there is a new interactable
        deskWindowsManager.AddInteractable();
        
        instantiatedInScene.gameObject.SetActive(true);
        
        //save position and parent for put back
        clickedInteractable.SaveStartPositionAndParent();
        
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
    public bool CheckToRemoveInteractable(InteractableOnTheLeft clickedInteractable, InteractableOnTheRight instantiatedInScene)
    {
        //check is inside area
        if (deskWindowsManager.CheckIsInPutBackInteractablesArea(instantiatedInScene.transform.position))
        {
            //tell windowsManager to remove an interactable
            deskWindowsManager.RemoveInteractable();
            
            //set parent
            Vector2 posLeft = clickedInteractable.transform.position;
            clickedInteractable.transform.SetParent(clickedInteractable.StartParent, false);
            clickedInteractable.transform.position = posLeft;
            
            clickedInteractable.SetInteractable(false);
            instantiatedInScene.SetInteractable(false);
        
            //move out of the desk
            Tween.Position(clickedInteractable.transform, clickedInteractable.StartPosition, putAnimationTime);
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