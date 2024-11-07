using UnityEngine;

/// <summary>
/// Inside AppointsmentsManager use this to check appointments for the day
/// </summary>
[CreateAssetMenu(menuName = "HORROR GAME/Appointment")]
public class AppointmentData : ScriptableObject
{
    public string Profession;
    public string AppointmentReason;
}
