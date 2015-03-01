using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RythmicManager : MonoBehaviour {

	const uint kuiStepGranularity = 16;
	const float kStepFrequency = 1.0f / kuiStepGranularity;
	const float kfPerfectAfterTiming = 0.05f;
	const float kfPerfectTiming = 0.05f;
	const float kfGoodTiming = 0.15f;
	const float kfOkTiming = 0.25f;
	const float kfOkAfterTiming = 0.1f;

	const float kfTotalAfterPerfectRatio = kfPerfectAfterTiming + kfOkAfterTiming;

	const float kfPerfectFloatTolerance = 0.01f;

	public VisualManager visualManager;
	public AudioClip tickSound;
	public AudioClip song;

	bool bSongStarted = false;

	Combo currentCombo = null;
	List<Note> visibleNotes = new List<Note>();

	float fSongTimer = 0.0f;
	float fStepTimer = 0.0f;
	float fComboTimer = 0.0f;

	uint uiStepCount = 0;

	float fCurrentUpdateFrequency = 0.0f;

	float fNoteAppearDelay = 3.0f;

	bool bGameEnded = false;

	public float CurrentUpdateFrequency {
		get
		{
			return fCurrentUpdateFrequency;
		}
	}

	// Use this for initialization
	void Start () {
		GetNextCombo ();

		gameObject.GetComponent<AudioSource>().PlayOneShot(song);
		bSongStarted = true;

		fStepTimer = fCurrentUpdateFrequency;
	}

	public void ProcessInput(string _sInput)
	{
		Note processedNote = null;

		foreach (Note note in visibleNotes)
		{
			if (note.sType.Equals(_sInput))
			{
				processedNote = note;
				break;
			}
		}

		if (processedNote == null)
		{
			// Miss you noob!
			return;
		}

		if (fComboTimer <= processedNote.fTime)
		{
			float noteDelta = processedNote.fTime - fComboTimer;
			float missRatio = noteDelta / fNoteAppearDelay;

			if (missRatio <= kfPerfectTiming)
			{
                visualManager.DisplayFeedback(processedNote, ETimingFeedbackType.PERFECT);
			}
			else if (missRatio <= kfGoodTiming)
			{
                visualManager.DisplayFeedback(processedNote, ETimingFeedbackType.GOOD);
			}
			else if (missRatio <= kfOkTiming)
			{
                visualManager.DisplayFeedback(processedNote, ETimingFeedbackType.OK);
			}
			else
			{
                visualManager.DisplayFeedback(processedNote, ETimingFeedbackType.MISS);
			}
		}
		else
		{
			float noteDelta = fComboTimer - processedNote.fTime; 
			float missRatio = noteDelta / fNoteAppearDelay;

			if (missRatio <= kfPerfectAfterTiming)
			{
                visualManager.DisplayFeedback(processedNote, ETimingFeedbackType.PERFECT);
			}
			else if (missRatio <= kfOkAfterTiming)
			{
                visualManager.DisplayFeedback(processedNote, ETimingFeedbackType.OK);
			}
			else
			{
                visualManager.DisplayFeedback(processedNote, ETimingFeedbackType.MISS);
			}
		}

		// Dissapear
		visualManager.DestroyNote(visibleNotes[0]);
		visibleNotes.Remove (processedNote);
	}
	
	// Update is called once per frame
	void Update ()
	{/*
		if (bGameEnded)
			return;*/

		fSongTimer += Time.deltaTime;

		// Update Current Combo
		fStepTimer += Time.deltaTime;
		if (fStepTimer >= fCurrentUpdateFrequency)
		{
			fComboTimer += fCurrentUpdateFrequency;

			if (uiStepCount == kuiStepGranularity)
				uiStepCount = 0;

			SongStep();
			uiStepCount++;

			fStepTimer -= fCurrentUpdateFrequency;
		}

		if (currentCombo == null)
			return;

		if (currentCombo.notes.Count == 0 && visibleNotes.Count == 0)
			return;

		if (fComboTimer < 0.0f)
			return;

		// Make notes appear
		while ((currentCombo.notes.Count > 0) &&
		       (fComboTimer >= currentCombo.notes[0].fTime - fNoteAppearDelay))
		{
			if (!currentCombo.notes[0].sType.Equals("Rest"))
			{
				// Appear
				float delay = fComboTimer - (currentCombo.notes[0].fTime - fNoteAppearDelay);

				visualManager.SpawnNote(currentCombo.notes[0], EPlayerId.PLAYER_ONE, fNoteAppearDelay - delay);
				visibleNotes.Add (currentCombo.notes[0]);
			}
			
			currentCombo.notes.RemoveAt (0);
		}
		
		// Check missed notes
		if (visibleNotes.Count > 0)
		{
			if (fComboTimer >= visibleNotes[0].fTime + fNoteAppearDelay * kfTotalAfterPerfectRatio)
			{
				// Disapear
				visualManager.DestroyNote(visibleNotes[0]);
				
				visibleNotes.RemoveAt(0);
			}
			else if (!visibleNotes[0].sType.Equals("Rest") &&
			         !visibleNotes[0].bPerfectTimePassed &&
			         fComboTimer >= visibleNotes[0].fTime - kfPerfectFloatTolerance)
			{
				// Perfect timing
				visualManager.DisplayNotePerfectTiming(visibleNotes[0], fNoteAppearDelay * kfTotalAfterPerfectRatio);

				visibleNotes[0].bPerfectTimePassed = true;
			}
		}
	}

	void SongStep()
	{
		if (uiStepCount == 0)
		{
			// Get Next Combo
			if (currentCombo != null && currentCombo.notes.Count == 0 && visibleNotes.Count == 0)
				GetNextCombo ();

			gameObject.GetComponent<AudioSource>().PlayOneShot(tickSound);
		}
	}

	void GetNextCombo()
	{
		currentCombo = SongManager.GetNextCombo();
		
		if (currentCombo == null)
		{
			// End the game
			bGameEnded = true;
			return;
		}

		fStepTimer = 0.0f;
		fComboTimer = 0.0f;//-2.0f * (60 / currentCombo.fBPM); // Pro hack!
		fCurrentUpdateFrequency = (60 / currentCombo.fBPM) * kStepFrequency;
	}
}
