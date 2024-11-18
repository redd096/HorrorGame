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

    [Space] 
    [SerializeField] private GameObject deadImage;
    [SerializeField] private GameObject arrestImage;
    [SerializeField] private GameObject arrestButton;

    private System.Action<ResidentData> onClick;

    public ResidentData ResidentData { get; private set; }

    /// <summary>
    /// Set values in UI
    /// </summary>
    public void Initialize(ResidentData residentData, bool isAlive, bool isFree)
    {
        ResidentData = residentData;
        
        photoImage.sprite = residentData.Photo;
        nameText.text = residentData.Name + " " + residentData.Surname;
        idCardText.text = residentData.IDCardNumber;
        roomNumberText.text = "Room " + residentData.RoomNumber.ToRoomString();
        
        //show or hide objects when dead or in arrest
        deadImage.SetActive(isAlive == false);
        arrestImage.SetActive(isFree == false);
    }

    /// <summary>
    /// Enable or disable arrestButton
    /// </summary>
    public void EnableArrestButton(bool isEnabled, System.Action<ResidentData> onClick)
    {
        arrestButton.SetActive(isEnabled);
        this.onClick = onClick;
    }

    /// <summary>
    /// This is attached to the button in inspector
    /// </summary>
    public void OnClickButton()
    {
        onClick?.Invoke(ResidentData);
    }
}