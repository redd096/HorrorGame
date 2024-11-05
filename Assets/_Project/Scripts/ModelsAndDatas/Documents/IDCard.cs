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
public class IDCard
{
    public string Name;
    public string Surname;
    public string CardNumber;
    public FDate DateBirth;
    public FDate ExpireDate;
    public string Signature;
    public Sprite Photo;

    public IDCard Clone()
    {
        return new IDCard()
        {
            Name = Name,
            Surname = Surname,
            CardNumber = CardNumber,
            DateBirth = DateBirth,
            ExpireDate = ExpireDate,
            Signature = Signature,
            Photo = Photo
        };
    }

#if UNITY_EDITOR
    public void CreateGraph(VisualElement container)
    {
        //name, surname and cardNumber
        TextField nameTextField = CreateElementsUtilities.CreateTextField("Name", Name, x => Name = x.newValue);
        TextField surnameTextField = CreateElementsUtilities.CreateTextField("Surname", Surname, x => Surname = x.newValue);
        TextField cardNumberTextField = CreateElementsUtilities.CreateTextField("Card Number", CardNumber, x => CardNumber = x.newValue);

        //birth date
        Foldout birthDateFoldout = CreateElementsUtilities.CreateFoldout("Birth Date");
        IntegerField day = CreateElementsUtilities.CreateIntegerField("Day", DateBirth.Day, x => DateBirth.Day = x.newValue);
        IntegerField month = CreateElementsUtilities.CreateIntegerField("Month", DateBirth.Month, x => DateBirth.Month = x.newValue);
        IntegerField year = CreateElementsUtilities.CreateIntegerField("Year", DateBirth.Year, x => DateBirth.Year = x.newValue);
        birthDateFoldout.Add(day);
        birthDateFoldout.Add(month);
        birthDateFoldout.Add(year);

        //expire date
        Foldout expireDateFoldout = CreateElementsUtilities.CreateFoldout("Expire Date");
        IntegerField dayExpire = CreateElementsUtilities.CreateIntegerField("Day", ExpireDate.Day, x => ExpireDate.Day = x.newValue);
        IntegerField monthExpire = CreateElementsUtilities.CreateIntegerField("Month", ExpireDate.Month, x => ExpireDate.Month = x.newValue);
        IntegerField yearExpire = CreateElementsUtilities.CreateIntegerField("Year", ExpireDate.Year, x => ExpireDate.Year = x.newValue);
        expireDateFoldout.Add(dayExpire);
        expireDateFoldout.Add(monthExpire);
        expireDateFoldout.Add(yearExpire);

        //signature and photo
        TextField signatureTextField = CreateElementsUtilities.CreateTextField("Signature", Signature, x => Signature = x.newValue);
        ObjectField photoObjectField = CreateElementsUtilities.CreateObjectFieldWithPreview("Photo", Photo, Vector2.one * 100, out Image photoImage, x => Photo = x.newValue as Sprite);

        //add to container
        container.Add(nameTextField);
        container.Add(surnameTextField);
        container.Add(cardNumberTextField);
        container.Add(birthDateFoldout);
        container.Add(expireDateFoldout);
        container.Add(signatureTextField);
        container.Add(photoObjectField);
        container.Add(photoImage);
    }
#endif
}