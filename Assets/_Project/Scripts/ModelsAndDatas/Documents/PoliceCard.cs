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
public class PoliceCard
{
    public string Name;
    public string Surname;
    public string IDCardNumber;
    public Sprite PoliceStamp;
    public bool NeedSecondStamp;
    public Sprite PoliceStamp2;

#if UNITY_EDITOR
    public void CreateGraph(VisualElement container)
    {
        //name, surname and cardNumber
        TextField nameTextField = CreateElementsUtilities.CreateTextField("Name", Name, x => Name = x.newValue);
        TextField surnameTextField = CreateElementsUtilities.CreateTextField("Surname", Surname, x => Surname = x.newValue);
        TextField cardNumberTextField = CreateElementsUtilities.CreateTextField("ID Card Number", IDCardNumber, x => IDCardNumber = x.newValue);

        //police stamp, toggle, and police stamp 2
        ObjectField stampObjectField = CreateElementsUtilities.CreateObjectFieldWithPreview("Police Stamp", PoliceStamp, Vector2.one * 100, out Image stampImage, x => PoliceStamp = x.newValue as Sprite);
        Toggle needSecondStampToggle = CreateElementsUtilities.CreateToggle("Need second stamp", NeedSecondStamp, x => NeedSecondStamp = x.newValue);
        ObjectField stamp2ObjectField = CreateElementsUtilities.CreateObjectFieldWithPreview("Police Stamp 2", PoliceStamp2, Vector2.one * 100, out Image stamp2Image, x => PoliceStamp2 = x.newValue as Sprite);

        //add to container
        container.Add(nameTextField);
        container.Add(surnameTextField);
        container.Add(cardNumberTextField);
        container.Add(stampObjectField);
        container.Add(stampImage);
        container.Add(needSecondStampToggle);
        container.Add(stamp2ObjectField);
        container.Add(stamp2Image);
    }
#endif
}