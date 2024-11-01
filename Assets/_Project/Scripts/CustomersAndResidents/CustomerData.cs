using UnityEngine;

/// <summary>
/// In a level there are various customers and events. This is the scriptable object to declare a customer
/// </summary>
[CreateAssetMenu(menuName = "HORROR GAME/Customer")]
public class CustomerData : ScriptableObject
{
    public CustomerModel Customer;
}
