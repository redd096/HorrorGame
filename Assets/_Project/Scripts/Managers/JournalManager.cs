using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This is used to update journal in game
/// </summary>
public class JournalManager : MonoBehaviour
{
    [Header("Pages")]
    [SerializeField] private GameObject[] pages;
    [SerializeField] private GameObject chapterRules;
    [SerializeField] private GameObject chapterResidents;
    [SerializeField] private GameObject chapterAppointments;
    
    [Header("UI")]
    [SerializeField] private GameObject[] rules;
    [SerializeField] private ResidentInJournal[] residents;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private AppointmentInJournal appointmentsPrefab;
    [SerializeField] private Transform appointmentsContainer;
    
    private int currentPage;

    public System.Action<ResidentData> OnClickResidentForArrest;

    /// <summary>
    /// Initialize when start level
    /// </summary>
    public void InitializeForThisLevel(FDate currentDate, CheckPlayerChoiceManager choiceManager, ResidentsManager residentsManager, AppointmentsManager appointmentsManager)
    {
        //update values in UI
        UpdateDate(currentDate);
        UpdateRules(choiceManager);
        UpdateResidents(residentsManager);
        UpdateAppointments(appointmentsManager);
    }

    /// <summary>
    /// When journal is used for arrest at the end of the day, activate ArrestButton on every resident
    /// </summary>
    public void SetResidentsForArrest(ResidentsManager residentsManager)
    {
        UpdateResidents(residentsManager);
        foreach (var resident in residents)
        {
            //be sure the resident was initialized
            ResidentData residentData = resident.ResidentData;
            if (residentData == null)
                continue;
            
            //set arrest button
            resident.EnableArrestButton(residentsManager.IsResidentAlive(residentData) && residentsManager.IsResidentFree(residentData), 
                clickedResident => OnClickResidentForArrest?.Invoke(clickedResident));
        }
    }
    
    #region update ui values

    /// <summary>
    /// Update date text
    /// </summary>
    /// <param name="date"></param>
    public void UpdateDate(FDate date)
    {
        dateText.text = date.ToAmericanString();
    }

    /// <summary>
    /// Show or hide rules in UI
    /// </summary>
    /// <param name="choiceManager"></param>
    public void UpdateRules(CheckPlayerChoiceManager choiceManager)
    {
        rules[0].SetActive(choiceManager.ShowAlwaysID);
        rules[1].SetActive(choiceManager.OutsiderShowRenunciationCard);
        rules[2].SetActive(choiceManager.ResidentShowResidentCard);
        rules[3].SetActive(choiceManager.OnlyAdults);
        rules[4].SetActive(choiceManager.WorkersShowAppointmentCard);
        rules[5].SetActive(choiceManager.NeedPoliceCard);
        rules[6].SetActive(choiceManager.PoliceCardNeedSecondStamp);
        rules[7].SetActive(false);
    }

    /// <summary>
    /// Update image and text for every Resident
    /// </summary>
    /// <param name="residentsManager"></param>
    public void UpdateResidents(ResidentsManager residentsManager)
    {
        List<ResidentData> listOfResidents = residentsManager.ListOfResidents;
        for (int i = 0; i < residents.Length; i++)
        {
            residents[i].gameObject.SetActive(true);
            if (listOfResidents.Count > i)
            {
                ResidentData residentData = listOfResidents[i];
                residents[i].Initialize(residentData, residentsManager.IsResidentAlive(residentData), residentsManager.IsResidentFree(residentData));
            }
            else
            {
                //if there are more residents in journal than residents in ResidentsManager, just hide it
                residents[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Update texts for appointments
    /// </summary>
    /// <param name="appointmentsManager"></param>
    public void UpdateAppointments(AppointmentsManager appointmentsManager)
    {
        for (int i = appointmentsContainer.childCount - 1; i >= 0; i--)
            Destroy(appointmentsContainer.GetChild(i).gameObject);
        AppointmentData[] listOfAppointments = appointmentsManager.Appointments;
        for (int i = 0; i < listOfAppointments.Length; i++)
        {
            var appointment = Instantiate(appointmentsPrefab, appointmentsContainer);
            appointment.Initialize(listOfAppointments[i]);
        }
    }
    
    #endregion
    
    #region go to page functions

    /// <summary>
    /// Deactivate current page and open page at index
    /// </summary>
    /// <param name="index"></param>
    public void GoToPage(int index)
    {
        pages[currentPage].SetActive(false);
        currentPage = index;
        pages[currentPage].SetActive(true);
    }

    /// <summary>
    /// If page is inside pages array, deactivate current page and open new one
    /// </summary>
    /// <param name="page"></param>
    public void GoToPage(GameObject page)
    {
        int index = System.Array.IndexOf(pages, page);
        if (index >= 0)
            GoToPage(index);
    }

    /// <summary>
    /// Deactivate current page and open page, also if not inside pages array
    /// </summary>
    /// <param name="page"></param>
    public void GoToPageNotInArray(GameObject page)
    {
        pages[currentPage].SetActive(false);
        page.SetActive(true);
    }
    
    #endregion
    
    #region functions for buttons in UI
    
    public void GoToNextPage()
    {
        if (currentPage < pages.Length - 1)
            GoToPage(currentPage + 1);
    }

    public void GoToPreviousPage()
    {
        if (currentPage > 0)
            GoToPage(currentPage - 1);
    }

    public void GoToChapterRules()
    {
        GoToPage(chapterRules);
    }

    public void GoToChapterResidents()
    {
        GoToPage(chapterResidents);
    }

    public void GoToChapterAppointments()
    {
        GoToPage(chapterAppointments);
    }
    
    #endregion
}