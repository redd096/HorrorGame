using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.UIElements;
using redd096.NodesGraph.Editor;
#endif

/// <summary>
/// Document for customers
/// </summary>
[System.Serializable]
public class AppointmentCard
{
    public string Name;
    public string Surname;
    public string Profession;
    public FDate AppointmentDate;
    public string AppointmentReason;
    public bool HasStamp;

    /// <summary>
    /// Check if this document is correct in game
    /// </summary>
    public bool IsCorrect(IDCard idCard, FDate currentDate, AppointmentData appointment, out string problem)
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
        if (appointment.Profession != Profession)               //already checked in AppointmentsManager
        {
            problem = "This appointment isn't scheduled";
            return false;
        }
        if (currentDate.IsEqual(AppointmentDate) == false)      //already checked in AppointmentsManager
        {
            problem = "This appointment isn't for today";
            return false;
        }
        if (appointment.AppointmentReason != AppointmentReason) //already checked in AppointmentsManager
        {
            problem = "This appointment isn't scheduled";
            return false;
        }
        if (HasStamp == false)
        {
            problem = "The document is missing the stamp";
            return false;
        }

        problem = null;
        return true;
    }

    public AppointmentCard Clone()
    {
        return new AppointmentCard()
        {
            Name = Name,
            Surname = Surname,
            Profession = Profession,
            AppointmentDate = AppointmentDate,
            AppointmentReason = AppointmentReason,
            HasStamp = HasStamp
        };
    }

#if UNITY_EDITOR
    public void CreateGraph(VisualElement container)
    {
        //name, surname and profession
        TextField nameTextField = CreateElementsUtilities.CreateTextField("Name", Name, x => Name = x.newValue.Trim());
        TextField surnameTextField = CreateElementsUtilities.CreateTextField("Surname", Surname, x => Surname = x.newValue.Trim());
        TextField professionTextField = CreateElementsUtilities.CreateTextField("Profession", Profession, x => Profession = x.newValue.Trim());

        //appointment date
        Foldout appointmentDateFoldout = CreateElementsUtilities.CreateFoldout("Appointment Date");
        AppointmentDate.Min1();
        IntegerField day = CreateElementsUtilities.CreateIntegerField("Day", AppointmentDate.Day, x => AppointmentDate.Day = Min1(x.newValue));
        IntegerField month = CreateElementsUtilities.CreateIntegerField("Month", AppointmentDate.Month, x => AppointmentDate.Month = Min1(x.newValue));
        IntegerField year = CreateElementsUtilities.CreateIntegerField("Year", AppointmentDate.Year, x => AppointmentDate.Year = Min1(x.newValue));
        appointmentDateFoldout.Add(day);
        appointmentDateFoldout.Add(month);
        appointmentDateFoldout.Add(year);

        //reason and stamp
        TextField reasonTextField = CreateElementsUtilities.CreateTextField("Appointment Reason", AppointmentReason, x => AppointmentReason = x.newValue.Trim());
        Toggle hasStampToggle = CreateElementsUtilities.CreateToggle("Has Stamp", HasStamp, x => HasStamp = x.newValue);

        //add to container
        container.Add(nameTextField);
        container.Add(surnameTextField);
        container.Add(professionTextField);
        container.Add(appointmentDateFoldout);
        container.Add(reasonTextField);
        container.Add(hasStampToggle);
    }

    int Min1(int value)
    {
        return Mathf.Max(value, 1);
    }

#endif
}