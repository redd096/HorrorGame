using TMPro;
using UnityEngine;

/// <summary>
/// This is a document to drag in scene
/// </summary>
public class AppointmentCardDraggable : DocumentDraggable
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text professionText;
    [SerializeField] TMP_Text dateAppointmentText;
    [SerializeField] TMP_Text appointmentReason;
    [SerializeField] GameObject stampObj;

    public void InitDocument(AppointmentCard doc)
    {
        nameText.text = doc.Name + " " + doc.Surname;
        professionText.text = $"({doc.Profession})";
        dateAppointmentText.text = doc.AppointmentDate.ToAmericanString();
        appointmentReason.text = doc.AppointmentReason;
        stampObj.SetActive(doc.HasStamp);
    }
}
