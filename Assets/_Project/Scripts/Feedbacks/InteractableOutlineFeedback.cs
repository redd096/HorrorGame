using UnityEngine;
using UnityEngine.UI;
using PrimeTween;

/// <summary>
/// If this interactable has Custom_SpriteOutline material, show pulsing outline when interactable and hide when not interactable
/// </summary>
[RequireComponent(typeof(InteractableBase))]
public class InteractableOutlineFeedback : MonoBehaviour
{
    [SerializeField] Image image;
    float pulseSpeed = 2f;

    InteractableBase interactableObj;
    Material mat;
    Tween tween;

    private void Awake()
    {
        interactableObj = GetComponent<InteractableBase>();

        //set image instance material, to not override the material in the project
        mat = new Material(image.material);
        image.material = mat;

        //add events
        interactableObj.onSetInteractable += OnSetInteractable;
    }

    private void OnDestroy()
    {
        //remove events
        if (interactableObj != null)
            interactableObj.onSetInteractable -= OnSetInteractable;
    }

    private void OnSetInteractable(bool isInteractable)
    {
        //stop tween
        tween.Stop();
        Color c = mat.GetColor("_OutlineColor");
        float startValue = c.a;

        //change outline alpha
        float endValue = isInteractable ? 1 : 0;  //enabled or disabled
        tween = Tween.Custom(startValue, endValue, pulseSpeed, onValueChange: x =>
        {
            c.a = x;
            mat.SetColor("_OutlineColor", c);
        }).OnComplete(() =>
        {
            //start pulse effect
            if (isInteractable)
            {
                tween = Tween.Custom(1f, 0f, pulseSpeed, ease: Ease.InOutQuad, cycles: -1, cycleMode: CycleMode.Yoyo, onValueChange: x =>
                {
                    c.a = x;
                    mat.SetColor("_OutlineColor", c);
                });
            }
        });
    }
}
