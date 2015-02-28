using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SongManager : MonoBehaviour {

	static private List<Combo> comboList = new List<Combo>();

	static public Combo GetNextCombo()
	{
		if (comboList.Count > 0) {
			Combo combo = comboList[0];
			comboList.RemoveAt(0);

			return combo;
		}

		return null;
	}

	// Use this for initialization
	void Init () {
		Combo combo = new Combo();

		for (int i = 0; i < 10; ++i) {
			Note note = new Note();
			note.fTime = (float)i;

			combo.notes.Add(note);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
