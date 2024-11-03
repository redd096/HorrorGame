
/// <summary>
/// Newspaper event values
/// </summary>
[System.Serializable]
public class EventNewspaper
{
    public int NewspaperInstanceID;
    public string NewspaperName;

    public EventNewspaper Clone()
    {
        return new EventNewspaper()
        {
            NewspaperInstanceID = NewspaperInstanceID,
            NewspaperName = NewspaperName,
        };
    }
}
