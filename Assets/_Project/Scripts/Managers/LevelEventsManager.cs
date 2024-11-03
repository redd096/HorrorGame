using redd096;
using UnityEngine;

public class LevelEventsManager : SimpleInstance<LevelEventsManager>
{
    public Transform NewspapersContainer;

    /// <summary>
    /// Show or hide newspaper
    /// </summary>
    /// <param name="newspaperName"></param>
    /// <param name="show"></param>
    public void ShowNewspaper(string newspaperName, bool show)
    {
        //show or hide container
        NewspapersContainer.gameObject.SetActive(show);

        //and newspaper
        Transform newspaper = NewspapersContainer.Find(newspaperName);
        if (newspaper) newspaper.gameObject.SetActive(show);
    }
}
