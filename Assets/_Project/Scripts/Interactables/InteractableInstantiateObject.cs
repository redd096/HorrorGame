using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// On click, instantiate an object and call event
/// </summary>
public class InteractableInstantiateObject : InteractableBase, IPointerClickHandler
{
    [SerializeField] GameObject prefab;

    private GameObject instantiatedObjectInScene;

    public void OnPointerClick(PointerEventData eventData)
    {
        //instantiate if null
        if (instantiatedObjectInScene == null)
            instantiatedObjectInScene = Instantiate(prefab);

        //on click, show instantiated object
        callbacks.InstantiatedDraggableClick(instantiatedObjectInScene);
    }
}