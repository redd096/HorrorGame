using UnityEngine;

/// <summary>
/// Play ot stop an animation on background image
/// </summary>
[System.Serializable]
public class EventBackgroundAnimation
{
    [Tooltip("Play or stop animation")] public bool PlayAnimation;
    [Tooltip("If play is true, play this animation")] public AnimationClip AnimationToPlay;
    public bool WaitAnimation;

    public EventBackgroundAnimation Clone()
    {
        return new EventBackgroundAnimation()
        {
            PlayAnimation = PlayAnimation,
            AnimationToPlay = AnimationToPlay,
            WaitAnimation = WaitAnimation
        };
    }
}
