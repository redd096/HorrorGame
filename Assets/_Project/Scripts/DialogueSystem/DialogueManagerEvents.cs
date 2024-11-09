using PixelCrushers.DialogueSystem;
using redd096;
using UnityEngine;

/// <summary>
/// Attached to DialogueManager to receive events - It's possible to see every event in PixelCrushers.DialogueSystem.DialogueSystemEvents
/// </summary>
public class DialogueManagerEvents : SimpleInstance<DialogueManagerEvents>
{
    [SerializeField] bool showDebugs = false;

    //dialogue system events - persistent (register and unregister manually)
    public System.Action onConversationStartPersistent;
    public System.Action onConversationEndPersistent;

    //dialogue system events - they are reset at the end of the conversation
    public System.Action onConversationStart;
    public System.Action onConversationEnd;
    public System.Action onConversationLine;
    public System.Action<Subtitle> onConversationLineSubtitle;
    public System.Action onConversationLineEnd;
    public System.Action<Subtitle> onConversationLineEndSubtitle;
    public System.Action onConversationResponseMenu;
    public System.Action onConversationResponseLine;
    public System.Action<Subtitle> onConversationResponseLineSubtitle;
    public System.Action onConversationResponseLineEnd;
    public System.Action<Subtitle> onConversationResponseLineEndSubtitle;
    public System.Action onShowUI;
    public System.Action onHideUI;

    //fix - onConversationLine is called 2 times when start conversation, but the first doesn't have speaker and settings updated
    private bool firstLineIsCompleted;
    //fix - onConversationLine is called more times instead of one, so wait onConversationLineEnd before call again event
    private bool waitingLineEnd;
    //there isn't an event for response selected, so we check when start new line after response menu is showed
    //and fix, because it calls OnConversationLine and OnConversationLineEnd for the response too, before update speaker and settings and call again OnConversationLine for the new line 
    //NB this isn't a real fix, it calls OnConversationLine and OnConversationLineEnd because it can show player subtitle with selected response as text (if enabled in DialogueManager)
    private bool wasResponseMenuShowed;

    protected override void InitializeInstance()
    {
        base.InitializeInstance();

        //register to events
        DialogueManagerUtilities.onShowUI += OnShowUI;
        DialogueManagerUtilities.onHideUI += OnHideUI;
    }

    private void OnDestroy()
    {
        //unregister from events
        DialogueManagerUtilities.onShowUI -= OnShowUI;
        DialogueManagerUtilities.onHideUI -= OnHideUI;
    }

    #region public API

    public void ResetEvents()
    {
        onConversationStart = null;
        onConversationEnd = null;
        onConversationLine = null;
        onConversationLineSubtitle = null;
        onConversationLineEnd = null;
        onConversationLineEndSubtitle = null;
        onConversationResponseMenu = null;
        onConversationResponseLine = null;
        onConversationResponseLineSubtitle = null;
        onConversationResponseLineEnd = null;
        onConversationResponseLineEndSubtitle = null;
        onShowUI = null;
        onHideUI = null;

        firstLineIsCompleted = false;
        waitingLineEnd = false;
        wasResponseMenuShowed = false;
    }

    #endregion

    #region dialogue system events

    /// <summary>
    /// Called by DialogueManager
    /// </summary>
    /// <param name="actor"></param>
    void OnConversationStart(Transform actor)
    {
        if (showDebugs) Debug.Log("On Conversation Start");

        onConversationStart?.Invoke();
        onConversationStartPersistent?.Invoke();
    }

    /// <summary>
    /// Called by DialogueManager
    /// </summary>
    /// <param name="actor"></param>
    public void OnConversationEnd(Transform actor)
    {
        if (showDebugs) Debug.Log("On Conversation End");

        System.Action cloneOnConversationEnd = onConversationEnd != null ? onConversationEnd.Clone() as System.Action : null;

        //on end conversation, reset events
        ResetEvents();

        cloneOnConversationEnd?.Invoke();
        onConversationEndPersistent?.Invoke();
    }

    /// <summary>
    /// Called on show subtitle (when someone start to talk)
    /// </summary>
    /// <param name="subtitle"></param>
    void OnConversationLine(Subtitle subtitle)
    {
        //fix event called two times when conversation start
        if (firstLineIsCompleted == false)
        {
            firstLineIsCompleted = true;
            if (showDebugs) Debug.Log("<color=red>On Conversation Line -> First Line </color>");
            return;
        }

        //fix event called more times
        if (waitingLineEnd)
        {
            if (showDebugs) Debug.Log("<color=red>On Conversation Line -> Waiting Line End </color>");
            return;
        }
        waitingLineEnd = true;

        //fix missing event, response selected - one LineStart is called only for the response, before call again with the new line updated
        if (wasResponseMenuShowed)
        {
            onConversationResponseLine?.Invoke();
            onConversationResponseLineSubtitle?.Invoke(subtitle);
            if (showDebugs) Debug.Log("<color=yellow>On Conversation Line -> Conversation Response Selected</color>");
            return;
        }

        if (showDebugs) Debug.Log("On Conversation Line");
        onConversationLine?.Invoke();
        onConversationLineSubtitle?.Invoke(subtitle);
    }

    /// <summary>
    /// Called when close subtitle
    /// </summary>
    /// <param name="subtitle"></param>
    void OnConversationLineEnd(Subtitle subtitle)
    {
        waitingLineEnd = false;

        //one LineEnd is called only for the response, before call new line updated
        if (wasResponseMenuShowed)
        {
            wasResponseMenuShowed = false;
            if (showDebugs) Debug.Log("<color=red>On Conversation Line End -> Response Menu Showed </color>");
            onConversationResponseLineEnd?.Invoke();
            onConversationResponseLineEndSubtitle?.Invoke(subtitle);
            return;
        }

        if (showDebugs) Debug.Log("On Conversation Line End");
        onConversationLineEnd?.Invoke();
        onConversationLineEndSubtitle?.Invoke(subtitle);
    }

    /// <summary>
    /// Called on show response menu (when player have to select a response)
    /// </summary>
    /// <param name="responses"></param>
    void OnConversationResponseMenu(Response[] responses)
    {
        if (showDebugs) Debug.Log("On Conversation Response Menu");
        wasResponseMenuShowed = true;
        onConversationResponseMenu?.Invoke();
    }

    /// <summary>
    /// Called by DialogueManagerUtilities when someone call ShowUI
    /// </summary>
    void OnShowUI()
    {
        if (showDebugs) Debug.Log("On Show UI");
        onShowUI?.Invoke();
    }

    /// <summary>
    /// Called by DialogueManagerUtilities when someone call HideUI
    /// </summary>
    void OnHideUI()
    {
        if (showDebugs) Debug.Log("On Hide UI");
        onHideUI?.Invoke();
    }

    #endregion
}