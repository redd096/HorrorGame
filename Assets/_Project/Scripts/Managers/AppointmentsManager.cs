using UnityEngine;

/// <summary>
/// Keep track of current day date and appointments
/// </summary>
public class AppointmentsManager : MonoBehaviour
{
    [SerializeField] AppointmentData[] appointments;

    public AppointmentData[] Appointments => appointments;

    public bool IsCorrectAppointment(AppointmentCard doc, FDate currentDate, out AppointmentData appointment, out string problem)
    {
        appointment = null;

        //check date
        if (doc.AppointmentDate.IsEqual(currentDate) == false)
        {
            problem = "This appointment isn't for today";
            return false;
        }

        //check every appointment
        foreach (var v in appointments)
        {
            //if this document's values are the same, return true
            if (doc.Name == v.Name
                && doc.Surname == v.Surname
                && doc.AppointmentReason == v.AppointmentReason
                && doc.Profession == v.Profession)
            {
                appointment = v;
                problem = null;
                return true;
            }
        }

        appointment = null;
        problem = "This appointment isn't scheduled";
        return false;
    }
}
