using UnityEngine;

/// <summary>
/// Play an animation on background image
/// </summary>
[System.Serializable]
public class EventStartBackgroundAnimation
{
    public AnimationClip AnimationToPlay;
    public bool WaitAnimation;
    public float DelayAfterAnimation;

    public EventStartBackgroundAnimation Clone()
    {
        return new EventStartBackgroundAnimation()
        {
            AnimationToPlay = AnimationToPlay,
            WaitAnimation = WaitAnimation,
            DelayAfterAnimation = DelayAfterAnimation
        };
    }
}
