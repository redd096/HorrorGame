using UnityEngine;

/// <summary>
/// Every interactable in scene
/// </summary>
public abstract class InteractableBase : MonoBehaviour
{
    protected IInteractablesEvents callbacks;
    protected bool interactable;

    public bool Interactable => interactable;

    protected virtual void Awake()
    {
        //save refs
        callbacks = DeskManager.instance.DeskStateMachine;
    }

    /// <summary>
    /// Set if user can interact/drag this object
    /// </summary>
    /// <param name="interactable"></param>
    public void SetInteractable(bool interactable)
    {
        this.interactable = interactable;
    }
}
