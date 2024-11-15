using UnityEngine;

/// <summary>
/// Blood event values
/// </summary>
[System.Serializable]
public class EventBlood
{
    public AnimationClip BackgroundAnimation;

    public EventBlood Clone()
    {
        return new EventBlood()
        {
            BackgroundAnimation = BackgroundAnimation
        };
    }
}
