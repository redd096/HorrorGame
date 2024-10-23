using redd096.Attributes;
using UnityEngine;
using PrimeTween;
using redd096;

/// <summary>
/// This manages UI
/// </summary>
public class DeskManager : SimpleInstance<DeskManager>
{
    [Header("Documents Prefabs")]
    [SerializeField] InteractableOnTheLeft prefabLeft;
    [SerializeField] InteractableDraggable prefabRight;

    [Header("Put document animation")]
    [SerializeField] Transform leftContainer;
    [SerializeField] Transform leftStartPoint;
    [SerializeField] Transform leftEndPoint;
    [SerializeField] Transform rightContainer;
    [SerializeField] Transform rightStartTopPoint;
    [SerializeField] Transform rightStartBottomPoint;
    [SerializeField] Transform rightEndPoint;
    [SerializeField] float putAnimationTime = 1;

    /// <summary>
    /// Add documents already instantiated, both left and right
    /// </summary>
    public void AddDocument(InteractableOnTheLeft docInScene, InteractableOnTheRight docDraggable)
    {
        //set parent
        docInScene.transform.SetParent(leftContainer, false);
        docDraggable.transform.SetParent(rightContainer, false);

        docInScene.SetInteractable(false);
        docDraggable.SetInteractable(false);

        //and move on the desk
        Tween.Position(docInScene.transform, leftStartPoint.position, leftEndPoint.position, putAnimationTime);
        Tween.Position(docDraggable.transform, rightStartTopPoint.position, rightEndPoint.position, putAnimationTime)
            .OnComplete(() => docDraggable.SetInteractable(true));
    }

    /// <summary>
    /// Add an object already instantiated
    /// </summary>
    public void AddInteractable(InteractableOnTheLeft clickedInteractable, InteractableOnTheRight instantiatedInScene)
    {
        //set parent
        clickedInteractable.transform.SetParent(leftContainer, false);
        instantiatedInScene.transform.SetParent(rightContainer, false);

        clickedInteractable.SetInteractable(false);
        instantiatedInScene.SetInteractable(false);

        //and move
        Tween.Position(clickedInteractable.transform, leftEndPoint.position, putAnimationTime);
        Tween.Position(instantiatedInScene.transform, rightStartBottomPoint.position, rightEndPoint.position, putAnimationTime)
            .OnComplete(() => instantiatedInScene.SetInteractable(true));
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