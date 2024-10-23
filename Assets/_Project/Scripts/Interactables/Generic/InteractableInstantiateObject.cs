using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Interactables in the left screen that can be click to instantiate interactable on the right screen
/// </summary>
public class InteractableInstantiateObject : InteractableOnTheLeft
{
    [SerializeField] InteractableOnTheRight prefab;

    private InteractableOnTheRight instantiatedObjectInScene;

    public override void OnPointerClick_Event(PointerEventData eventData)
    {
        base.OnPointerClick_Event(eventData);

        //instantiate if null
        if (instantiatedObjectInScene == null)
        {
            instantiatedObjectInScene = Instantiate(prefab);
            instantiatedObjectInScene.Init(callbacks, this);
        }

        //on click, show instantiated object
        callbacks.ClickAndInstantiateInteractable(this, instantiatedObjectInScene);
    }
}