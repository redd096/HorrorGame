
/// <summary>
/// Newspaper event values
/// </summary>
[System.Serializable]
public class EventNewspaper
{
    public string NewspaperName;

    public EventNewspaper Clone()
    {
        return new EventNewspaper()
        {
            NewspaperName = NewspaperName,
        };
    }
}
