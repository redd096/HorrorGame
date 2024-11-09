using PixelCrushers.DialogueSystem;
using UnityEngine;

/// <summary>
/// Attached to DialogueManager, to help with some functions
/// </summary>
public static class DialogueManagerUtilities
{
    private static bool showDebugs = false;

    //PPD dialogue variables
    const string preludeComeBackLater = "DialoguePrelude1.comeBackLater";
    const string cinematicVariable = "BooleanValue";

    public static bool isUIHidden;

    public static System.Action onShowUI;
    public static System.Action onHideUI;

    #region test functions

    //private void Awake()
    //{
    //    //register to events
    //    Lua.RegisterFunction("ProvaFunction", this, SymbolExtensions.GetMethodInfo(() => ProvaFunction()));
    //    Lua.RegisterFunction("ProvaFunctionString", this, SymbolExtensions.GetMethodInfo(() => ProvaFunctionString(string.Empty)));
    //    Lua.RegisterFunction("ProvaFunctionBool", this, SymbolExtensions.GetMethodInfo(() => ProvaFunctionBool(false)));
    //    Lua.RegisterFunction("ProvaFunctionDouble", this, SymbolExtensions.GetMethodInfo(() => ProvaFunctionDouble(0)));
    //    Lua.RegisterFunction("ChangeAnimation", this, SymbolExtensions.GetMethodInfo(() => ChangeAnimation(string.Empty)));
    //}

    //private void OnDestroy()
    //{
    //    //unregister from events
    //    Lua.UnregisterFunction("ProvaFunction");
    //    Lua.UnregisterFunction("ProvaFunctionString");
    //    Lua.UnregisterFunction("ProvaFunctionBool");
    //    Lua.UnregisterFunction("ProvaFunctionDouble");
    //    Lua.UnregisterFunction("ChangeAnimation");
    //}

    //void ProvaFunction()
    //{
    //    Debug.Log("Prova Function");
    //}

    //void ProvaFunctionString(string value)
    //{
    //    Debug.Log("Prova Function String: " + value);
    //}

    //void ProvaFunctionBool(bool value)
    //{
    //    Debug.Log("Prova Function bool: " + value);
    //}

    //void ProvaFunctionDouble(double value)
    //{
    //    Debug.Log("Prova Function double: " + value);
    //}

    //void ChangeAnimation(string value)
    //{
    //    Debug.Log("Change Animation: " + value);
    //    string[] s = value.Split(';');

    //    //first value is name of the object
    //    GameObject go = GameObject.Find(s[0]);
    //    if (go)
    //    {
    //        //second value is animation to play
    //        Animator anim = go.GetComponentInChildren<Animator>();
    //        if (anim)
    //        {
    //            anim.Play(s[1]);
    //        }
    //    }
    //}

    #endregion

    #region conversation functions

    /// <summary>
    /// Check if this conversation is in database
    /// </summary>
    /// <returns></returns>
    public static bool CheckConversationExists(string conversationName)
    {
        return DialogueManager.masterDatabase.GetConversation(conversationName) != null;
    }

    /// <summary>
    /// Start conversation
    /// </summary>
    public static void StartConversation(string conversationName, Transform actor, Transform conversant)
    {
        DialogueManager.StartConversation(conversationName, actor, conversant);
    }

    /// <summary>
    /// Stop conversation
    /// </summary>
    public static void StopConversation()
    {
        DialogueManager.StopConversation();
    }

    /// <summary>
    /// Check if there is conversation playing at the moment (both if ui is showed or not)
    /// </summary>
    /// <returns></returns>
    public static bool IsConversationPlaying()
    {
        return DialogueManager.conversationView != null;
    }

    /// <summary>
    /// Return conversation ui if playing (both if ui is showed or not)
    /// </summary>
    /// <returns></returns>
    public static StandardDialogueUI GetConversationUI()
    {
        var standardDialogueUI = DialogueManager.dialogueUI as StandardDialogueUI;
        if (standardDialogueUI)
            return standardDialogueUI;
        return null;
    }

