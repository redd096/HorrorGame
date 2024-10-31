using UnityEditor;

/// <summary>
/// Data to save and load a "Document for customers" inside a GraphView
/// </summary>
[System.Serializable]
public class GraphData_RenunciationCard
{
    public string Name;
    public string Surname;
    public string IDCardNumber;
    public FDate DateBirth;
    public string SignaturePath;

    public GraphData_RenunciationCard(RenunciationCard doc)
    {
        Name = doc.Name;
        Surname = doc.Surname;
        IDCardNumber = doc.IDCardNumber;
        DateBirth = doc.DateBirth;
        SignaturePath = AssetDatabase.GetAssetPath(doc.Signature);
    }
}