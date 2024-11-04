using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a document to drag in scene
/// </summary>
public class IDCardDraggable : DocumentDraggable
{
    [SerializeField] Image customerImage;
    [SerializeField] TMP_Text idNumberText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text dateBirthText;
    [SerializeField] TMP_Text expireDateText;

    public void InitDocument(IDCard doc)
    {
        customerImage.sprite = doc.Photo;
        idNumberText.text = doc.CardNumber;
        nameText.text = doc.Name + " " + doc.Surname;
        dateBirthText.text = doc.DateBirth.ToString();
    }
}