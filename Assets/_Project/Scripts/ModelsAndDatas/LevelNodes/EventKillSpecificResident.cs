
/// <summary>
/// Set a resident to kill
/// </summary>
[System.Serializable]
public class EventKillSpecificResident
{
    public ResidentData Resident;

    public EventKillSpecificResident Clone()
    {
        return new EventKillSpecificResident()
        {
            Resident = Resident
        };
    }
}
