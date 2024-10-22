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
    [SerializeField] DocumentLeft prefabLeft;
    [SerializeField] DocumentRight prefabRight;

    [Header("Put document animation")]
    [SerializeField] Transform leftContainer;
    [SerializeField] Transform leftStartPosition;
    [SerializeField] Transform leftEndPosition;
    [SerializeField] Transform rightContainer;
    [SerializeField] Transform rightStartPosition;
    [SerializeField] Transform rightEndPosition;
    [SerializeField] float putDocumentAnimationTime = 1;

    [Button]
    void AddDocument()
    {
        //instantiate both left and right
        DocumentLeft docLeft = Instantiate(prefabLeft, leftContainer);
        docLeft.transform.position = leftStartPosition.position;
        DocumentRight docRight = Instantiate(prefabRight, rightContainer);
        docRight.transform.position = rightStartPosition.position;

        //init
        docLeft.Init(docRight);
        docRight.Init(docLeft);
        docRight.SetInteractable(false);

        //and move on the desk
        Tween.Position(docLeft.transform, leftEndPosition.position, putDocumentAnimationTime);
        Tween.Position(docRight.transform, rightEndPosition.position, putDocumentAnimationTime)
            .OnComplete(() => docRight.SetInteractable(true));
    }

    private System.Collections.IEnumerator Start()
    {
        Debug.Log("TODO - example first document");

        yield return new WaitForSeconds(0.1f);
        AddDocument();
    }

    #region documents events

    public void DocumentBeginDrag(DocumentRight doc)
    {

    }

    public void DocumentDrag(DocumentRight doc)
    {

    }

    public void DocumentEndDrag(DocumentRight doc)
    {

    }

    #endregion
}