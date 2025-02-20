using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This manages UI like DeskManager, but only for some windows like box where put documents or box to put back interactables. 
/// This is managed by DeskManager
/// </summary>
public class DeskWindowsManager : MonoBehaviour
{
    [SerializeField] private RectTransform giveDocumentsArea;
    [SerializeField] private RectTransform putBackInteractablesArea;
    [SerializeField] private RectTransform boardArea;
    
    [Header("Animation")]
    [SerializeField] private Image giveDocumentsImage;
    [SerializeField] private TMP_Text giveDocumentsText;
    [SerializeField] private Image putBackInteractablesImage;
    [SerializeField] private TMP_Text putBackInteractablesText;
    [SerializeField] private float animationDuration = 1f;

    private bool isDocumentAreaActive;
    private bool isInteractablesAreaActive;

    public RectTransform BoardArea => boardArea;

    private void Awake()
    {
        //hide by default
        giveDocumentsImage.color = new Color(giveDocumentsImage.color.r, giveDocumentsImage.color.g, giveDocumentsImage.color.b, 0f);
        giveDocumentsText.color = new Color(giveDocumentsText.color.r, giveDocumentsText.color.g, giveDocumentsText.color.b, 0f);
        putBackInteractablesImage.color = new Color(putBackInteractablesImage.color.r, putBackInteractablesImage.color.g, putBackInteractablesImage.color.b, 0f);
        putBackInteractablesText.color = new Color(putBackInteractablesText.color.r, putBackInteractablesText.color.g, putBackInteractablesText.color.b, 0f); 
    }

    #region public API

    /// <summary>
    /// Show or hide area to give back documents
    /// </summary>
    /// <param name="show"></param>
    public void ShowDocumentsArea(bool show)
    {
        if (isDocumentAreaActive != show)
        {
            isDocumentAreaActive = show;
            ShowArea(show, giveDocumentsImage, giveDocumentsText);
        }
    }

    /// <summary>
    /// Show or hide area to put back interactables
    /// </summary>
    /// <param name="show"></param>
    public void ShowInteractablesArea(bool show)
    {
        if (isInteractablesAreaActive != show)
        {
            isInteractablesAreaActive = show;
            ShowArea(show, putBackInteractablesImage, putBackInteractablesText);
        }
    }
    
    /// <summary>
    /// Check if this rect is inside documents area
    /// </summary>
    /// <returns></returns>
    public bool CheckIsInGiveDocumentsArea(Vector2 point)
    {
        return CheckIsInArea(point, giveDocumentsArea, isDocumentAreaActive, canGoOutsideUp: true, canGoOutisdeDown: false);
    }
    
    /// <summary>
    /// Check if this rect is inside interactables area
    /// </summary>
    /// <returns></returns>
    public bool CheckIsInPutBackInteractablesArea(Vector2 point)
    {
        return CheckIsInArea(point, putBackInteractablesArea, isInteractablesAreaActive, canGoOutsideUp: false, canGoOutisdeDown: true);
    }

    /// <summary>
    /// Check if this rect is inside board area
    /// </summary>
    /// <param name="rectTr"></param>
    /// <returns></returns>
    public bool CheckIsInBoardArea(RectTransform rectTr)
    {
        return CheckIsCompletelyInArea(rectTr, boardArea, true);
    }
    
    #endregion
    
    #region private API

    private void ShowArea(bool show, Image areaImage, TMP_Text areaText)
    {
        //stop current animations
        Tween.StopAll(areaImage);
        Tween.StopAll(areaText);

        //start blink animation
        if (show)
        {
            Tween.Alpha(areaImage, 1, animationDuration).OnComplete(() =>
                Tween.Alpha(areaImage, 1f, 0f, animationDuration, Ease.InOutQuad, -1, CycleMode.Yoyo));
            
            Tween.Alpha(areaText, 1f, animationDuration).OnComplete(() => 
                Tween.Alpha(areaText, 1f, 0f, animationDuration, Ease.InOutQuad, -1, CycleMode.Yoyo));
        }
        //or fade out
        else
        {
            Tween.Alpha(areaImage, 0, animationDuration);
            Tween.Alpha(areaText, 0f, animationDuration);
        }
    }

    private bool CheckIsInArea(Vector2 point, RectTransform area, bool isAreaActive, bool canGoOutsideUp, bool canGoOutisdeDown)
    {
        //if area isn't active, return false
        if (isAreaActive == false)
            return false;
        
        //else, check if point is in area
        if (RectTransformUtility.RectangleContainsScreenPoint(area, point))
            return true;

        //check if can go outisde up or down
        Vector2 yCenteredPoint = new Vector2(point.x, area.position.y);
        if (RectTransformUtility.RectangleContainsScreenPoint(area, yCenteredPoint))
        {
            if (canGoOutsideUp && point.y > yCenteredPoint.y)
                return true;
            else if (canGoOutisdeDown && point.y < yCenteredPoint.y)
                return true;
        }

        return false;
    }

    private bool CheckIsCompletelyInArea(RectTransform rectTr, RectTransform area, bool isAreaActive)
    {
        //if area isn't active, return false
        if (isAreaActive == false)
            return false;

        //else, check if every point is in area
        Vector3[] points = new Vector3[4];
        rectTr.GetWorldCorners(points);
        foreach (var point in points)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(area, point) == false)
                return false;
        }

        return true;
    }

    #endregion
}
