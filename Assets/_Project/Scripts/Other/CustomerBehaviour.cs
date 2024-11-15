using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Customer prefab
/// </summary>
public class CustomerBehaviour : MonoBehaviour
{
    [SerializeField] Image customerImage;
    [SerializeField] float changeSpritesSpeed = 0.3f;
    [Space]
    [SerializeField] Transform rotationTransform;
    [SerializeField] float rotationIntensity = 2f;
    [SerializeField] float rotationSpeed = 0.5f;
    [Space]
    [SerializeField] float fadeTime = 3f;

    Sprite[] sprites;
    int spriteIndex;
    float timer;

    /// <summary>
    /// Set customer sprites
    /// </summary>
    /// <param name="sprites"></param>
    public void Init(params Sprite[] sprites)
    {
        //save sprites
        this.sprites = sprites;
        spriteIndex = 0;
        customerImage.sprite = sprites[spriteIndex];
    }

    private void Update()
    {
        //animate sprites
        if (sprites != null)
        {
            if (Time.time > timer)
            {
                timer = Time.time + changeSpritesSpeed;
                spriteIndex = (spriteIndex + 1) % sprites.Length;
                customerImage.sprite = sprites[spriteIndex];
            }
        }
    }

    /// <summary>
    /// Start walk animation
    /// </summary>
    public void StartWalk()
    {
        Tween.StopAll(rotationTransform);

        //start rotation animation
        Tween.LocalRotation(rotationTransform, new Vector3(0, 0, rotationIntensity), rotationSpeed * 0.5f)
            .OnComplete(() => Tween.LocalRotation(rotationTransform, new Vector3(0, 0, rotationIntensity), new Vector3(0, 0, -rotationIntensity), rotationSpeed, Ease.Default, cycles: -1, CycleMode.Yoyo));
    }

    /// <summary>
    /// Stop walk animation
    /// </summary>
    public void StopWalk()
    {
        Tween.StopAll(rotationTransform);

        //reset rotation
        Tween.LocalRotation(rotationTransform, Quaternion.identity, rotationSpeed * 0.5f);
    }

    /// <summary>
    /// Fade image from 1 to 0 alpha. It's used for ghosts
    /// </summary>
    /// <returns></returns>
    public Tween FadeAlpha()
    {
        return Tween.Alpha(customerImage, 0f, fadeTime, Ease.InOutSine);
    }
}
