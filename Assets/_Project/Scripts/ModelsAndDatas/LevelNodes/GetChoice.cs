
/// <summary>
/// Get choice values
/// </summary>
[System.Serializable]
public class GetChoice
{
    public string VariableName;

    public GetChoice Clone()
    {
        return new GetChoice()
        {
            VariableName = VariableName
        };
    }
}
