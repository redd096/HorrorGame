using System.Collections;
using PrimeTween;
using redd096;
using UnityEngine;

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
        
        return MoveCustomer(customerInstance, startPoint.position, endPoint.position);
    }

    /// <summary>
    /// Move customer from current position to end position
    /// </summary>
    /// <param name="customerInstance"></param>
    /// <param name="endPosition"></param>
    /// <returns></returns>
    public Sequence MoveCustomer(CustomerBehaviour customerInstance, Vector3 endPosition)
    {
        return MoveCustomer(customerInstance, customerInstance.transform.position, endPosition);
    }

    /// <summary>
    /// Move customer from start position to end position
    /// </summary>
    /// <param name="customerInstance"></param>
    /// <param name="startPosition"></param>
    /// <param name="endPosition"></param>
    /// <returns></returns>
    public Sequence MoveCustomer(CustomerBehaviour customerInstance, Vector3 startPosition, Vector3 endPosition)
    {
        Sequence sequence = Sequence.Create();

        //start walk animation
        sequence.ChainCallback(customerInstance.StartWalk);
        
        //move customer
        sequence.Chain(Tween.Position(customerInstance.transform, startPosition, endPosition, customerAnimation, Ease.InOutSine));
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
            Debug.LogError("Missing dialogue: " + dialogueName);
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
}