using redd096.Attributes;
using UnityEngine;

/// <summary>
/// Check if player choice is correct or not
/// </summary>
public class CheckPlayerChoiceManager : MonoBehaviour
{
    [Header("Rules")]
    public bool ShowAlwaysID = true;
    public bool OnlyAdults = true;
    public int AdultAge = 18;
    public bool ResidentShowResidentCard = true;
    public bool OutsiderShowRenunciationCard = true;
    public bool WorkersShowAppointmentCard = true;
    public bool NeedPoliceCard = false;
    public bool PoliceCardNeedSecondStamp = false;

    private FDate currentDate;
    private ResidentsManager residentsManager;
    private AppointmentsManager appointmentsManager;

    /// <summary>
    /// DEBUG ONLY - Stamp if this customer is OK or has some wrong document
    /// </summary>
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

    /// <summary>
    /// Initialize when start level
    /// </summary>
    /// <param name="currentDate"></param>
    public void InitializeForThisLevel(FDate currentDate, ResidentsManager residentsManager, AppointmentsManager appointmentsManager)
    {
        this.currentDate = currentDate;
        this.residentsManager = residentsManager;
        this.appointmentsManager = appointmentsManager;
    }

    /// <summary>
    /// Check if the player choice (lets customer enter or not) is correct. If wrong, call LevelManager
    /// </summary>
    /// <param name="currentNode"></param>
    /// <param name="currentChoice"></param>
    public bool CheckPlayerChoice(LevelNodeData currentNode, bool currentChoice, out string problem)
    {
        if (currentNode is CustomerData customerData == false)
        {
            Debug.LogError("Trying to check player choice on a wrong LevelNode");
            problem = "Trying to check player choice on a wrong LevelNode";
            return false;
        }

        bool shouldEnter = CheckCustomer(customerData.Customer, out problem);        
        if (shouldEnter == currentChoice)
        {
            Debug.Log("<color=green>Correct choice!</color>");
            return true;
        }
        else
        {
            Debug.Log($"<color=red>{problem}</color>");
            return false;
        }
    }

    private bool CheckCustomer(Customer customer, out string problem)
    {
        //check if show id
        if (ShowAlwaysID && customer.GiveIDCard == false)
        {
            problem = "Customer didn't showed the ID Card";
            return false;
        }

        IDCard idCard = customer.IDCard;

        //check if adult
        if (OnlyAdults)
        {
            System.DateTime date = currentDate.GetDateTime();
            System.DateTime birthDate = idCard.BirthDate.GetDateTime();
            if (birthDate.AddYears(AdultAge) > date)        //if birthday + 18 years is after today, this day still didn't come
            {
                problem = $"Customer doesn't have {AdultAge} years";
                return false;
            }
        }

        //check if show resident card
        if (residentsManager.IsResident(idCard, out ResidentData resident))
        {
            if (ResidentShowResidentCard && customer.GiveResidentCard == false)
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
            if (OutsiderShowRenunciationCard && customer.GiveRenunciationCard == false)
            {
                problem = "Customer didn't showed the Renunciation Card";
                return false;
            }

            if (customer.RenunciationCard.IsCorrect(idCard, out problem) == false)
                return false;
        }

        //check appointment card
        if (WorkersShowAppointmentCard && customer.GiveAppointmentCard)
        {
            if (appointmentsManager.IsCorrectAppointment(customer.AppointmentCard, currentDate, out AppointmentData appointment, out problem) == false)
                return false;

            if (customer.AppointmentCard.IsCorrect(idCard, currentDate, appointment, out problem) == false)
                return false;
        }

        //check police card
        if (NeedPoliceCard)
        {
            if (customer.GivePoliceCard == false)
            {
                problem = "Customer didn't showed the Police Card";
                return false;
            }

            if (customer.PoliceCard.IsCorrect(idCard, currentDate, PoliceCardNeedSecondStamp, out problem) == false)
                return false;
        }

        problem = "Customer had everything OK";
        return true;
    }
}