using UnityEngine;

/// <summary>
/// SKill a random resident, or kill a specific resident
/// </summary>
[System.Serializable]
public class EventKillResident
{
    public bool KillRandom;
    [Tooltip("If random is false, set a resident")] public ResidentData SpecificResident;

    public EventKillResident Clone()
    {
        return new EventKillResident()
        {
            KillRandom = KillRandom,
            SpecificResident = SpecificResident
        };
    }
}
