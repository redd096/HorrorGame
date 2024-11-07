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
    [SerializeField] TMP_Text signatureText;

    public void InitDocument(IDCard doc)
    {
        nameText.text = doc.Name + " " + doc.Surname;
        idNumberText.text = doc.CardNumber;
        dateBirthText.text = doc.BirthDate.ToEuropeString();
        expireDateText.text = doc.ExpireDate.ToEuropeString();
        signatureText.text = doc.Signature;
        customerImage.sprite = doc.Photo;
    }
}