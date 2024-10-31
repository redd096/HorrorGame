using UnityEngine;

/// <summary>
/// In a level there are various customers and events. This is the scriptable object to declare a customer
/// </summary>
[CreateAssetMenu(menuName = "HORROR GAME/Customer")]
public class CustomerData : ScriptableObject
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
