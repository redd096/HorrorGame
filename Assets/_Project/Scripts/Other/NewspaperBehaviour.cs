using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached to newspaper prefab, to set image in game
/// </summary>
public class NewspaperBehaviour : MonoBehaviour
{
    [SerializeField] Image[] images;

    /// <summary>
    /// If a resident was killed, set the sprite inside the newspaper
    /// </summary>
    /// <param name="killedResidents"></param>
    public void Init(ResidentData[] killedResidents)
    {
        for (int i = 0; i < images.Length; i++)
        {
            //if there are more images than residents, hide image
            bool isOk = killedResidents.Length > i;
            images[i].gameObject.SetActive(isOk);
            
            //set resident photo
            if (isOk)
                images[i].sprite = killedResidents[i].Photo;
        }
    }
}
