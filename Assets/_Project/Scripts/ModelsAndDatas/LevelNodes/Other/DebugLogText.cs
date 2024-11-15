
/// <summary>
/// Stamp a text in console
/// </summary>
[System.Serializable]
public class DebugLogText
{
    public string Text;

    public DebugLogText Clone()
    {
        return new DebugLogText()
        {
            Text = Text,
        };
    }
}