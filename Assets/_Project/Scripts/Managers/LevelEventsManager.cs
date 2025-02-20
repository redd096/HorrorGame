using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using UnityEngine.UI;

/// <summary>
/// This is used by LevelManager and is specific for the events (not customers or other)
/// </summary>
public class LevelEventsManager : MonoBehaviour
{
    [Header("Newspaper event")]
    [SerializeField] CanvasGroup newspapersContainer;
    [SerializeField] float durationNewspaper = 3;

    [Header("Background events")]
    [SerializeField] Animator backgroundAnimator;
    [SerializeField] AnimationClip backgroundIdle;
    [SerializeField] AnimationClip backgroundRed;

    [Header("Blood events")]
    [SerializeField] RectTransform waveRect;
    [SerializeField] float waveAnimationDuration = 5;
    [SerializeField] float delayAfterWaveAnimation = 3;

    [Header("Arrest at the end of the day")] 
    [SerializeField] InteractableDragFromTheRight journalInteractable;
    [SerializeField] JournalManager journalManagerForArrest;

    [Header("Camera Flash")]
    [SerializeField] Animator cameraFlashAnimator;
    [SerializeField] Image cameraFlashImage;
    [SerializeField] AnimationClip cameraFlashAnimation;
    [SerializeField] float delayAfterFlashAnimation = 2;

    //when start red event, find every object with this script and save in array
    private RedEventFeedback[] redEventsPlaying;

    /// <summary>
    /// Show for few seconds the newspaper
    /// </summary>
    /// <param name="newspaperPrefab"></param>
    /// <param name="residentsKilledThisDay"></param>
    public Sequence ShowNewspaper(NewspaperBehaviour newspaperPrefab, List<ResidentData> residentsKilledThisDay)
    {
        //instantiate newspaper
        if (newspaperPrefab == null)
        {
            Debug.LogError("Newspaper is null", gameObject);
            return default;
        }

        //instantiate newspaper and immediately fade in
        NewspaperBehaviour newspaper = Instantiate(newspaperPrefab, newspapersContainer.transform);
        newspaper.Init(residentsKilledThisDay.ToArray());
        newspapersContainer.gameObject.SetActive(true);
        newspapersContainer.alpha = 1;

        //then wait few seconds
        Sequence sequence = Sequence.Create();
        sequence.ChainDelay(durationNewspaper);

        //fade out
        sequence.Chain(Tween.Alpha(newspapersContainer, 0, duration: 1f));
        sequence.ChainCallback(() =>
        {
            //hide container and newspaper
            newspapersContainer.gameObject.SetActive(false);
            Destroy(newspaper.gameObject);
        });

        return sequence;
    }

    /// <summary>
    /// Play an animation on background image
    /// </summary>
    /// <param name="animationClip"></param>
    public void PlayBackgroundEvent(AnimationClip animationClip)
    {
        backgroundAnimator.Play(animationClip.name);
    }

    /// <summary>
    /// Back to normal background image (idle or red, depend on RedEvent is still playing)
    /// </summary>
    public void StopBackgroundEvent()
    {
        if (redEventsPlaying == null)
            backgroundAnimator.Play(backgroundIdle.name);
        else
            backgroundAnimator.Play(backgroundRed.name);
    }

    /// <summary>
    /// Change everything to Red
    /// </summary>
    /// <param name="play">play or stop</param>
    public void RedEvent(bool play)
    {
        if (play)
        {
            //find feedbacks and start Red event
            redEventsPlaying = FindObjectsOfType<RedEventFeedback>();
            foreach (var v in redEventsPlaying)
                v.StartRedEvent();

            //also background
            backgroundAnimator.Play(backgroundRed.name);
        }
        else
        {
            //previous Red events, back to normal
            if (redEventsPlaying != null)
            {
                foreach (var v in redEventsPlaying)
                    v.StopRedEvent();
                redEventsPlaying = null;
            }

            //also background
            backgroundAnimator.Play(backgroundIdle.name);
        }
    }

