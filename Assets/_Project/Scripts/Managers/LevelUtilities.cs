using System.Collections;
using PrimeTween;
using redd096;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is used to do simple functions, like move a customer or play a dialogue and wait it to finish
/// </summary>
public class LevelUtilities : SimpleInstance<LevelUtilities>
{
    [Header("Customer")]
    [SerializeField] CustomerBehaviour customerPrefab;
    [SerializeField] Transform customerContainer;
    [SerializeField] Transform customerStartPoint;
    [SerializeField] Transform customerEndPoint;
    [SerializeField] float customerAnimation = 3;

    [Header("Fade in / Fade out")] 
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeTime = 2;

    #region generic

    /// <summary>
    /// Tween.Position but update if startPoint or endPoint change position (e.g. change resolution screen)
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static Tween TweenPositionDyanmic(Transform tr, Transform startPoint, Transform endPoint, TweenSettings settings)
    {
        return Tween.Custom(0f, 1f, settings, x =>
        {
            tr.position = Vector3.Lerp(startPoint.position, endPoint.position, x);
        });
    }

    /// <summary>
    /// Tween.Position but update if endPoint change position (e.g. change resolution screen)
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="endPoint"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static Tween TweenPositionDyanmic(Transform tr, Transform endPoint, TweenSettings settings)
    {
        Vector3 startPosition = tr.position;
        return Tween.Custom(0f, 1f, settings, x =>
        {
            tr.position = Vector3.Lerp(startPosition, endPoint.position, x);
        });
    }

    /// <summary>
    /// Tween.UIAnchoredPosition but update if endPoint change position (e.g. change resolution screen)
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="endPoint"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static Tween TweenUIAnchoredPositionDyanmic(RectTransform tr, RectTransform endPoint, TweenSettings settings)
    {
        Vector2 startPosition = tr.anchoredPosition;
        return Tween.Custom(0f, 1f, settings, x =>
        {
            tr.anchoredPosition = Vector2.Lerp(startPosition, endPoint.anchoredPosition, x);
        });
    }

    #endregion

    #region customer

    /// <summary>
    /// Instantiate, initialize and return the customer behaviour
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    public CustomerBehaviour InstantiateCustomer(Customer customer)
    {
        CustomerBehaviour customerInstance = Instantiate(customerPrefab, customerContainer);
        customerInstance.Init(customer.CustomerImage.ToArray());
        
        return customerInstance;
    }
    
    /// <summary>
    /// Move customer in front of desk or outside the screen
    /// </summary>
    /// <param name="customerInstance"></param>
    /// <param name="enterInScene">if true, move from outside to in front of the desk. If false, move outside the screen</param>
    /// <returns></returns>
    public Sequence MoveCustomer(CustomerBehaviour customerInstance, bool enterInScene)
    {
        //select start and end points (enter in scene, or is leaving the scene)
        Transform startPoint = enterInScene ? customerStartPoint : customerEndPoint;
        Transform endPoint = enterInScene ? customerEndPoint : customerStartPoint;
        
        //move customer
        Sequence sequence = MoveCustomer(customerInstance, startPoint, endPoint);

        //stop walk when enter in scene (not necessary when go outside the screen)
        if (enterInScene)
            sequence.ChainCallback(customerInstance.StopWalk);

        return sequence;
    }

    ///// <summary>
    ///// Move customer from current position to end position
    ///// </summary>
    ///// <param name="customerInstance"></param>
    ///// <param name="endPosition"></param>
    ///// <returns></returns>
    //public Sequence MoveCustomer(CustomerBehaviour customerInstance, Vector3 endPosition)
    //{
    //    return MoveCustomer(customerInstance, customerInstance.transform.position, endPosition);
    //}

    /// <summary>
    /// Move customer from start position to end position
    /// </summary>
    /// <param name="customerInstance"></param>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <returns></returns>
    public Sequence MoveCustomer(CustomerBehaviour customerInstance, Transform startPoint, Transform endPoint)
    {
        Sequence sequence = Sequence.Create();

        //start walk animation
        sequence.ChainCallback(customerInstance.StartWalk);
        
        //move customer
        sequence.Chain(TweenPositionDyanmic(customerInstance.transform, startPoint, endPoint, new TweenSettings(customerAnimation, Ease.InOutSine)));
        return sequence;
    }

    /// <summary>
    /// Move customer outside the screen (both normal or ghost) and destroy it
    /// </summary>
    /// <returns></returns>
    public Sequence MoveCustomerAwayFromScreen(CustomerBehaviour customerInstance, Customer customer)
    {
        Sequence sequence = Sequence.Create();

        //move customer away from screen normally
        if (customer.GoAwayLikeGhost == false)
        {
            sequence.Chain(MoveCustomer(customerInstance, enterInScene: false));
        }
        //TEMP - or fade away, because we have some customer that are ghosts
        //probably is better to have an "event node" just for ghosts
        else
        {
            sequence.Chain(customerInstance.FadeAlpha());
        }

        //and destroy it
        sequence.ChainCallback(() =>
        {
            Destroy(customerInstance.gameObject);
            customer = null;
        });

        return sequence;
    }
    
    #endregion
    
    #region dialogue

    /// <summary>
    /// Start a dialogue and wait it to finish
    /// </summary>
    /// <param name="dialogueName"></param>
    /// <returns></returns>
    public IEnumerator WaitDialogue(string dialogueName)
    {
        //be sure there is a dialogue
        if (string.IsNullOrEmpty(dialogueName))
            yield break;

        if (DialogueManagerUtilities.CheckConversationExists(dialogueName) == false)
        {
            Debug.LogError("Missing dialogue in database: " + dialogueName);
            yield break;
        }

        //register to end dialogue event
        bool isTalking = true;
        DialogueManagerEvents.instance.onConversationEnd += () => isTalking = false;

        //start dialogue
        DialogueManagerUtilities.StartConversation(dialogueName);

        //wait dialogue to finish
        yield return new WaitWhile(() => isTalking);
    }
    
    #endregion
    
    #region fade in / fade out

    /// <summary>
    /// Fade in to show scene
    /// </summary>
    /// <returns></returns>
    public Tween FadeIn()
    {
        //on complete, be sure image is deactivated to avoid block player clicks
        return FadeAlpha(alpha: 0, fadeTime)
            .OnComplete(() => fadeImage.gameObject.SetActive(false));
    }

    /// <summary>
    /// Fade out to hide scene
    /// </summary>
    /// <returns></returns>
    public Tween FadeOut()
    {
        return FadeAlpha(alpha: 1, fadeTime);
    }
    
    /// <summary>
    /// Fade image to alpha
    /// </summary>
    /// <param name="alpha"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public Tween FadeAlpha(float alpha, float duration)
    {
        //be sure image is active
        fadeImage.gameObject.SetActive(true);
        
        //do fade
        Tween.StopAll(fadeImage);
        return Tween.Alpha(fadeImage, alpha, duration, Ease.InOutSine);
    }
    
    #endregion
}