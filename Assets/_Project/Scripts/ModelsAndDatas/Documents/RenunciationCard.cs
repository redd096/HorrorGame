using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.UIElements;
using redd096.NodesGraph.Editor;
using UnityEditor.UIElements;
#endif

/// <summary>
/// Document for customers
/// </summary>
[System.Serializable]
public class RenunciationCard
{
    public string Name;
    public string Surname;
    public string IDCardNumber;
    public FDate DateBirth;
    public Sprite Signature;

#if UNITY_EDITOR
    public void CreateGraph(VisualElement container)
    {
        //name, surname and cardNumber
        TextField nameTextField = CreateElementsUtilities.CreateTextField("Name", Name, x => Name = x.newValue);
        TextField surnameTextField = CreateElementsUtilities.CreateTextField("Surname", Surname, x => Surname = x.newValue);
        TextField cardNumberTextField = CreateElementsUtilities.CreateTextField("ID Card Number", IDCardNumber, x => IDCardNumber = x.newValue);

        //date birth
        Foldout dateBirthFoldout = CreateElementsUtilities.CreateFoldout("Date Birth");
        IntegerField day = CreateElementsUtilities.CreateIntegerField("Day", DateBirth.Day, x => DateBirth.Day = x.newValue);
        IntegerField month = CreateElementsUtilities.CreateIntegerField("Month", DateBirth.Month, x => DateBirth.Month = x.newValue);
        IntegerField year = CreateElementsUtilities.CreateIntegerField("Year", DateBirth.Year, x => DateBirth.Year = x.newValue);
        dateBirthFoldout.Add(day);
        dateBirthFoldout.Add(month);
        dateBirthFoldout.Add(year);

        //signature
        ObjectField signatureObjectField = CreateElementsUtilities.CreateObjectFieldWithPreview("Signature", Signature, Vector2.one * 100, out Image signatureImage, x => Signature = x.newValue as Sprite);

        //add to container
        container.Add(nameTextField);
        container.Add(surnameTextField);
        container.Add(cardNumberTextField);
        container.Add(dateBirthFoldout);
        container.Add(signatureObjectField);
        container.Add(signatureImage);
    }
#endif
}