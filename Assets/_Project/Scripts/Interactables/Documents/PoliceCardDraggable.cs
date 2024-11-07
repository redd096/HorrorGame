using TMPro;
using UnityEngine;

/// <summary>
/// This is a document to drag in scene
/// </summary>
public class PoliceCardDraggable : DocumentDraggable
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text dateText;
    [SerializeField] TMP_Text signatureText;
    [SerializeField] GameObject firstStamp;
    [SerializeField] GameObject secondStamp;

    public void InitDocument(PoliceCard doc)
    {
        nameText.text = doc.Name + " " + doc.Surname;
        dateText.text = doc.ValidateDate.ToAmericanString();
        signatureText.text = doc.Signature;
        firstStamp.SetActive(doc.HasFirstStamp);
        secondStamp.SetActive(doc.HasSecondStamp);
    }
}