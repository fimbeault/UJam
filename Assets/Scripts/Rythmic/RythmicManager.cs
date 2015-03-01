using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RythmicManager : MonoBehaviour {

	public cameraShake camShake;

	const float kfSongTime = 180.0f;

	const uint kuiStepGranularity = 16;
	const float kStepFrequency = 1.0f / kuiStepGranularity;
	const float kfPerfectAfterTiming = 0.05f;
	const float kfPerfectTiming = 0.05f;
	const float kfGoodTiming = 0.15f;
	const float kfOkTiming = 0.25f;
	const float kfOkAfterTiming = 0.1f;

	const float kfTotalAfterPerfectRatio = kfPerfectAfterTiming + kfOkAfterTiming;

	const float kfPerfectFloatTolerance = 0.01f;

    public GameManager gameManager;
	public VisualManager visualManager;
	public AudioClip tickSound;
	public AudioClip song;

	bool bSongStarted = false;
	bool bGetRekt = false;

	Combo currentCombo = null;
	List<Note> visibleNotes = new List<Note>();

	float fSongTimer = 0.0f;
	float fStepTimer = 0.0f;
	float fComboTimer = 0.0f;

	uint uiStepCount = 0;

	float fCurrentUpdateFrequency = 0.0f;

	float fNoteAppearDelay = 2.0f;

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

	public void ProcessInput(string _sInput, EPlayerId aPlayerId)
	{
		Note processedNote = null;
		
		foreach (Note note in visibleNotes)
		{
			if (note.sType.Equals(_sInput) && aPlayerId == note.playerId)
			{
				processedNote = note;
				break;
			}
		}

		if (processedNote == null)
		{
			// Miss you noob!
			//Miss ();
			return;
		}

		if (fComboTimer <= processedNote.fTime)
		{
			float noteDelta = processedNote.fTime - fComboTimer;
			float missRatio = noteDelta / fNoteAppearDelay;

			if (missRatio <= kfPerfectTiming)
			{
                visualManager.DisplayFeedback(processedNote, ETimingFeedbackType.PERFECT, 100);
			}
			else if (missRatio <= kfGoodTiming)
			{
                visualManager.DisplayFeedback(processedNote, ETimingFeedbackType.GOOD, 75);
			}
			else if (missRatio <= kfOkTiming)
			{
                visualManager.DisplayFeedback(processedNote, ETimingFeedbackType.OK, 50);
			}
			else
			{
				Miss ();

				if (bGetRekt)
				{
					visualManager.DestroyNote(processedNote);
					visibleNotes.Remove (processedNote);
				}

				return;
			}
		}
		else
		{
			float noteDelta = fComboTimer - processedNote.fTime; 
			float missRatio = noteDelta / fNoteAppearDelay;

			if (missRatio <= kfPerfectAfterTiming)
			{
                visualManager.DisplayFeedback(processedNote, ETimingFeedbackType.PERFECT, 100);
			}
			else if (missRatio <= kfOkAfterTiming)
			{
                visualManager.DisplayFeedback(processedNote, ETimingFeedbackType.OK, 50);
			}
			else
			{
				Miss ();

				if (bGetRekt)
				{
					visualManager.DestroyNote(processedNote);
					visibleNotes.Remove (processedNote);
				}

				return;
			}
		}

		// Dissapear
        visualManager.DestroyNote(processedNote);
		visibleNotes.Remove (processedNote);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (bGameEnded)
			return;

        if (fSongTimer >= kfSongTime)
        {
            bGameEnded = true;
            gameManager.OnGameEnd();
            return;
        }

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

				currentCombo.notes[0].playerId = gameManager.CurrentActivePlayer;

				//if (gameManager.CurrentActivePlayer.Id == 0)
				{
					visualManager.SpawnNote(currentCombo.notes[0], gameManager.CurrentActivePlayer, fNoteAppearDelay - delay);
					visibleNotes.Add (currentCombo.notes[0]);
				}

				if (bGetRekt)
				{
					gameManager.OnStartNextCombo();
				}
			}
			
			currentCombo.notes.RemoveAt (0);
		}
		
		// Check missed notes
		if (visibleNotes.Count > 0)
		{
			if (fComboTimer >= visibleNotes[0].fTime + fNoteAppearDelay * kfTotalAfterPerfectRatio)
			{
				// Disapear
				Miss ();

				if (bGetRekt)
				{
					visualManager.DestroyNote(visibleNotes[0]);
					
					visibleNotes.RemoveAt(0);
				}

				return;
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
            if (currentCombo == null || currentCombo.notes.Count == 0 && visibleNotes.Count == 0)
			{
				GetNextCombo();

				if (!bGetRekt && currentCombo != null && currentCombo.notes.Count > 0)
					gameManager.OnStartNextCombo();
			}

			//gameObject.GetComponent<AudioSource>().PlayOneShot(tickSound);
		}
	}

	void GetNextCombo()
	{
		currentCombo = SongManager.GetNextCombo(fSongTimer);

		float fBPM = SongManager.GetCurrentSectionBPM ();

		fComboTimer = 0.0f;
		fCurrentUpdateFrequency = (60 / fBPM) * kStepFrequency;

		if (fBPM <= 111.0f)
			fNoteAppearDelay = 2.0f;
		else if (fBPM <= 136.0f)
			fNoteAppearDelay = 1.4f;
		else
			fNoteAppearDelay = 1.1f;
	}

	void Miss()
	{
		visualManager.DisplayFeedback(visibleNotes[0], ETimingFeedbackType.MISS, 0);

		if (bGetRekt)
			return;

		if (camShake != null)
			camShake.Shake ();

		foreach (Note note in visibleNotes)
			visualManager.DestroyNote(note);

		visibleNotes.Clear ();
		currentCombo.notes.Clear ();
		currentCombo = null;
	}
}
