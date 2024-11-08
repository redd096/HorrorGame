using System.Collections;
using UnityEngine;

/// <summary>
/// When stamp on the right is dragged, do animations
/// </summary>
[RequireComponent(typeof(StampDraggableBase))]
public class StampRightFeedback : StampLeftFeedback
{
    [Space] 
    [SerializeField] private GameObject stampToPutOnDocument;
    
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
        stamp.onStamp += OnStamp;

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
            stamp.onStamp -= OnStamp;

            //remove events also for stamp on the left screen
            if (left)
            {
                stamp.onBeginDrag += left.OnBeginDrag;
                stamp.onEndDrag += left.OnEndDrag;
            }
        }
    }

    private void OnStamp(Vector2 position, DocumentDraggable hit)
    {
        //if (hit.CanReceiveStamp)
            StartCoroutine(OnStampCoroutine(position, hit.transform));
    }

    private IEnumerator OnStampCoroutine(Vector2 position, Transform stampParent)
    {
        //wait few seconds to move stamp down
        yield return new WaitForSeconds(0.1f);
        
        //put stamp on the document
        GameObject go = Instantiate(stampToPutOnDocument, stampParent);
        go.transform.position = position;
    }
}
