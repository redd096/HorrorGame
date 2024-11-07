using redd096;
using UnityEngine;

/// <summary>
/// Keep track of residents in the hotel
/// </summary>
public class ResidentsManager : SimpleInstance<ResidentsManager>
{
    [SerializeField] ResidentData[] listOfResidents;

    public ResidentData[] ListOfResidents => listOfResidents;

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
