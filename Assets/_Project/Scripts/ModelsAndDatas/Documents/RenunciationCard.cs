using UnityEngine;
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
    public FDate BirthDate;
    public int DurationStayInDays;
    public int CorrectDurationStayInDays;
    public string Signature;

    /// <summary>
    /// Check if this document is correct in game
    /// </summary>
    public bool IsCorrect(IDCard idCard, out string problem)
    {
        if (idCard.Name != Name)
        {
            problem = "Wrong name";
            return false;
        }
        if (idCard.Surname != Surname)
        {
            problem = "Wrong surname";
            return false;
        }
        if (idCard.CardNumber != IDCardNumber)
        {
            problem = "Wrong ID card number";
            return false;
        }
        if (idCard.BirthDate.IsEqual(BirthDate) == false)
        {
            problem = "Wrong date of birth";
            return false;
        }
        if (DurationStayInDays != CorrectDurationStayInDays)
        {
            problem = "Wrong duration stay in days";
            return false;
        }
        if (idCard.Signature != Signature)
        {
            problem = "Wrong signature";
            return false;
        }

        problem = null;
        return true;
    }

    public RenunciationCard Clone()
    {
        return new RenunciationCard()
        {
            Name = Name,
            Surname = Surname,
            IDCardNumber = IDCardNumber,
            BirthDate = BirthDate,
            DurationStayInDays = DurationStayInDays,
            CorrectDurationStayInDays = CorrectDurationStayInDays,
            Signature = Signature
        };
    }

#if UNITY_EDITOR
    public void CreateGraph(VisualElement container)
    {
        //name, surname and cardNumber
        TextField nameTextField = CreateElementsUtilities.CreateTextField("Name", Name, x => Name = x.newValue.Trim());
        TextField surnameTextField = CreateElementsUtilities.CreateTextField("Surname", Surname, x => Surname = x.newValue.Trim());
        TextField cardNumberTextField = CreateElementsUtilities.CreateTextField("ID Card Number", IDCardNumber, x => IDCardNumber = x.newValue.Trim());

        //birth date
        Foldout birthDateFoldout = CreateElementsUtilities.CreateFoldout("Birth Date");
        BirthDate.Min1();
        IntegerField day = CreateElementsUtilities.CreateIntegerField("Day", BirthDate.Day, x => BirthDate.Day = Min1(x.newValue));
        IntegerField month = CreateElementsUtilities.CreateIntegerField("Month", BirthDate.Month, x => BirthDate.Month = Min1(x.newValue));
        IntegerField year = CreateElementsUtilities.CreateIntegerField("Year", BirthDate.Year, x => BirthDate.Year = Min1(x.newValue));
        birthDateFoldout.Add(day);
        birthDateFoldout.Add(month);
        birthDateFoldout.Add(year);

        //duration stay
        IntegerField durationStayIntegerField = CreateElementsUtilities.CreateIntegerField("Duration of Stay (written on document)", DurationStayInDays, x => DurationStayInDays = x.newValue);
        IntegerField correctTurationStayIntegerField = CreateElementsUtilities.CreateIntegerField("Duration of Stay (correct for player)", CorrectDurationStayInDays, x => CorrectDurationStayInDays = x.newValue);

        //signature
        TextField signatureTextField = CreateElementsUtilities.CreateTextField("Signature", Signature, x => Signature = x.newValue.Trim());

        //add to container
        container.Add(nameTextField);
        container.Add(surnameTextField);
        container.Add(cardNumberTextField);
        container.Add(birthDateFoldout);
        container.Add(durationStayIntegerField);
        container.Add(correctTurationStayIntegerField);
        container.Add(signatureTextField);
    }

    int Min1(int value)
    {
        return Mathf.Max(value, 1);
    }
#endif
}