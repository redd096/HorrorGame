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
        TextField nameTextField = CreateElementsUtilities.CreateTextField("Name", Name, x => Name = x.newValue);
        TextField surnameTextField = CreateElementsUtilities.CreateTextField("Surname", Surname, x => Surname = x.newValue);

        //validate date
        Foldout validateDateFoldout = CreateElementsUtilities.CreateFoldout("Date");
        IntegerField day = CreateElementsUtilities.CreateIntegerField("Day", ValidateDate.Day, x => ValidateDate.Day = x.newValue);
        IntegerField month = CreateElementsUtilities.CreateIntegerField("Month", ValidateDate.Month, x => ValidateDate.Month = x.newValue);
        IntegerField year = CreateElementsUtilities.CreateIntegerField("Year", ValidateDate.Year, x => ValidateDate.Year = x.newValue);
        validateDateFoldout.Add(day);
        validateDateFoldout.Add(month);
        validateDateFoldout.Add(year);

        //signature
        TextField signatureTextField = CreateElementsUtilities.CreateTextField("Signature", Signature, x => Signature = x.newValue);

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
#endif
}