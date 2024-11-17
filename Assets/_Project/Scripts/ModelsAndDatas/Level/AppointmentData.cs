using UnityEngine;

/// <summary>
/// Inside AppointmentsManager use this to check appointments for the day
/// </summary>
[CreateAssetMenu(menuName = "HORROR GAME/Appointment")]
public class AppointmentData : ScriptableObject
{
    public string Name;
    public string Surname;
    public string Profession;
    public string AppointmentReason;
}
