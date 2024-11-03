
/// <summary>
/// Newspaper event values
/// </summary>
[System.Serializable]
public class EventNewspaper
{
    public long NewspaperFileID;
    public string NewspaperName;

    public EventNewspaper Clone()
    {
        return new EventNewspaper()
        {
            NewspaperFileID = NewspaperFileID,
            NewspaperName = NewspaperName,
        };
    }
}
