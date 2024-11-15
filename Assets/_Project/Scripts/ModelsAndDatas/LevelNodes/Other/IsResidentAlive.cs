
/// <summary>
/// Return if resident is alive or not
/// </summary>
[System.Serializable]
public class IsResidentAlive
{
    public ResidentData Resident;

    public IsResidentAlive Clone()
    {
        return new IsResidentAlive()
        {
            Resident = Resident
        };
    }
}
