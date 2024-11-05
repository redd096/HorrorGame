using TMPro;
using UnityEngine;

/// <summary>
/// This is a document to drag in scene
/// </summary>
public class ResidentCardDraggable : DocumentDraggable
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text idNumberText;
    [SerializeField] TMP_Text roomText;
    [SerializeField] TMP_Text signatureText;

    public void InitDocument(ResidentCard doc)
    {
        nameText.text = doc.Name + " " + doc.Surname;
        idNumberText.text = doc.IDCardNumber;
        roomText.text = doc.RoomNumber.ToString();
        signatureText.text = doc.Signature;
    }
}