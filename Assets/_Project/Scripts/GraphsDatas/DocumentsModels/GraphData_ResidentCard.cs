using UnityEditor;

/// <summary>
/// Data to save and load a "Document for customers" inside a GraphView
/// </summary>
[System.Serializable]
public class GraphData_ResidentCard
{
    public string Name;
    public string Surname;
    public FRoom RoomNumber;
    public string IDCardNumber;
    public string SignaturePath;

    public GraphData_ResidentCard(ResidentCard doc)
    {
        Name = doc.Name;
        Surname = doc.Surname;
        RoomNumber = doc.RoomNumber;
        IDCardNumber = doc.IDCardNumber;
        SignaturePath = AssetDatabase.GetAssetPath(doc.Signature);
    }
}