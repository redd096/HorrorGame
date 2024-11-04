
/// <summary>
/// Simple struct to avoid use a string to declare a date
/// </summary>
[System.Serializable]
public struct FDate
{
    public int Day;
    public int Month;
    public int Year;

    public override string ToString()
    {
        System.DateTime date = new System.DateTime(Year, Month, Day);

        return date.ToLongDateString();
    }
}
