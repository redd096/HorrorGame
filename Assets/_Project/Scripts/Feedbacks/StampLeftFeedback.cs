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

    protected virtual void Start()
    {
        startLocalPosition = stampTransform.localPosition.y;
        SetSprite(false);
    }

    private void ResetAnimations()
    {
        //stop raise, laying down or rotation animation
        Tween.StopAll(stampTransform);
        Tween.StopAll(shadowTransform);

        //and reset rotation
        shadowTransform.localRotation = Quaternion.identity;
    }

    private void SetSprite(bool spriteWithShadow)
    {
        //set sprite normal or with shadow
        stampTransform.gameObject.SetActive(spriteWithShadow == false);
        shadowTransform.gameObject.SetActive(spriteWithShadow);
    }

    public void OnBeginDrag()
    {
        ResetAnimations();

        //raise stamp
        Tween.LocalPositionY(stampTransform, height, raiseAnimation)
            .OnComplete(() =>
            {
                //change sprite
                SetSprite(true);

                //start rotation animation
                Tween.LocalRotation(shadowTransform, new Vector3(0, 0, -rotationIntensity), new Vector3(0, 0, rotationIntensity), rotationSpeed, Ease.Default, cycles: -1, CycleMode.Yoyo);
            });
    }

    public void OnEndDrag()
    {
        ResetAnimations();

        //change sprite
        SetSprite(false);

        //lay down
        Tween.LocalPositionY(stampTransform, startLocalPosition, raiseAnimation);
    }
}
