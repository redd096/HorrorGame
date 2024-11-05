#if UNITY_EDITOR
using UnityEngine.UIElements;
using redd096.NodesGraph.Editor;
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
    public int DurationStayInDays;
    public string Signature;

    public RenunciationCard Clone()
    {
        return new RenunciationCard()
        {
            Name = Name,
            Surname = Surname,
            IDCardNumber = IDCardNumber,
            DateBirth = DateBirth,
            DurationStayInDays = DurationStayInDays,
            Signature = Signature
        };
    }

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

        //duration stay
        IntegerField durationStayIntegerField = CreateElementsUtilities.CreateIntegerField("Duration of Stay", DurationStayInDays, x => DurationStayInDays = x.newValue);

        //signature
        TextField signatureTextField = CreateElementsUtilities.CreateTextField("Signature", Signature, x => Signature = x.newValue);

        //add to container
        container.Add(nameTextField);
        container.Add(surnameTextField);
        container.Add(cardNumberTextField);
        container.Add(dateBirthFoldout);
        container.Add(durationStayIntegerField);
        container.Add(signatureTextField);
    }
#endif
}