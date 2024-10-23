using PrimeTween;
using UnityEngine;

/// <summary>
/// Elements to animate stamp on the left screen, but it's moved by stamp on the right
/// </summary>
public class StampLeftFeedback : MonoBehaviour
{
    [SerializeField] Transform stampTransform;
    [SerializeField] Transform shadowTransform;
    [SerializeField] float height = 200;
    [SerializeField] float raiseAnimation = 0.2f;
    [SerializeField] float rotationIntensity = 4;
    [SerializeField] float rotationSpeed = 0.5f;

    private float startLocalPosition;
    private Sequence sequence;

    protected virtual void Start()
    {
        startLocalPosition = stampTransform.localPosition.y;
        shadowTransform.localScale = Vector3.zero;
    }

    private void ResetAnimations()
    {
        //if still raising or laying down, stop it
        if (sequence.isAlive)
            sequence.Stop();

        //if dragging, stop rotation
        Tween.StopAll(stampTransform);
        stampTransform.localRotation = Quaternion.identity;
    }

    public void OnBeginDrag()
    {
        ResetAnimations();

        //raise stamp
        sequence = Sequence.Create();
        sequence.Chain(Tween.LocalPositionY(stampTransform, height, raiseAnimation));
        sequence.Group(Tween.Scale(shadowTransform, 1f, raiseAnimation));

        sequence.OnComplete(() =>
        {
            //start rotation animation
            Tween.LocalRotation(stampTransform, new Vector3(0, 0, -rotationIntensity), new Vector3(0, 0, rotationIntensity), rotationSpeed, Ease.Default, cycles: -1, CycleMode.Yoyo);
        });
    }

    public void OnEndDrag()
    {
        ResetAnimations();

        //lay down
        sequence = Sequence.Create();
        sequence.Chain(Tween.LocalPositionY(stampTransform, startLocalPosition, raiseAnimation));
        sequence.Group(Tween.Scale(shadowTransform, 0f, raiseAnimation));
    }
}
