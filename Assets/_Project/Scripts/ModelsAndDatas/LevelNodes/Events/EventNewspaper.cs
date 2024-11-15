
/// <summary>
/// Newspaper event values
/// </summary>
[System.Serializable]
public class EventNewspaper
{
    public NewspaperBehaviour NewspaperPrefab;

    public EventNewspaper Clone()
    {
        return new EventNewspaper()
        {
            NewspaperPrefab = NewspaperPrefab
        };
    }
}
