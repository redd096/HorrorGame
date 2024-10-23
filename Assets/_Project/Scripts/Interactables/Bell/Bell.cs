using UnityEngine.EventSystems;

/// <summary>
/// Click this to call next client
/// </summary>
public class Bell : InteractableBase
{
    public override void OnPointerClick_Event(PointerEventData eventData)
    {
        base.OnPointerClick_Event(eventData);

        callbacks.BellClick();
    }
}
