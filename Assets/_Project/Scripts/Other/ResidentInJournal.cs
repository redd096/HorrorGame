using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// In journal there is the residents' chapter. This is used to update their values
/// </summary>
public class ResidentInJournal : MonoBehaviour
{
    [SerializeField] private Image photoImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text idCardText;
    [SerializeField] private TMP_Text roomNumberText;
    
    /// <summary>
    /// Set values in UI
    /// </summary>
    /// <param name="residentData"></param>
    public void Initialize(ResidentData residentData)
    {
        photoImage.sprite = residentData.Photo;
        nameText.text = residentData.Name + " " + residentData.Surname;
        idCardText.text = residentData.IDCardNumber;
        roomNumberText.text = residentData.RoomNumber.ToRoomString();
    }
}