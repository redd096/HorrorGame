using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Customers values
/// </summary>
[System.Serializable]
public class CustomerModel
{
    public List<Sprite> CustomerImage = new List<Sprite>();
    public string Dialogue;

    [Space]

    public bool GiveIDCard;
    public IDCard IDCard = new IDCard();

    public bool GiveRenunciationCard;
    public RenunciationCard RenunciationCard = new RenunciationCard();

    public bool GiveResidentCard;
    public ResidentCard ResidentCard = new ResidentCard();

    public bool GivePoliceCard;
    public PoliceCard PoliceCard = new PoliceCard();

    [Space]

    public List<FGiveToUser> ObjectsToGiveToPlayer = new List<FGiveToUser>();

    public CustomerModel Clone()
    {
        return new CustomerModel()
        {
            CustomerImage = new List<Sprite>(CustomerImage),
            Dialogue = Dialogue,
            GiveIDCard = GiveIDCard,
            IDCard = new IDCard()
            {
                Name = IDCard.Name,
                Surname = IDCard.Surname,
                CardNumber = IDCard.CardNumber,
                DateBirth = IDCard.DateBirth,
                Signature = IDCard.Signature,
                Photo = IDCard.Photo,
            },
            GiveRenunciationCard = GiveRenunciationCard,
            RenunciationCard = new RenunciationCard()
            {
                Name = RenunciationCard.Name,
                Surname = RenunciationCard.Surname,
                IDCardNumber = RenunciationCard.IDCardNumber,
                DateBirth = RenunciationCard.DateBirth,
                Signature = RenunciationCard.Signature,
            },
            GiveResidentCard = GiveResidentCard,
            ResidentCard = new ResidentCard()
            {
                Name = ResidentCard.Name,
                Surname = ResidentCard.Surname,
                IDCardNumber = ResidentCard.IDCardNumber,
                RoomNumber = ResidentCard.RoomNumber,
                Signature = ResidentCard.Signature,
            },
            GivePoliceCard = GivePoliceCard,
            PoliceCard = new PoliceCard()
            {
                Name = PoliceCard.Name,
                Surname = PoliceCard.Surname,
                IDCardNumber = PoliceCard.IDCardNumber,
                PoliceStamp = PoliceCard.PoliceStamp,
                NeedSecondStamp = PoliceCard.NeedSecondStamp,
                PoliceStamp2 = PoliceCard.PoliceStamp2
            },
            ObjectsToGiveToPlayer = new List<FGiveToUser>(ObjectsToGiveToPlayer),
        };
    }
}
