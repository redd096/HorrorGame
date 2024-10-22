using UnityEngine.EventSystems;

/// <summary>
/// Click this to call next client
/// </summary>
public class Bell : InteractableBase, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactable == false)
            return;

        callbacks.BellClick();
    }
}
