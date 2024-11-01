using UnityEngine;

/// <summary>
/// Inside ResidentsManager use this to check which residents are in hotel
/// </summary>
[CreateAssetMenu(menuName = "HORROR GAME/Resident")]
public class ResidentData : ScriptableObject
{
    public Sprite Photo;
    public string Name;
    public string Surname;
    public FDate DateBirth;
    public FRoom RoomNumber;
}