    /// <summary>
    /// Return subtitle panel with activeSelf true (both if ui is showed or not)
    /// </summary>
    /// <returns></returns>
    public static StandardUISubtitlePanel GetActiveSubtitlePanel()
    {
        var standardDialogueUI = GetConversationUI();
        if (standardDialogueUI)
        {
            foreach (var subtitlePanel in standardDialogueUI.conversationUIElements.subtitlePanels)
            {
                if (subtitlePanel.gameObject.activeSelf)
                    return subtitlePanel;
            }
        }

        return null;
    }

    #endregion

    #region PPD dialogue variables

    /// <summary>
    /// Check preludeComeBackLater variable. Return its value and reset it to false
    /// </summary>
    /// <returns></returns>
    public static bool CheckPreludeComeBackLater()
    {
        if (TryGetVariable(preludeComeBackLater, out Lua.Result result) && result.isBool && result.asBool == true)
        {
            SetVariable(preludeComeBackLater, false);   //reset to false
            return true;
        }
        return false;
    }

    /// <summary>
    /// Check if there is the field to stop dialogue and start cinematic
    /// </summary>
    /// <returns></returns>
    public static bool CheckCinematic(Subtitle subtitle)
    {
        return GetBool(cinematicVariable, subtitle);
    }

    #endregion

    #region variables and fields by value

    /// <summary>
    /// If this dialogue has a field with this name, return its value. Else check for a variable with this name and return it
    /// </summary>
    /// <param name="variableName">field or variable name</param>
    /// <param name="subtitle">this is used for fields, to get list of field for this subtitle</param>
    /// <returns></returns>
    public static string GetString(string variableName, Subtitle subtitle = null)
    {
        if (TryGetField(variableName, subtitle, out Field field))
        {
            //Field.LookupValue()
            return field.value;
        }
        else if (TryGetVariable(variableName, out Lua.Result variable))
        {
            return variable.asString;
        }

        return string.Empty;
    }

    /// <summary>
    /// If this dialogue has a field with this name, return its value. Else check for a variable with this name and return it
    /// </summary>
    /// <param name="variableName">field or variable name</param>
    /// <param name="subtitle">this is used for fields, to get list of field for this subtitle</param>
    /// <returns></returns>
    public static bool GetBool(string variableName, Subtitle subtitle = null)
    {
        if (TryGetField(variableName, subtitle, out Field field))
        {
            //Field.LookupBool()
            return Tools.StringToBool(field.value);
        }
        else if (TryGetVariable(variableName, out Lua.Result variable))
        {
            return variable.asBool;
        }

        return false;
    }

    /// <summary>
    /// If this dialogue has a field with this name, return its value. Else check for a variable with this name and return it
    /// </summary>
    /// <param name="variableName">field or variable name</param>
    /// <param name="subtitle">this is used for fields, to get list of field for this subtitle</param>
    /// <returns></returns>
    public static float GetFloat(string variableName, Subtitle subtitle = null)
    {
        if (TryGetField(variableName, subtitle, out Field field))
        {
            //Field.LookupFloat
            return Tools.StringToFloat(field.value);
        }
        else if (TryGetVariable(variableName, out Lua.Result variable))
        {
            return variable.asFloat;
        }

        return 0f;
    }

    /// <summary>
    /// If this dialogue has a field with this name, return its value. Else check for a variable with this name and return it
    /// </summary>
    /// <param name="variableName">field or variable name</param>
    /// <param name="subtitle">this is used for fields, to get list of field for this subtitle</param>
    /// <returns></returns>
    public static int GetInt(string variableName, Subtitle subtitle = null)
    {
        if (TryGetField(variableName, subtitle, out Field field))
        {
            //Field.LookupInt
            return Tools.StringToInt(field.value);
        }
        else if (TryGetVariable(variableName, out Lua.Result variable))
        {
            return variable.asInt;
        }

        return 0;
    }

    #endregion

    #region generic variables and fields

    /// <summary>
    /// Set value for a variable. This can't be used for fields
    /// </summary>
    public static void SetVariable(string variableName, object value)
    {
        DialogueLua.SetVariable(variableName, value);
    }

