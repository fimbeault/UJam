using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RythmicManager : MonoBehaviour {

	const float kStepFrequency = 1.0f / 64.0f;

	public VisualManager visualManager;

	Combo currentCombo = null;
	List<Note> visibleNotes = new List<Note>();

	float fSongTimer = 0.0f;
	float fStepTimer = 0.0f;
	float fComboTimer = 0.0f;

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

		if (missRatio < 0.3f)
		{
			// Wow such awesome!
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

		// Get Next Combo
		if (currentCombo.notes.Count == 0)
		{
			GetNextCombo ();
			return;
		}

		// Update Current Combo
		fStepTimer += Time.deltaTime;
		if (fStepTimer >= fCurrentUpdateFrequency)
		{
			fComboTimer += fStepTimer;

			SongStep();
			fStepTimer = 0.0f;
		}
	}

	void SongStep()
	{
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
		if (currentCombo == null)
		{
			currentCombo = SongManager.GetNextCombo();
			
			if (currentCombo == null)
			{
				// End the game
				bGameEnded = true;
				return;
			}
		}

		fStepTimer = 0.0f;
		fComboTimer = 0.0f;
		fCurrentUpdateFrequency = (60 / currentCombo.fBPM) * kStepFrequency;
	}
}
