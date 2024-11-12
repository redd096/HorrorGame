using UnityEngine;
using PrimeTween;

/// <summary>
/// This is used by LevelManager and is specific for the events (not customers or other)
/// </summary>
public class LevelEventsManager : MonoBehaviour
{
    [SerializeField] CanvasGroup newspapersContainer;
    [SerializeField] float durationNewspaper = 3;

    /// <summary>
    /// Show for few seconds the newspaper
    /// </summary>
    /// <param name="newspaperPrefab"></param>
    public Sequence ShowNewspaper(NewspaperBehaviour newspaperPrefab, ResidentData killedResident)
    {
        //instantiate newspaper
        if (newspaperPrefab == null)
        {
            Debug.LogError("Newspaper is null", gameObject);
            return default;
        }

        //instantiate newspaper and immediatly fade in
        NewspaperBehaviour newspaper = Instantiate(newspaperPrefab, newspapersContainer.transform);
        newspaper.Init(killedResident);
        newspapersContainer.gameObject.SetActive(true);
        newspapersContainer.alpha = 1;

        //then wait few seconds
        Sequence sequence = Sequence.Create();
        sequence.ChainDelay(durationNewspaper);

        //fade out
        sequence.Chain(Tween.Alpha(newspapersContainer, 0, duration: 1f));
        sequence.ChainCallback(() =>
        {
            //hide container and newspaper
            newspapersContainer.gameObject.SetActive(false);
            Destroy(newspaper.gameObject);
        });

        return sequence;
    }
}