    /// <summary>
    /// Return variable with this name
    /// </summary>
    /// <returns></returns>
    public static Lua.Result GetVariable(string variableName)
    {
        return DialogueLua.GetVariable(variableName);
    }

    /// <summary>
    /// Check if there is a variable with this name and return it
    /// </summary>
    /// <returns></returns>
    public static bool TryGetVariable(string variableName, out Lua.Result result)
    {
        result = DialogueLua.GetVariable(variableName);
        return result.hasReturnValue;
    }

    /// <summary>
    /// Return field with this name
    /// </summary>
    /// <param name="subtitle">look every field in this subtitle</param>
    /// <returns></returns>
    public static Field GetField(string fieldName, Subtitle subtitle)
    {
        return Field.Lookup(subtitle.dialogueEntry.fields, fieldName);
    }

    /// <summary>
    /// Check if there is a field with this name and return it
    /// </summary>
    /// <param name="subtitle">look every field in this subtitle</param>
    /// <returns></returns>
    public static bool TryGetField(string fieldName, Subtitle subtitle, out Field result)
    {
        if (subtitle == null)
        {
            result = null;
            return false;
        }

        result = Field.Lookup(subtitle.dialogueEntry.fields, fieldName);
        return result != null;
    }

    #endregion

    #region Show and Hide UI

    public static void HideUI()
    {
        isUIHidden = true;

        //do nothing if there isn't a dialogue running
        if (IsConversationPlaying() == false)
        {
            if (showDebugs) Debug.Log("<color=red>Hide UI -> Dialogue is null </color>");
            return;
        }

        if (showDebugs) Debug.Log("Hide UI");

        //replaced SetDialoguePanel with DialogueUIController
        //DialogueManager.SetDialoguePanel(false, true);

        //hide dialogue ui
        var standardDialogueUI = DialogueManager.dialogueUI as StandardDialogueUI;
        if (standardDialogueUI)
        {
            //DialogueUIController dialogueUIController = standardDialogueUI.GetComponent<DialogueUIController>();
            //if (dialogueUIController)
            //{
            //    dialogueUIController.HideUI();
            //}
        }

        onHideUI?.Invoke();
    }

    public static void ShowUI()
    {
        isUIHidden = false;

        //do nothing if there isn't a dialogue running
        if (IsConversationPlaying() == false)
        {
            if (showDebugs) Debug.Log("<color=red>Show UI -> Dialogue is null </color>");
            return;
        }

        if (showDebugs) Debug.Log("Show UI");

        //replaced SetDialoguePanel with DialogueUIController + call subtitlePanel.Select() instead of ShowContinueButton
        //DialogueManager.SetDialoguePanel(true);

        var standardDialogueUI = DialogueManager.dialogueUI as StandardDialogueUI;
        if (standardDialogueUI)
        {
            //show dialogue ui
            //DialogueUIController dialogueUIController = standardDialogueUI.GetComponent<DialogueUIController>();
            //if (dialogueUIController)
            //{
            //    dialogueUIController.ShowUI();
            //}

            //find subtitle panel to show
            foreach (var subtitlePanel in standardDialogueUI.conversationUIElements.subtitlePanels)
            {
                if (subtitlePanel.gameObject.activeSelf)
                {
                    //replaced ShowContinueButton with subtitlePanel.Select()
                    ////show continue button
                    //if (subtitlePanel.continueButton.gameObject.activeSelf == false)
                    //{
                    //    subtitlePanel.ShowContinueButton();
                    //
                    //    //update layout
                    //    instance.StartCoroutine(UpdateLayoutCoroutine(subtitlePanel.continueButton.transform.parent.GetComponent<RectTransform>()));
                    //}

                    //select continue button
                    subtitlePanel.Select();

                    //restart typewriter effect
                    subtitlePanel.subtitleText.SetActive(false);
                    subtitlePanel.subtitleText.SetActive(true);

                    break;
                }
            }
        }

        onShowUI?.Invoke();
    }

    //private static IEnumerator UpdateLayoutCoroutine(RectTransform rect)
    //{
    //    yield return null;
    //    if (rect)
    //        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    //}

    #endregion
}