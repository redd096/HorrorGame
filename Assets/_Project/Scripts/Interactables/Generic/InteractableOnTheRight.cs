
/// <summary>
/// This is the interactable that user can move on the right screen. It has a reference to its copy in the left screen
/// </summary>
public class InteractableOnTheRight : InteractableBase
{
    private InteractableOnTheLeft copyInScene;
    public InteractableOnTheLeft CopyInScene => copyInScene;

    /// <summary>
    /// Initialize with its copy in the left screen
    /// </summary>
    /// <param name="copyInScene"></param>
    public void Init(IInteractablesEvents callbacks, InteractableOnTheLeft copyInScene)
    {
        Init(callbacks);
        this.copyInScene = copyInScene;
    }
}
