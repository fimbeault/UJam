using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisualManager : MonoBehaviour
{
    public List<SpriteRenderer> _CrowdIndicatorList;

    public List<Transform> _P1SpawnPositionsList;
    public List<Transform> _P2SpawnPositionsList;

    public float _ButtonTravelDistance;
    public float _ArrowTravelDistance;

    public ButtonRenderer _ButtonRendererPrefab;

    public List<CharacterRenderer> _P1CharacterRendererList;
    public List<CharacterRenderer> _P2CharacterRendererList;

    private List<DisplayedNoteData> mDisplayedNoteDataList;

	// Use this for initialization
	void Start ()
    {
        mDisplayedNoteDataList = new List<DisplayedNoteData>();
	}
	
	// Update is called once per frame
	void Update()
    {
        MoveDisplayedNotes(Time.deltaTime);
	}

    private void MoveDisplayedNotes(float aTimeElapsed)
    {
        foreach (DisplayedNoteData displayedNoteData in mDisplayedNoteDataList)
        {
            if (!displayedNoteData.DoUpdatePosition) continue;

            Transform rendererTransform = displayedNoteData.Renderer.transform;
            Vector3 velocity = new Vector3();
            float travelDistance = displayedNoteData.TravelSpeed * aTimeElapsed;
            
            switch (displayedNoteData.AxisData.AxisDirection)
            {
                case EAxisData.DIRECTION_LEFT:
                {
                    velocity.x = travelDistance;
                    break;
                }
                case EAxisData.DIRECTION_RIGHT:
                {
                    velocity.x = -travelDistance;
                    break;
                }
                case EAxisData.DIRECTION_UP:
                {
                    velocity.y = -travelDistance;
                    break;
                }
                case EAxisData.DIRECTION_DOWN:
                {
                    velocity.y = travelDistance;
                    break;
                }
            }

            rendererTransform.position += velocity;
        }
    }

    public void DisplayNotePerfectTiming(Note aNote, float aRemainingLifetime)
    {
        DisplayedNoteData displayedNoteData = GetDisplayedNoteDataByNote(aNote);

        if (displayedNoteData == null)
        {
            Debug.LogError("VisualManager.DisplayNotePerfectTiming :: Tried to display perfect timing on a note that isn't registered");
            return;
        }

        displayedNoteData.DoUpdatePosition = false;
        displayedNoteData.Renderer.PlayPerfectTimingReached(aRemainingLifetime);

        displayedNoteData.OnPositionReachedTime = Time.time;
        displayedNoteData.OnPositionReachedTimeLeft = aRemainingLifetime;
    }

    public void SpawnNote(Note aNote, EPlayerId aPlayerId, float aTimeTotravel)
    {
        EAxisData axisData = EAxisData.GetAxisByName(aNote.sType);

        List<Transform> spawnPositionList = (aPlayerId.Id == 0) ? _P1SpawnPositionsList : _P2SpawnPositionsList;

        ButtonRenderer buttonRenderer = Instantiate(_ButtonRendererPrefab) as ButtonRenderer;
        buttonRenderer.SetSpriteFrame(axisData);
        buttonRenderer.gameObject.transform.position = spawnPositionList[axisData.AxisDirection].position;

        buttonRenderer.PlayFadeIn();

        DisplayedNoteData displayedNoteData = new DisplayedNoteData();
        displayedNoteData.Note = aNote;
        displayedNoteData.Renderer = buttonRenderer;
        displayedNoteData.AxisData = axisData;
        displayedNoteData.TravelSpeed = GetTravelDistance(aNote) / aTimeTotravel;
        displayedNoteData.OriginalPosition = buttonRenderer.gameObject.transform.position;
        displayedNoteData.DoUpdatePosition = true;

        displayedNoteData.OnNodeCreateTime = Time.time;
        displayedNoteData.NodeTravelElapsedTime = aTimeTotravel;

        mDisplayedNoteDataList.Add(displayedNoteData);
    }

    private float GetTravelDistance(Note aNote)
    {
        float travelDistance = _ButtonTravelDistance;

        if (aNote.sType == EAxisData.LEFT.AxisName ||
            aNote.sType == EAxisData.RIGHT.AxisName ||
            aNote.sType == EAxisData.UP.AxisName ||
            aNote.sType == EAxisData.DOWN.AxisName)
        {
            travelDistance = _ArrowTravelDistance;
        }

        return travelDistance;
    }

    public void DestroyNote(Note aNote)
    {
        DisplayedNoteData displayedNoteData = GetDisplayedNoteDataByNote(aNote);

        if (displayedNoteData == null)
        {
            Debug.LogError("VisualManager.DestroyNote :: Tried to delete a note that isn't registered");
            return;
        }

        Debug.Log("Killed node. Start time : " + displayedNoteData.OnNodeCreateTime + ", travel time : " + displayedNoteData.NodeTravelElapsedTime + ", Position reached time " + displayedNoteData.OnPositionReachedTime + ", TimeElapsed left received : " + displayedNoteData.OnPositionReachedTimeLeft + ", Current time : " + Time.time);

        Destroy(displayedNoteData.Renderer.gameObject);
        mDisplayedNoteDataList.Remove(displayedNoteData);
    }

    public void DisplayFeedback(Note aNote, ETimingFeedbackType aFeedbackType)
    {
        Debug.Log(aFeedbackType);
    }

    private DisplayedNoteData GetDisplayedNoteDataByNote(Note aNote)
    {
        foreach (DisplayedNoteData displayedNoteData in mDisplayedNoteDataList)
        {
            if (displayedNoteData.Note == aNote)
            {
                return displayedNoteData;
            }
        }

        return null;
    }
}

public class DisplayedNoteData
{
    public Note Note;
    public ButtonRenderer Renderer;
    public EAxisData AxisData;
    public float TravelSpeed;
    public Vector3 OriginalPosition;
    public bool DoUpdatePosition;


    public float OnNodeCreateTime;
    public float NodeTravelElapsedTime;
    public float OnPositionReachedTime;
    public float OnPositionReachedTimeLeft;
}
