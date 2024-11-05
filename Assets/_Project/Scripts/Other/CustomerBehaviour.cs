using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Customer prefab
/// </summary>
public class CustomerBehaviour : MonoBehaviour
{
    [SerializeField] Image customerImage;
    [SerializeField] float changeSpritesSpeed = 0.1f;
    [Space]
    [SerializeField] Transform rotationTransform;
    [SerializeField] float rotationIntensity = 4;
    [SerializeField] float rotationSpeed = 0.5f;

    Sprite[] sprites;
    int spriteIndex;
    float timer;

    public void Init(params Sprite[] sprites)
    {
        //save sprites
        this.sprites = sprites;
        spriteIndex = 0;
        customerImage.sprite = sprites[spriteIndex];

        //start rotation animation
        StartRotationAnimation();
    }

    private void Update()
    {
        //animate sprites
        if (sprites != null)
        {
            if (Time.time > timer)
            {
                timer = Time.time + changeSpritesSpeed;
                spriteIndex = spriteIndex + 1 % sprites.Length;
                customerImage.sprite = sprites[spriteIndex];
            }
        }
    }

    private void StopRotationAnimation()
    {
        //reset rotation
        Tween.StopAll(rotationTransform);
        rotationTransform.localRotation = Quaternion.identity;
    }

    private void StartRotationAnimation()
    {
        StopRotationAnimation();

        //start rotation animation
        Tween.LocalRotation(rotationTransform, new Vector3(0, 0, rotationIntensity), rotationSpeed * 0.5f)
            .OnComplete(() => Tween.LocalRotation(rotationTransform, new Vector3(0, 0, rotationIntensity), new Vector3(0, 0, -rotationIntensity), rotationSpeed, Ease.Default, cycles: -1, CycleMode.Yoyo));
    }
}
