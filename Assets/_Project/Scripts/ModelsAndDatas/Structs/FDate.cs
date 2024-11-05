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

    public void Min1()
    {
        Day = Mathf.Max(Day, 1);
        Month = Mathf.Max(Month, 1);
        Year = Mathf.Max(Year, 1);
    }

    public override string ToString()
    {
        //return base.ToString();

        //fix DateTime error when 0
        if (Day == 0) Day = 1;
        if (Month == 0) Month = 1;
        if (Year == 0) Year = 1;

        System.DateTime date = new System.DateTime(Year, Month, Day);
        return date.ToShortDateString();
    }
}
