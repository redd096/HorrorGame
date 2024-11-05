using TMPro;
using UnityEngine;

/// <summary>
/// This is a document to drag in scene
/// </summary>
public class RenunciationCardDraggable : DocumentDraggable
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text idNumberText;
    [SerializeField] TMP_Text dateBirthText;
    [SerializeField] TMP_Text durationStayText;
    [SerializeField] TMP_Text signatureText;

    public void InitDocument(RenunciationCard doc)
    {
        nameText.text = doc.Name + " " + doc.Surname;
        idNumberText.text = doc.IDCardNumber;
        dateBirthText.text = doc.DateBirth.ToString();
        durationStayText.text = doc.DurationStayInDays + " Days";
        signatureText.text = doc.Signature;
    }
}