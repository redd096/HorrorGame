using redd096.Attributes;
using UnityEngine;
using PrimeTween;
using redd096.v2.ComponentsSystem;

/// <summary>
/// When someone gives player a document, put 3d object on the desk in scene and show 2d object to player desk camera. Player can interact with the 2d object
/// </summary>
public class DeskManager : BasicStateMachineComponent
{
    //blackboard
    public const string DRAGGED_OBJECT_BLACKBOARD = "DraggedObject";

    [Header("States")]
    public DeskNormalState NormalState;
    public DeskDraggingState DraggingState;

    [Header("Prefabs")]
    [SerializeField] Document3D prefab3D;
    [SerializeField] Document2D prefab2D;

    [Header("Put document animation")]
    [SerializeField] Transform document3DStartPosition;
    [SerializeField] Transform document3DEndPosition;
    [SerializeField] Transform document2DStartPosition;
    [SerializeField] Transform document2DEndPosition;
    [SerializeField] float putDocumentAnimationTime = 1;

    [Header("Drag")]
    [SerializeField] Camera deskCamera;

    public Camera DeskCamera => deskCamera;

    [Button]
    void AddDocument()
    {
        //instantiate both 3d and 2d
        Document3D doc3d = Instantiate(prefab3D, document3DStartPosition.position, Quaternion.identity);
        Document2D doc2d = Instantiate(prefab2D, document2DStartPosition.position, Quaternion.identity);
        doc2d.SetInteractable(false);

        //and move on the desk
        Tween.Position(doc3d.transform, document3DEndPosition.position, putDocumentAnimationTime);
        Tween.Position(doc2d.transform, document2DEndPosition.position, putDocumentAnimationTime)
            .OnComplete(() => doc2d.SetInteractable(true));
    }

    private void Awake()
    {
        SetState(NormalState);
        AddDocument();
    }
}