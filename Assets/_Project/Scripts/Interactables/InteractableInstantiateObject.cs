using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// On click, instantiate an object and call event
/// </summary>
public class InteractableInstantiateObject : InteractableBase
{
    [SerializeField] InteractableBase prefab;

    private InteractableBase instantiatedObjectInScene;

    public override void OnPointerClick_Event(PointerEventData eventData)
    {
        base.OnPointerClick_Event(eventData);

        //instantiate if null
        if (instantiatedObjectInScene == null)
            instantiatedObjectInScene = Instantiate(prefab);

        //on click, show instantiated object
        callbacks.InstantiatedDraggableClick(instantiatedObjectInScene);
    }
}