using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Customers values
/// </summary>
[System.Serializable]
public class Customer
{
    public List<Sprite> CustomerImage = new List<Sprite>();
    public string DialogueWhenCome;
    public string DialogueWhenPlayerSayNo;
    public string DialogueWhenPlayerSayYes;

    [Space]

    public bool GiveIDCard;
    public IDCard IDCard = new IDCard();

    public bool GiveRenunciationCard;
    public RenunciationCard RenunciationCard = new RenunciationCard();

    public bool GiveResidentCard;
    public ResidentCard ResidentCard = new ResidentCard();

    public bool GivePoliceCard;
    public PoliceCard PoliceCard = new PoliceCard();

    public bool GiveAppointmentCard;
    public AppointmentCard AppointmentCard = new AppointmentCard();

    [Space]

    public List<FGiveToUser> ObjectsToGiveToPlayer = new List<FGiveToUser>();

    public Customer Clone()
    {
        return new Customer()
        {
            CustomerImage = new List<Sprite>(CustomerImage),
            DialogueWhenCome = DialogueWhenCome,
            DialogueWhenPlayerSayNo = DialogueWhenPlayerSayNo,
            DialogueWhenPlayerSayYes = DialogueWhenPlayerSayYes,
            GiveIDCard = GiveIDCard,
            IDCard = IDCard.Clone(),
            GiveRenunciationCard = GiveRenunciationCard,
            RenunciationCard = RenunciationCard.Clone(),
            GiveResidentCard = GiveResidentCard,
            ResidentCard = ResidentCard.Clone(),
            GivePoliceCard = GivePoliceCard,
            PoliceCard = PoliceCard.Clone(),
            GiveAppointmentCard = GiveAppointmentCard,
            AppointmentCard = AppointmentCard.Clone(),
            ObjectsToGiveToPlayer = new List<FGiveToUser>(ObjectsToGiveToPlayer),
        };
    }
}
