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
    [SerializeField] private TMP_Text dateTet;
    [SerializeField] private AppointmentInJournal appointmentsPrefab;
    [SerializeField] private Transform appointmentsContainer;
    
    private int currentPage;

    /// <summary>
    /// Initialize when start level
    /// </summary>
    public void InitializeForThisLevel(FDate currentDate, CheckPlayerChoiceManager choiceManager, ResidentsManager residentsManager, AppointmentsManager appointmentsManager)
    {
        //show or hide rules
        rules[0].SetActive(choiceManager.ShowAlwaysID);
        rules[1].SetActive(choiceManager.OutsiderShowRenunciationCard);
        rules[2].SetActive(choiceManager.ResidentShowResidentCard);
        rules[3].SetActive(choiceManager.OnlyAdults);
        rules[4].SetActive(choiceManager.WorkersShowAppointmentCard);
        rules[5].SetActive(choiceManager.NeedPoliceCard);
        rules[6].SetActive(choiceManager.PoliceCardNeedSecondStamp);
        rules[7].SetActive(false);

        //update residents
        ResidentData[] listOfResidents = residentsManager.ListOfResidents;
        for (int i = 0; i < residents.Length; i++)
        {
            residents[i].gameObject.SetActive(true);
            if (listOfResidents.Length > i)
                residents[i].Initialize(listOfResidents[i]);
            else
                residents[i].gameObject.SetActive(false);
        }
        
        //set date
        dateTet.text = currentDate.ToAmericanString();
        
        //update appointments
        for (int i = appointmentsContainer.childCount - 1; i >= 0; i--)
            Destroy(appointmentsContainer.GetChild(i).gameObject);
        AppointmentData[] listOfAppointments = appointmentsManager.Appointments;
        for (int i = 0; i < listOfAppointments.Length; i++)
        {
            var appointment = Instantiate(appointmentsPrefab, appointmentsContainer);
            appointment.Initialize(listOfAppointments[i]);
        }
    }

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
}