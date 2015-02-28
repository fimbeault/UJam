using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RythmicManager : MonoBehaviour {

	const uint kuiStepGranularity = 16;
	const float kStepFrequency = 1.0f / kuiStepGranularity;
	const float kfPerfectTiming = 0.1f;
	const float kfGoodTiming = 0.2f;
	const float kfBadTiming = 0.4f;

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

	float kfNoteAppearDelay = 0.4f;

	bool bGameEnded = false;

	// Use this for initialization
	void Start () {
		GetNextCombo ();
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

		float noteDelta = Mathf.Abs(processedNote.fTime - fComboTimer);
		float missRatio = noteDelta / kfNoteAppearDelay;

		if (missRatio <= kfPerfectTiming)
		{
			// Wow such awesomely perfect!
		}
		else if (missRatio <= kfGoodTiming)
		{
			// Much good
		}
		else if (missRatio <= kfBadTiming)
		{
			// Very lame
		}
		else
		{
			// So fail
		}

		// Dissapear
		visualManager.DestroyNote(visibleNotes[0]);
		visibleNotes.Remove (processedNote);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (bGameEnded)
			return;

		fSongTimer += Time.deltaTime;

		// Update Current Combo
		fStepTimer += Time.deltaTime;
		if (fStepTimer >= fCurrentUpdateFrequency)
		{
			fComboTimer += fStepTimer;

			if (uiStepCount == kuiStepGranularity)
				uiStepCount = 0;

			SongStep();
			uiStepCount++;

			fStepTimer = 0.0f;
		}
	}

	void SongStep()
	{
		if (!bSongStarted)
		{
			gameObject.GetComponent<AudioSource>().PlayOneShot(song);
			bSongStarted = true;
		}

		if (uiStepCount == 0)
		{
			// Get Next Combo
			if (currentCombo.notes.Count == 0 && visibleNotes.Count == 0)
				GetNextCombo ();

			gameObject.GetComponent<AudioSource>().PlayOneShot(tickSound);
		}

		if (currentCombo.notes.Count == 0 && visibleNotes.Count == 0)
			return;

		// Make notes appear
		while ((currentCombo.notes.Count > 0) &&
		       (fComboTimer >= currentCombo.notes[0].fTime - kfNoteAppearDelay))
		{
			if (!currentCombo.notes[0].sType.Equals("Rest"))
			{
				// Appear
				visualManager.SpawnNote(currentCombo.notes[0], EPlayerId.PLAYER_ONE, kfNoteAppearDelay * 2);
				visibleNotes.Add (currentCombo.notes[0]);
			}

			currentCombo.notes.RemoveAt (0);
		}

		// Check missed notes
		while ((visibleNotes.Count > 0) &&
		       (fComboTimer > visibleNotes[0].fTime + kfNoteAppearDelay))
		{
			// Disapear
			visualManager.DestroyNote(visibleNotes[0]);

			visibleNotes.RemoveAt(0);
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
		fComboTimer = 0.0f;
		fCurrentUpdateFrequency = (60 / currentCombo.fBPM) * kStepFrequency;
	}
}
