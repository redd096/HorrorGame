using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Keep track of residents in the hotel
/// </summary>
public class ResidentsManager : MonoBehaviour
{
    [SerializeField] List<ResidentData> listOfResidents = new List<ResidentData>();
    
    private List<ResidentData> deadResidents = new List<ResidentData>();
    private List<ResidentData> arrestedResidents = new List<ResidentData>();
    
    public List<ResidentData> ListOfResidents => listOfResidents;
    public List<ResidentData> DeadResidents => deadResidents;
    public List<ResidentData> ArrestedResidents => arrestedResidents;

    /// <summary>
    /// Check if resident is dead or alive
    /// </summary>
    /// <param name="resident"></param>
    /// <returns></returns>
    public bool IsResidentAlive(ResidentData resident)
    {
        return deadResidents.Contains(resident) == false;
    }

    /// <summary>
    /// Check if resident is arrested or free
    /// </summary>
    /// <param name="resident"></param>
    /// <returns></returns>
    public bool IsResidentFree(ResidentData resident)
    {
        return arrestedResidents.Contains(resident) == false;
    }

    /// <summary>
    /// Arrest a resident from the list
    /// </summary>
    /// <param name="resident"></param>
    public ResidentData ArrestResident(ResidentData resident)
    {
        if (arrestedResidents.Contains(resident) == false)
        {
            arrestedResidents.Add(resident);
            return resident;
        }
        
        Debug.LogError("Resident is already arrested: " + resident, gameObject);
        return null;
    }

    /// <summary>
    /// Kill a resident from the list
    /// </summary>
    /// <param name="resident"></param>
    public ResidentData KillResident(ResidentData resident)
    {
        if (deadResidents.Contains(resident) == false)
        {
            deadResidents.Add(resident);
            return resident;
        }
        
        Debug.LogError("Resident is already dead: " + resident, gameObject);
        return null;
    }

    /// <summary>
    /// Kill a random resident from the list
    /// </summary>
    public ResidentData KillRandomResident()
    {
        //find random resident still alive and not arrested
        List<ResidentData> residents = listOfResidents.Where(x => IsResidentAlive(x) && IsResidentFree(x)).ToList();
        if (residents.Count > 0)
        {
            return KillResident(residents[Random.Range(0, residents.Count)]);
        }

        Debug.LogError("There aren't residents in the list", gameObject);
        return null;
    }

    /// <summary>
    /// Check if this IDCard is for a resident
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="resident"></param>
    /// <returns></returns>
    public bool IsResident(IDCard doc, out ResidentData resident)
    {
        //check every resident
        foreach (var v in listOfResidents)
        {
            //if this document's values are the same, return true
            if (doc.Photo == v.Photo
                && doc.Name == v.Name
                && doc.Surname == v.Surname
                && doc.CardNumber == v.IDCardNumber)
            {
                resident = v;
                return true;
            }
        }

        resident = null;
        return false;
    }
}
