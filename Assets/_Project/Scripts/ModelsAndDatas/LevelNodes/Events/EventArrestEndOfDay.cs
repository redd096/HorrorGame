using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Arrest at the end of the day, where user select a resident to arrest
/// </summary>
[System.Serializable]
public class EventArrestEndOfDay
{
    public List<Sprite> CustomerImage = new List<Sprite>();
    public string DialogueWhenCome;

    public EventArrestEndOfDay Clone()
    {
        return new EventArrestEndOfDay()
        {
            CustomerImage = CustomerImage,
            DialogueWhenCome = DialogueWhenCome
        };
    }
}