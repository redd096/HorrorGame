using UnityEngine;

[CreateAssetMenu(menuName = "HorrorGame/Resident")]
public class ResidentData : ScriptableObject
{
    public Sprite Photo;
    public string Name;
    public string Surname;
    public FDate DateBirth;
    public FRoom RoomNumber;
}
