using UnityEditor;
using UnityEngine;

/// <summary>
/// Data to save and load a "Customer" or "Event" inside a GraphView
/// </summary>
[System.Serializable]
public class GraphData_CustomerData
{
    public string CustomerImagePath;
    public string Dialogue;

    [Space]

    public bool GiveIDCard;
    public GraphData_IDCard IDCard;

    public bool GiveRenunciationCard;
    public GraphData_RenunciationCard RenunciationCard;

    public bool GiveResidentCard;
    public GraphData_ResidentCard ResidentCard;

    public bool GivePoliceDocument;
    public GraphData_PoliceDocument PoliceDocument;

    [Space]

    public GraphData_FGiveToUser[] ObjectsToGiveToPlayer;

    public GraphData_CustomerData(CustomerData data)
    {
        CustomerImagePath = AssetDatabase.GetAssetPath(data.CustomerImage);
        Dialogue = data.Dialogue;

        GiveIDCard = data.GiveIDCard;
        IDCard = new GraphData_IDCard(data.IDCard);

        GiveRenunciationCard = data.GiveRenunciationCard;
        RenunciationCard = new GraphData_RenunciationCard(data.RenunciationCard);

        GiveResidentCard = data.GiveResidentCard;
        ResidentCard = new GraphData_ResidentCard(data.ResidentCard);

        GivePoliceDocument = data.GivePoliceDocument;
        PoliceDocument = new GraphData_PoliceDocument(data.PoliceDocument);

        if (data.ObjectsToGiveToPlayer != null)
        {
            ObjectsToGiveToPlayer = new GraphData_FGiveToUser[data.ObjectsToGiveToPlayer.Length];
            for (int i = 0; i < data.ObjectsToGiveToPlayer.Length; i++)
            {
                ObjectsToGiveToPlayer[i] = new GraphData_FGiveToUser(data.ObjectsToGiveToPlayer[i]);
            }
        }
        else
        {
            ObjectsToGiveToPlayer = null;
        }
    }
}