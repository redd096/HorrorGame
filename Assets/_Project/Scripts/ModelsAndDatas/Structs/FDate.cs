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
        Min1();

        return new System.DateTime(Year, Month, Day);
    }

    /// <summary>
    /// April 02, 1940
    /// </summary>
    /// <returns></returns>
    public string ToAmericanString()
    {
        System.DateTime date = GetDateTime();
        return date.ToString("MMMM dd, yyyy", new System.Globalization.CultureInfo("en-EN"));
    }

    /// <summary>
    /// 02 April 1940
    /// </summary>
    /// <returns></returns>
    public string ToEuropeString()
    {
        System.DateTime date = GetDateTime();
        return date.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("en-EN"));
        //return date.ToShortDateString();
    }

    /// <summary>
    /// Fix DateTime errors if values are 0
    /// </summary>
    public void Min1()
    {
        Day = Mathf.Max(Day, 1);
        Month = Mathf.Max(Month, 1);
        Year = Mathf.Max(Year, 1);
    }
}
