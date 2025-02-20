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
    public FDate BirthDate;
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
            BirthDate = BirthDate,
            ExpireDate = ExpireDate,
            Signature = Signature,
            Photo = Photo
        };
    }

#if UNITY_EDITOR
    public void CreateGraph(VisualElement container)
    {
        //name, surname and cardNumber
        TextField nameTextField = CreateElementsUtilities.CreateTextField("Name", Name, x => Name = x.newValue.Trim());
        TextField surnameTextField = CreateElementsUtilities.CreateTextField("Surname", Surname, x => Surname = x.newValue.Trim());
        TextField cardNumberTextField = CreateElementsUtilities.CreateTextField("Card Number", CardNumber, x => CardNumber = x.newValue.Trim());

        //birth date
        Foldout birthDateFoldout = CreateElementsUtilities.CreateFoldout("Birth Date");
        BirthDate.Min1();
        IntegerField day = CreateElementsUtilities.CreateIntegerField("Day", BirthDate.Day, x => BirthDate.Day = Min1(x.newValue));
        IntegerField month = CreateElementsUtilities.CreateIntegerField("Month", BirthDate.Month, x => BirthDate.Month = Min1(x.newValue));
        IntegerField year = CreateElementsUtilities.CreateIntegerField("Year", BirthDate.Year, x => BirthDate.Year = Min1(x.newValue));
        birthDateFoldout.Add(day);
        birthDateFoldout.Add(month);
        birthDateFoldout.Add(year);

        //expire date
        Foldout expireDateFoldout = CreateElementsUtilities.CreateFoldout("Expire Date");
        ExpireDate.Min1();
        IntegerField dayExpire = CreateElementsUtilities.CreateIntegerField("Day", ExpireDate.Day, x => ExpireDate.Day = Min1(x.newValue));
        IntegerField monthExpire = CreateElementsUtilities.CreateIntegerField("Month", ExpireDate.Month, x => ExpireDate.Month = Min1(x.newValue));
        IntegerField yearExpire = CreateElementsUtilities.CreateIntegerField("Year", ExpireDate.Year, x => ExpireDate.Year = Min1(x.newValue));
        expireDateFoldout.Add(dayExpire);
        expireDateFoldout.Add(monthExpire);
        expireDateFoldout.Add(yearExpire);

        //signature and photo
        TextField signatureTextField = CreateElementsUtilities.CreateTextField("Signature", Signature, x => Signature = x.newValue.Trim());
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

    int Min1(int value)
    {
        return Mathf.Max(value, 1);
    }

#endif
}