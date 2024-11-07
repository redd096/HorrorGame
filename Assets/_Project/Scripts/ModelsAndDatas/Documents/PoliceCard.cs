using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.UIElements;
using redd096.NodesGraph.Editor;
#endif

/// <summary>
/// Document for customers
/// </summary>
[System.Serializable]
public class PoliceCard
{
    public string Name;
    public string Surname;
    public FDate ValidateDate;
    public string Signature;
    public bool HasFirstStamp;
    public bool HasSecondStamp;

    /// <summary>
    /// Check if this document is correct in game
    /// </summary>
    public bool IsCorrect(IDCard idCard, FDate currentDate, bool needSecondStamp, out string problem)
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
        if (currentDate.IsEqual(ValidateDate) == false)
        {
            problem = "This document isn't for today";
            return false;
        }
        if (idCard.Signature != Signature)
        {
            problem = "Wrong signature";
            return false;
        }
        if (HasFirstStamp == false)
        {
            problem = "The document is missing the stamp";
            return false;
        }
        if (needSecondStamp && HasSecondStamp == false)
        {
            problem = "The document is missing the second stamp";
            return false;
        }

        problem = null;
        return true;
    }

    public PoliceCard Clone()
    {
        return new PoliceCard()
        {
            Name = Name,
            Surname = Surname,
            ValidateDate = ValidateDate,
            Signature = Signature,
            HasFirstStamp = HasFirstStamp,
            HasSecondStamp = HasSecondStamp
        };
    }

#if UNITY_EDITOR
    public void CreateGraph(VisualElement container)
    {
        //name, surname
        TextField nameTextField = CreateElementsUtilities.CreateTextField("Name", Name, x => Name = x.newValue.Trim());
        TextField surnameTextField = CreateElementsUtilities.CreateTextField("Surname", Surname, x => Surname = x.newValue.Trim());

        //validate date
        Foldout validateDateFoldout = CreateElementsUtilities.CreateFoldout("Validate Date");
        ValidateDate.Min1();
        IntegerField day = CreateElementsUtilities.CreateIntegerField("Day", ValidateDate.Day, x => ValidateDate.Day = Min1(x.newValue));
        IntegerField month = CreateElementsUtilities.CreateIntegerField("Month", ValidateDate.Month, x => ValidateDate.Month = Min1(x.newValue));
        IntegerField year = CreateElementsUtilities.CreateIntegerField("Year", ValidateDate.Year, x => ValidateDate.Year = Min1(x.newValue));
        validateDateFoldout.Add(day);
        validateDateFoldout.Add(month);
        validateDateFoldout.Add(year);

        //signature
        TextField signatureTextField = CreateElementsUtilities.CreateTextField("Signature", Signature, x => Signature = x.newValue.Trim());

        //police stamps
        Toggle hasFirstStampToggle = CreateElementsUtilities.CreateToggle("Has first stamp", HasFirstStamp, x => HasFirstStamp = x.newValue);
        Toggle hasSecondStampToggle = CreateElementsUtilities.CreateToggle("Has second stamp", HasSecondStamp, x => HasSecondStamp = x.newValue);

        //add to container
        container.Add(nameTextField);
        container.Add(surnameTextField);
        container.Add(validateDateFoldout);
        container.Add(signatureTextField);
        container.Add(hasFirstStampToggle);
        container.Add(hasSecondStampToggle);
    }

    int Min1(int value)
    {
        return Mathf.Max(value, 1);
    }
#endif
}