using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisualManager : MonoBehaviour
{
    public List<SpriteRenderer> _CrowdIndicatorList;

    public List<Transform> _P1SpawnPositionsList;
    public List<Transform> _P2SpawnPositionsList;

    public float _ButtonTravelDistance;

    public ButtonRenderer _ButtonRendererPrefab;

    public List<DisplayedNoteData> mDisplayedNoteDataList;

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
	/*
    public void OnAxisChanged(EPlayerId aPlayerId, EAxisData aAxisData, bool aIsAxisActive)
    {
        Note note = new Note();
        note.sType = aAxisData.AxisName;
        note.fTime = 3.0f;

        SpawnNote(note, aPlayerId);

        StartCoroutine(DestroyWithDelay(note));
    }

    private IEnumerator DestroyWithDelay(Note aNote)
    {
        yield return new WaitForSeconds(4f);
        DestroyNote(aNote);
    }*/

    public void SpawnNote(Note aNote, EPlayerId aPlayerId, float fLifetime)
    {
        EAxisData axisData = EAxisData.GetAxisByName(aNote.sType);

        List<Transform> spawnPositionList = (aPlayerId.Id == 0) ? _P1SpawnPositionsList : _P2SpawnPositionsList;

        ButtonRenderer buttonRenderer = Instantiate(_ButtonRendererPrefab) as ButtonRenderer;
        buttonRenderer.SetSpriteFrame(axisData);
        buttonRenderer.gameObject.transform.position = spawnPositionList[axisData.AxisDirection].position;


        DisplayedNoteData displayedNoteData = new DisplayedNoteData();
        displayedNoteData.Note = aNote;
        displayedNoteData.Renderer = buttonRenderer;
        displayedNoteData.AxisData = axisData;
		displayedNoteData.TravelSpeed = _ButtonTravelDistance / fLifetime;

        mDisplayedNoteDataList.Add(displayedNoteData);
    }

    public void DestroyNote(Note aNote)
    {
        foreach (DisplayedNoteData displayedNoteData in mDisplayedNoteDataList)
        {
            if (displayedNoteData.Note == aNote)
            {
                Destroy(displayedNoteData.Renderer.gameObject);
                mDisplayedNoteDataList.Remove(displayedNoteData);
                return;
            }
        }
    }
}

public struct DisplayedNoteData
{
    public Note Note;
    public ButtonRenderer Renderer;
    public EAxisData AxisData;
    public float TravelSpeed;
}
