#if UNITY_EDITOR
using UnityEngine.UIElements;
using redd096.NodesGraph.Editor;
#endif

/// <summary>
/// Document for customers
/// </summary>
[System.Serializable]
public class ResidentCard
{
    public string Name;
    public string Surname;
    public string IDCardNumber;
    public FRoom RoomNumber;
    public string Signature;

    public ResidentCard Clone()
    {
        return new ResidentCard()
        {
            Name = Name,
            Surname = Surname,
            IDCardNumber = IDCardNumber,
            RoomNumber = RoomNumber,
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

        //room number
        Foldout roomNumberFoldout = CreateElementsUtilities.CreateFoldout("Room Number");
        IntegerField floor = CreateElementsUtilities.CreateIntegerField("Floor", RoomNumber.Floor, x => RoomNumber.Floor = x.newValue);
        IntegerField room = CreateElementsUtilities.CreateIntegerField("Room", RoomNumber.Room, x => RoomNumber.Room = x.newValue);
        roomNumberFoldout.Add(floor);
        roomNumberFoldout.Add(room);

        //signature
        TextField signatureTextField = CreateElementsUtilities.CreateTextField("Signature", Signature, x => Signature = x.newValue);

        //add to container
        container.Add(nameTextField);
        container.Add(surnameTextField);
        container.Add(cardNumberTextField);
        container.Add(roomNumberFoldout);
        container.Add(signatureTextField);
    }
#endif
}