using redd096.Attributes;
using UnityEngine;
using PrimeTween;
using redd096;

/// <summary>
/// This manages UI and has a reference to the statemachine of the desk
/// </summary>
public class DeskManager : SimpleInstance<DeskManager>
{
    [SerializeField] DeskStateMachine stateMachine;

    [Header("Prefabs")]
    [SerializeField] DocumentScene prefabLeft;
    [SerializeField] DocumentInteract prefabRight;

    [Header("Put document animation")]
    [SerializeField] Transform leftContainer;
    [SerializeField] Transform leftStartPosition;
    [SerializeField] Transform leftEndPosition;
    [SerializeField] Transform rightContainer;
    [SerializeField] Transform rightStartPosition;
    [SerializeField] Transform rightEndPosition;
    [SerializeField] float putDocumentAnimationTime = 1;

    [Button("Add Document (only in Play)", ButtonAttribute.EEnableType.PlayMode)]
    public void AddDocument()
    {
        //instantiate both left and right
        DocumentScene docScene = Instantiate(prefabLeft, leftContainer);
        docScene.transform.position = leftStartPosition.position;
        DocumentInteract docInteract = Instantiate(prefabRight, rightContainer);
        docInteract.transform.position = rightStartPosition.position;

        //init
        docInteract.Init(docScene, stateMachine);
        docInteract.SetInteractable(false);

        //and move on the desk
        Tween.Position(docScene.transform, leftEndPosition.position, putDocumentAnimationTime);
        Tween.Position(docInteract.transform, rightEndPosition.position, putDocumentAnimationTime)
            .OnComplete(() => docInteract.SetInteractable(true));
    }

    private System.Collections.IEnumerator Start()
    {
        Debug.Log("TODO - example first document");

        yield return new WaitForSeconds(0.1f);
        AddDocument();
    }
}