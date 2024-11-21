using UnityEngine.EventSystems;

/// <summary>
/// Click this to start camera flash event
/// </summary>
public class CameraInteractable : InteractableOnTheLeft
{
    public override void OnPointerClick_Event(PointerEventData eventData)
    {
        base.OnPointerClick_Event(eventData);

        callbacks.CameraClick();
    }
}
