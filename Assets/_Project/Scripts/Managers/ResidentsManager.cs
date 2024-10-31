using UnityEngine;

/// <summary>
/// Keep track of residents in the hotel
/// </summary>
public class ResidentsManager : MonoBehaviour
{
    [SerializeField] ResidentData[] listOfResidents;

    public ResidentData[] ListOfResidents => listOfResidents;
}
