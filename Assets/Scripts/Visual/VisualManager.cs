﻿using UnityEngine;
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

            if (Vector3.Distance(rendererTransform.position, displayedNoteData.OriginalPosition) >= _ButtonTravelDistance)
            {
                displayedNoteData.DoUpdatePosition = false;
                displayedNoteData.Renderer.PlayFadeOut();
            }
        }
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
        displayedNoteData.TravelSpeed = _ButtonTravelDistance / aTimeTotravel;
        displayedNoteData.OriginalPosition = buttonRenderer.gameObject.transform.position;
        displayedNoteData.DoUpdatePosition = true;

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

public class DisplayedNoteData
{
    public Note Note;
    public ButtonRenderer Renderer;
    public EAxisData AxisData;
    public float TravelSpeed;
    public Vector3 OriginalPosition;
    public bool DoUpdatePosition;
}
