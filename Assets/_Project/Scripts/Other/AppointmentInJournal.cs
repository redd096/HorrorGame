using TMPro;
using UnityEngine;

/// <summary>
/// In journal there is the appointments' chapter. This is used to update their values
/// </summary>
public class AppointmentInJournal : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text professionText;
    [SerializeField] private TMP_Text reasonText;
    
    /// <summary>
    /// Set values in UI
    /// </summary>
    /// <param name="appointmentData"></param>
    public void Initialize(AppointmentData appointmentData)
    {
        nameText.text = appointmentData.Name + " " + appointmentData.Surname;
        professionText.text = appointmentData.Profession;
        reasonText.text = appointmentData.AppointmentReason;
    }
}
