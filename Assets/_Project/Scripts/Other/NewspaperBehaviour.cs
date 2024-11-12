using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached to newspaper prefab, to set image in game
/// </summary>
public class NewspaperBehaviour : MonoBehaviour
{
    [SerializeField] Image image;

    /// <summary>
    /// If a resident was killed, set the sprite inside the newspaper
    /// </summary>
    /// <param name="killedResident"></param>
    public void Init(ResidentData killedResident)
    {
        if (image && killedResident)
            image.sprite = killedResident.Photo;
    }
}
