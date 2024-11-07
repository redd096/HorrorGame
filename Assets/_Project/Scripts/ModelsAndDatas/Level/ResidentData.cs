using redd096.Attributes;
using UnityEngine;

/// <summary>
/// Inside ResidentsManager use this to check which residents are in hotel
/// </summary>
[CreateAssetMenu(menuName = "HORROR GAME/Resident")]
public class ResidentData : ScriptableObject
{
    [ShowAssetPreview] public Sprite Photo;
    public string Name;
    public string Surname;
    public string IDCardNumber;
    public FRoom RoomNumber;
}
