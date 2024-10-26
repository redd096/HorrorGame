
/// <summary>
/// This is the interactable in "scene". 
/// Some interactables user can click to interact, others like documents are just to view
/// </summary>
public class InteractableOnTheLeft : InteractableBase
{
    /// <summary>
    /// Show or hide in scene
    /// </summary>
    /// <param name="show"></param>
    public void ShowInScene(bool show)
    {
        gameObject.SetActive(show);
    }
}
