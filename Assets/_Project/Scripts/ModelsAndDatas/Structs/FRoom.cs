
/// <summary>
/// Simple struct to avoid use a string to declare the room number
/// </summary>
[System.Serializable]
public struct FRoom
{
    public int Floor;
    public int Room;

    public override string ToString()
    {
        //return base.ToString();
        return Floor.ToString() + Room.ToString();
    }
}
