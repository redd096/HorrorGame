using UnityEngine;

/// <summary>
/// Customers values
/// </summary>
[System.Serializable]
public class CustomerModel
{
    public Sprite CustomerImage;
    public string Dialogue;

    [Space]

    public bool GiveIDCard;
    public IDCard IDCard;

    public bool GiveRenunciationCard;
    public RenunciationCard RenunciationCard;

    public bool GiveResidentCard;
    public ResidentCard ResidentCard;

    public bool GivePoliceDocument;
    public PoliceDocument PoliceDocument;

    [Space]

    public FGiveToUser[] ObjectsToGiveToPlayer;
}
