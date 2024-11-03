
/// <summary>
/// Save choice values
/// </summary>
[System.Serializable]
public class SaveChoice
{
    public string VariableName;

    public SaveChoice Clone()
    {
        return new SaveChoice()
        {
            VariableName = VariableName
        };
    }
}
