using redd096;
using redd096.Attributes;
using UnityEngine;

/// <summary>
/// Check if player choice is correct or not
/// </summary>
public class CheckPlayerChoiceManager : SimpleInstance<CheckPlayerChoiceManager>
{
    [SerializeField] bool showAlwaysID = true;
    [SerializeField] bool onlyAdults = true;
    [SerializeField] int adultAge = 18;
    [SerializeField] bool residentShowResidentCard = true;
    [SerializeField] bool outsiderShowRenunciationCard = true;
    [SerializeField] bool needPoliceCard = false;
    [SerializeField] bool policeCardNeedSecondStamp = false;

    [Button]
    void StampCurrentSituation()
    {
        if (LevelManager.instance.CurrentNode is CustomerData customerData == false)
        {
            Debug.LogError("Trying to check player choice on a wrong LevelNode");
            return;
        }

        bool shouldEnter = CheckCustomer(customerData.Customer, out string problem);
        if (shouldEnter)
            Debug.Log("<color=green>Everything OK</color>");
        else
            Debug.Log($"<color=red>{problem}</color>");
    }

    public void CheckPlayerChoice(LevelNodeData currentNode, bool currentChoice)
    {
        if (currentNode is CustomerData customerData == false)
        {
            Debug.LogError("Trying to check player choice on a wrong LevelNode");
            return;
        }

        bool shouldEnter = CheckCustomer(customerData.Customer, out string problem);        
        if (shouldEnter == currentChoice)
            Debug.Log("<color=green>Correct choice!</color>");
        else
            Debug.Log($"<color=red>{problem}</color>");
    }

    private bool CheckCustomer(Customer customer, out string problem)
    {
        //check if show id
        if (showAlwaysID && customer.GiveIDCard == false)
        {
            problem = "Customer didn't showed the ID Card";
            return false;
        }

        IDCard idCard = customer.IDCard;

        //check if adult
        if (onlyAdults)
        {
            System.DateTime currentDate = LevelManager.instance.CurrentDate.GetDateTime();
            System.DateTime birthDate = idCard.BirthDate.GetDateTime();
            if (birthDate.AddYears(adultAge) > currentDate)     //if birthday + 18 years is after today, this day still didn't come
            {
                problem = "Customer doesn't have 18 years";
                return false;
            }
        }

        //check if show resident card
        if (ResidentsManager.instance.IsResident(idCard, out ResidentData resident))
        {
            if (residentShowResidentCard && customer.GiveResidentCard == false)
            {
                problem = "Customer didn't showed the Resident Card";
                return false;
            }

            if (customer.ResidentCard.IsCorrect(idCard, resident, out problem) == false)
                return false;
        }
        //or show renunciation card
        else
        {
            if (outsiderShowRenunciationCard && customer.GiveRenunciationCard == false)
            {
                problem = "Customer didn't showed the Renunciation Card";
                return false;
            }

            if (customer.RenunciationCard.IsCorrect(idCard, out problem) == false)
                return false;
        }

        //check appointment card
        if (customer.GiveAppointmentCard)
        {
            if (AppointmentsManager.instance.IsCorrectAppointment(customer.AppointmentCard, out AppointmentData appointment, out problem) == false)
                return false;

            if (customer.AppointmentCard.IsCorrect(idCard, LevelManager.instance.CurrentDate, appointment, out problem) == false)
                return false;
        }

        //check police card
        if (needPoliceCard)
        {
            if (customer.GivePoliceCard == false)
            {
                problem = "Customer didn't showed the Police Card";
                return false;
            }

            if (customer.PoliceCard.IsCorrect(idCard, LevelManager.instance.CurrentDate, policeCardNeedSecondStamp, out problem) == false)
                return false;
        }

        problem = "Customer had everything OK";
        return true;
    }
}
