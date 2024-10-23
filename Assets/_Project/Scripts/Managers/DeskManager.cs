using redd096.Attributes;
using UnityEngine;
using PrimeTween;
using redd096;

/// <summary>
/// This manages UI and has a reference to the statemachine of the desk
/// </summary>
public class DeskManager : SimpleInstance<DeskManager>
{
    [Header("Documents Prefabs")]
    [SerializeField] DocumentScene prefabLeft;
    [SerializeField] DocumentDraggable prefabRight;

    [Header("Put document animation")]
    [SerializeField] Transform leftContainer;
    [SerializeField] Transform leftStartPosition;
    [SerializeField] Transform leftEndPosition;
    [SerializeField] Transform rightContainer;
    [SerializeField] Transform rightStartPosition;
    [SerializeField] Transform rightEndPosition;
    [SerializeField] float putDocumentAnimationTime = 1;

    [Header("Instantiated interactables")]
    [SerializeField] Transform interactablesContainer;
    [SerializeField] Transform interactablesStartPosition;
    [SerializeField] Transform interactablesEndPosition;
    [SerializeField] float putInteractableAnimationTime = 1;

    /// <summary>
    /// Instantiate document both left and right
    /// </summary>
    [Button("Add Document (only in Play)", ButtonAttribute.EEnableType.PlayMode)]
    public void AddDocument()
    {
        //instantiate both left and right
        DocumentScene docScene = Instantiate(prefabLeft, leftContainer);
        DocumentDraggable docInteract = Instantiate(prefabRight, rightContainer);

        //init
        docInteract.Init(docScene);
        docInteract.SetInteractable(false);

        //and move on the desk
        Tween.Position(docScene.transform, leftStartPosition.position, leftEndPosition.position, putDocumentAnimationTime);
        Tween.Position(docInteract.transform, rightStartPosition.position, rightEndPosition.position, putDocumentAnimationTime)
            .OnComplete(() => docInteract.SetInteractable(true));
    }

    /// <summary>
    /// Add an object already instantiated
    /// </summary>
    /// <param name="interactableInScene"></param>
    public void AddInteractable(InteractableBase interactableInScene)
    {
        //set parent
        interactableInScene.transform.SetParent(interactablesContainer);

        interactableInScene.SetInteractable(false);

        //and move
        Tween.Position(interactableInScene.transform, interactablesStartPosition.position, interactablesEndPosition.position, putInteractableAnimationTime)
            .OnComplete(() => interactableInScene.SetInteractable(true));
    }

    private System.Collections.IEnumerator Start()
    {
        Debug.Log("TODO - example first document");

        yield return new WaitForSeconds(0.1f);
        AddDocument();
    }
}