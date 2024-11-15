
/// <summary>
/// Stop background animation event
/// </summary>
[System.Serializable]
public class EventStopBackgroundAnimation
{
    public float DelayBeforeNextNode;

    public EventStopBackgroundAnimation Clone()
    {
        return new EventStopBackgroundAnimation()
        {
            DelayBeforeNextNode = DelayBeforeNextNode,
        };
    }
}