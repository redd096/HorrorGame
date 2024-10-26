using UnityEngine;

/// <summary>
/// Keep track residents in the hotel
/// </summary>
public class ResidentsManager : MonoBehaviour
{
    [SerializeField] ResidentData[] listOfResidents;

    public ResidentData[] ListOfResidents => listOfResidents;
}
