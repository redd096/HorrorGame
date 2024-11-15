
/// <summary>
/// Delay before move to next node
/// </summary>
[System.Serializable]
public class DelayForSeconds
{
    public float Delay;

    public DelayForSeconds Clone()
    {
        return new DelayForSeconds()
        {
            Delay = Delay,
        };
    }
}