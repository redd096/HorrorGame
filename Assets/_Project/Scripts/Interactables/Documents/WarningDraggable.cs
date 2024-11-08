using redd096;
using TMPro;
using UnityEngine;

/// <summary>
/// This is a document to give to player when he do the wrong choice (enter customers when they have wrong documents)
/// </summary>
public class WarningDraggable : DocumentDraggable
{
    [SerializeField] TMP_Text numberOfWarningsText;
    [SerializeField] TMP_Text warningText;

    public void InitDocument(int counter, string message)
    {
        numberOfWarningsText.text = NumberToWords.ToOrdinalNumberInWords(counter).ToUpper() + " WARNING:";
        warningText.text = message;
    }
}
