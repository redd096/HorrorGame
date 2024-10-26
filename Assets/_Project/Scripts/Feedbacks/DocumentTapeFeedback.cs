using UnityEngine;
using redd096.Attributes;
using System.Collections.Generic;

/// <summary>
/// When a document is inserted inside the board, show tape on it
/// </summary>
[RequireComponent(typeof(DocumentDraggable))]
public class DocumentTapeFeedback : MonoBehaviour
{
    [Header("How much tapes and where, when put this document inside board")]
    [MinMaxSlider(0, 9)][SerializeField] Vector2Int minMaxRandomNumberOfTapes = new Vector2Int(2, 4);
    [SerializeField] RectTransform tapePrefab;
    [GridSelectable(nameof(tapePossiblePositions), 3, 3, hideThisProperty = true)][SerializeField] byte b;   //this is used just to use the GridSelectable attribute
    [HideInInspector][SerializeField] Vector2Int[] tapePossiblePositions;

    DocumentDraggable docDraggable;
    RectTransform[] tapes;

    private void Awake()
    {
        //add events
        docDraggable = GetComponent<DocumentDraggable>();
        docDraggable.onBeginDrag += OnBeginDrag;
        docDraggable.onEnterBoard += OnEnterBoard;
    }

    private void OnDestroy()
    {
        //remove events
        if (docDraggable)
        {
            docDraggable.onBeginDrag -= OnBeginDrag;
            docDraggable.onEnterBoard -= OnEnterBoard;
        }
    }

    private void OnBeginDrag()
    {
        //when start drag document, remove tapes
        RemoveTapes();
    }

    private void OnEnterBoard()
    {
        //show tapes when enter in the board
        RemoveTapes();
        ShowRandomTapes();
    }

    void ShowRandomTapes()
    {
        //be sure isn't empty
        if (tapePossiblePositions == null || tapePossiblePositions.Length < 1)
            tapePossiblePositions = new Vector2Int[1] { Vector2Int.zero };

        //calculate random number of tapes
        int max = Mathf.Min(minMaxRandomNumberOfTapes.y + 1, tapePossiblePositions.Length); //be sure to not exceed possible positions length
        int min = Mathf.Min(minMaxRandomNumberOfTapes.x, max);                              //be sure to not exceed max
        int random = Random.Range(min, max);

        //instantiate tapes
        List<Vector2Int> possiblePositions = new List<Vector2Int>(tapePossiblePositions);
        tapes = new RectTransform[random];
        for (int i = 0; i < random; i++)
        {
            RectTransform tape = Instantiate(tapePrefab, transform);
            tapes[i] = tape;

            //get random position from the list
            int randomPosIndex = Random.Range(0, possiblePositions.Count);
            Vector2Int randomPos = possiblePositions[randomPosIndex];
            possiblePositions.RemoveAt(randomPosIndex);

            //set position
            Vector2 pos = (Vector2)randomPos * 0.5f;
            tape.anchorMin = pos;
            tape.anchorMax = pos;
            tape.anchoredPosition = Vector2.zero;

            //set rotation
            Vector3 rot = Vector3.zero;
            rot.z = Random.Range(-60f, 60f);
            tape.localEulerAngles = rot;
        }
    }

    void RemoveTapes()
    {
        if (tapes == null)
            return;

        //remove tapes
        foreach (var tape in tapes)
            Destroy(tape.gameObject);

        tapes = null;
    }
}