    /// <summary>
    /// Play blood event on backgroundImage and waveImage
    /// </summary>
    /// <param name="backgroundAnimation"></param>
    public Sequence PlayBloodEvent(AnimationClip backgroundAnimation)
    {
        PlayBackgroundEvent(backgroundAnimation);
        waveRect.sizeDelta = new Vector2(waveRect.sizeDelta.x, 0f);
        waveRect.gameObject.SetActive(true);

        Sequence sequence = Sequence.Create();

        //blood from 0 to canvas height
        Canvas canvas = waveRect.GetComponentInParent<Canvas>();
        sequence.Chain(Tween.UISizeDelta(waveRect, new Vector2(waveRect.sizeDelta.x, canvas.GetComponent<RectTransform>().sizeDelta.y), waveAnimationDuration, Ease.InOutSine));

        //delay and stop
        sequence.ChainDelay(delayAfterWaveAnimation);
        sequence.ChainCallback(() =>
        {
            StopBackgroundEvent();
            waveRect.sizeDelta = new Vector2(waveRect.sizeDelta.x, 0f);
            waveRect.gameObject.SetActive(false);
        });

        return sequence;
    }

    /// <summary>
    /// Show PoliceMan tell something to player, then make player choose which resident to arrest
    /// </summary>
    /// <param name="policeManImages"></param>
    /// <param name="policeManDialogue"></param>
    /// <param name="residentsManager"></param>
    /// <param name="onClickArrestResident"></param>
    /// <returns></returns>
    public IEnumerator PlayArrestAtTheEndOfDayEvent(List<Sprite> policeManImages, string policeManDialogue, ResidentsManager residentsManager, System.Action<ResidentData> onClickArrestResident)
    {
        //instantiate policeman
        CustomerBehaviour policeman = LevelUtilities.instance.InstantiateCustomer(new Customer() { CustomerImage = policeManImages });

        //move customer inside the screen and start dialogue
        yield return LevelUtilities.instance.MoveCustomer(policeman, enterInScene: true).ToYieldInstruction();
        yield return LevelUtilities.instance.WaitDialogue(policeManDialogue);
        
        //open journal board and show journal with residents to arrest 
        bool playerSelectedWhoArrest = false;
        journalInteractable.ForceOpen();
        journalInteractable.SetInteractable(false);
        journalManagerForArrest.SetResidentsForArrest(residentsManager);
        journalManagerForArrest.gameObject.SetActive(true);
        
        //wait until player select one resident to arrest
        journalManagerForArrest.OnClickResidentForArrest += OnClick_ArrestResident;
        yield return new WaitUntil(() => playerSelectedWhoArrest);
        
        //fade out
        yield return LevelUtilities.instance.FadeOut().ToYieldInstruction();

        //when player select on resident, call event from LevelManager to arrest it
        void OnClick_ArrestResident(ResidentData residentData)
        {
            journalManagerForArrest.OnClickResidentForArrest -= OnClick_ArrestResident;
            onClickArrestResident?.Invoke(residentData);
            playerSelectedWhoArrest = true;
        }
    }

    /// <summary>
    /// Do camera flash, then back to normal
    /// </summary>
    /// <returns></returns>
    public Sequence PlayCameraFlashEvent()
    {
        Sequence sequence = Sequence.Create();
        
        Color color = cameraFlashImage.color;
        color.a = 1f;
        cameraFlashImage.color = color;
        cameraFlashAnimator.gameObject.SetActive(true);

        //play animation and wait to complete
        sequence.ChainCallback(() => cameraFlashAnimator.Play(cameraFlashAnimation.name, -1, normalizedTime: 0f));
        sequence.ChainDelay(cameraFlashAnimation.length + delayAfterFlashAnimation);        //+ delay to show white screen for few seconds

        //alpha to 0
        sequence.Chain(Tween.Alpha(cameraFlashImage, 0f, duration: 1f));
        sequence.ChainCallback(() => cameraFlashAnimator.gameObject.SetActive(false));

        return sequence;
    }
}
