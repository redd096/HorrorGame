using UnityEngine;

/// <summary>
/// Simple struct to avoid use a string to declare a date
/// </summary>
[System.Serializable]
public struct FDate
{
    [Min(1)] public int Day;
    [Min(1)] public int Month;
    [Min(1)] public int Year;

    public bool IsEqual(FDate other)
    {
        return Day == other.Day && Month == other.Month && Year == other.Year;
    }

    public System.DateTime GetDateTime()
    {
        //fix DateTime error when 0
        if (Day == 0) Day = 1;
        if (Month == 0) Month = 1;
        if (Year == 0) Year = 1;

        return new System.DateTime(Year, Month, Day);
    }

    public string ToAmericanString()
    {
        System.DateTime date = GetDateTime();
        return date.ToString("MMMM dd, yyyy");
    }

    public string ToEuropeString()
    {
        System.DateTime date = GetDateTime();
        return date.ToString("dd MMMM yyyy");
        //return date.ToShortDateString();
    }

    public void Min1()
    {
        Day = Mathf.Max(Day, 1);
        Month = Mathf.Max(Month, 1);
        Year = Mathf.Max(Year, 1);
    }
}
