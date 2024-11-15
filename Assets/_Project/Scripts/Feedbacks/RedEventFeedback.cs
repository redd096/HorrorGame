using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is used by some events, to change sprite to Red 
/// </summary>
public class RedEventFeedback : MonoBehaviour
{
    [SerializeField] Image interactableImage;
    [SerializeField] Sprite redSprite;

    Sprite defaultSprite;

    /// <summary>
    /// Set Red sprite
    /// </summary>
    public void StartRedEvent()
    {
        //save default sprite
        if (defaultSprite == null)
            defaultSprite = interactableImage.sprite;

        interactableImage.sprite = redSprite;
    }

    /// <summary>
    /// Back to normal sprite
    /// </summary>
    public void StopRedEvent()
    {
        if (defaultSprite != null)
            interactableImage.sprite = defaultSprite;
    }
}
