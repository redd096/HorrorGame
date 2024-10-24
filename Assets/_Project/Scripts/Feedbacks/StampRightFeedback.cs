using UnityEngine;

/// <summary>
/// When stamp on the right is dragged, do animations
/// </summary>
[RequireComponent(typeof(StampDraggableBase))]
public class StampRightFeedback : StampLeftFeedback
{
    private StampDraggableBase stamp;
    private StampLeftFeedback left;

    protected override void Start()
    {
        base.Start();

        //save refs
        stamp = GetComponent<StampDraggableBase>();

        //add events
        stamp.onBeginDrag += OnBeginDrag;
        stamp.onEndDrag += OnEndDrag;

        //add events also for stamp on the left screen
        left = stamp.CopyInScene.GetComponent<StampLeftFeedback>();
        stamp.onBeginDrag += left.OnBeginDrag;
        stamp.onEndDrag += left.OnEndDrag;
    }

    private void OnDestroy()
    {
        //remove events
        if (stamp)
        {
            stamp.onBeginDrag -= OnBeginDrag;
            stamp.onEndDrag -= OnEndDrag;

            //remove events also for stamp on the left screen
            if (left)
            {
                stamp.onBeginDrag += left.OnBeginDrag;
                stamp.onEndDrag += left.OnEndDrag;
            }
        }
    }
}
