using UnityEditor;

/// <summary>
/// Data to save and load a "Document for customers" inside a GraphView
/// </summary>
[System.Serializable]
public class GraphData_IDCard
{
    public string Name;
    public string Surname;
    public FDate DateBirth;
    public string CardNumber;
    public string SignaturePath;
    public string PhotoPath;

    public GraphData_IDCard(IDCard doc)
    {
        Name = doc.Name;
        Surname = doc.Surname;
        DateBirth = doc.DateBirth;
        CardNumber = doc.CardNumber;
        SignaturePath = AssetDatabase.GetAssetPath(doc.Signature);
        PhotoPath = AssetDatabase.GetAssetPath(doc.Photo);
    }
}