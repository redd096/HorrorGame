
/// <summary>
/// Simple struct to avoid use a string to declare the room number
/// </summary>
[System.Serializable]
public struct FRoom
{
    public int Floor;
    public int Room;

    public bool IsEqual(FRoom other)
    {
        return Floor == other.Floor && Room == other.Room;
    }

    public string ToRoomString()
    {
        return Floor.ToString() + Room.ToString();
    }
}
