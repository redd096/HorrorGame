using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keep track of residents in the hotel
/// </summary>
public class ResidentsManager : MonoBehaviour
{
    [SerializeField] List<ResidentData> listOfResidents = new List<ResidentData>();

    /// <summary>
    /// Remove a resident from the list
    /// </summary>
    /// <param name="resident"></param>
    public void RemoveResident(ResidentData resident)
    {
        if (listOfResidents.Contains(resident))
            listOfResidents.Remove(resident);
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
