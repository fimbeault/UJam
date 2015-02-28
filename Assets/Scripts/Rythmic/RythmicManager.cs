using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RythmicManager : MonoBehaviour {

	Combo currentCombo = null;
	List<Note> visibleNotes = new List<Note>();

	float fComboTimer = 0.0f;

	float kfNoteAppearDelay = 0.4f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		fComboTimer += Time.deltaTime;

		CheckEndGame ();
		PoolNextNoteToShow ();
		CleanUnplayedNotes ();
	}

	void CheckEndGame()
	{
		if (currentCombo == null)
		{
			currentCombo = SongManager.GetNextCombo();
			
			if (currentCombo == null)
			{
				// End the game
				return;
			}
		}
	}

	void PoolNextNoteToShow()
	{
		if (currentCombo.notes.Count == 0) {
			currentCombo = null;
			return;
		}

		Note nextNote = currentCombo.notes [0];

		float fNextNoteTime = nextNote.fTime - fComboTimer;
		if (fNextNoteTime <= kfNoteAppearDelay)
		{
			// Appear

			visibleNotes.Add (nextNote);
			currentCombo.notes.RemoveAt (0);
		}
	}

	void CleanUnplayedNotes()
	{
		while (fComboTimer > visibleNotes[0].fTime + kfNoteAppearDelay)
		{
			// Disapear

			visibleNotes.RemoveAt(0);
		}
	}
}
