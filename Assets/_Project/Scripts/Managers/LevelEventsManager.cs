using redd096;
using UnityEngine;
using PrimeTween;

/// <summary>
/// This is used by LevelManager and is specific for the events (not customers or other)
/// </summary>
public class LevelEventsManager : SimpleInstance<LevelEventsManager>
{
    [SerializeField] CanvasGroup newspapersContainer;

    public Transform NewspapersContainer => newspapersContainer.transform;

    /// <summary>
    /// Show for few seconds the newspaper
    /// </summary>
    /// <param name="newspaperName"></param>
    public Sequence ShowNewspaper(string newspaperName)
    {
        //find newspaper in scene
        Transform newspaper = newspapersContainer.transform.Find(newspaperName);
        if (newspaper == null)
        {
            Debug.LogError($"Impossible to find newspaper with this name {newspaperName}");
            return default;
        }

        //show container and newspaper, and immediatly fade in
        newspapersContainer.gameObject.SetActive(true);
        newspaper.gameObject.SetActive(true);
        newspapersContainer.alpha = 1;

        //then wait few seconds
        Sequence sequence = Sequence.Create();
        sequence.ChainDelay(3);

        //fade out
        sequence.Chain(Tween.Alpha(newspapersContainer, 0, duration: 1f));
        sequence.ChainCallback(() =>
        {
            //hide container and newspaper
            newspapersContainer.gameObject.SetActive(false);
            newspaper.gameObject.SetActive(false);
        });

        return sequence;
    }
}
