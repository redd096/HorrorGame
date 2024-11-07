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

    /// <summary>
    /// Check if this document is correct in game
    /// </summary>
    public bool IsCorrect(IDCard idCard, ResidentData resident, out string problem)
    {
        if (idCard.Name != Name || resident.Name != Name)                                   //could check only one, because resident and idCard are already checked in ResidentsManager
        {
            problem = "Wrong name";
            return false;
        }
        if (idCard.Surname != Surname || resident.Surname != Surname)                       //could check only one, because resident and idCard are already checked in ResidentsManager
        {
            problem = "Wrong surname";
            return false;
        }
        if (idCard.CardNumber != IDCardNumber || resident.IDCardNumber != IDCardNumber)     //could check only one, because resident and idCard are already checked in ResidentsManager
        {
            problem = "Wrong ID card number";
            return false;
        }
        if (resident.RoomNumber.IsEqual(RoomNumber) == false)
        {
            problem = "Wrong room number";
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
        TextField nameTextField = CreateElementsUtilities.CreateTextField("Name", Name, x => Name = x.newValue.Trim());
        TextField surnameTextField = CreateElementsUtilities.CreateTextField("Surname", Surname, x => Surname = x.newValue.Trim());
        TextField cardNumberTextField = CreateElementsUtilities.CreateTextField("ID Card Number", IDCardNumber, x => IDCardNumber = x.newValue.Trim());

        //room number
        Foldout roomNumberFoldout = CreateElementsUtilities.CreateFoldout("Room Number");
        IntegerField floor = CreateElementsUtilities.CreateIntegerField("Floor", RoomNumber.Floor, x => RoomNumber.Floor = x.newValue);
        IntegerField room = CreateElementsUtilities.CreateIntegerField("Room", RoomNumber.Room, x => RoomNumber.Room = x.newValue);
        roomNumberFoldout.Add(floor);
        roomNumberFoldout.Add(room);

        //signature
        TextField signatureTextField = CreateElementsUtilities.CreateTextField("Signature", Signature, x => Signature = x.newValue.Trim());

        //add to container
        container.Add(nameTextField);
        container.Add(surnameTextField);
        container.Add(cardNumberTextField);
        container.Add(roomNumberFoldout);
        container.Add(signatureTextField);
    }
#endif
}