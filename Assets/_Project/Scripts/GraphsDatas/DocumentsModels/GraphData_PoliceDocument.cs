using UnityEditor;

/// <summary>
/// Data to save and load a "Document for customers" inside a GraphView
/// </summary>
[System.Serializable]
public class GraphData_PoliceDocument
{
    public string Name;
    public string Surname;
    public string IDCardNumber;
    public string PoliceStampPath;
    public bool NeedSecondStamp;
    public string PoliceStamp2Path;

    public GraphData_PoliceDocument(PoliceDocument doc)
    {
        Name = doc.Name;
        Surname = doc.Surname;
        IDCardNumber = doc.IDCardNumber;
        PoliceStampPath = AssetDatabase.GetAssetPath(doc.PoliceStamp);
        NeedSecondStamp = doc.NeedSecondStamp;
        PoliceStamp2Path = AssetDatabase.GetAssetPath(doc.PoliceStamp2);
    }
}