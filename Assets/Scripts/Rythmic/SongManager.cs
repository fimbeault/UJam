using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SongManager : MonoBehaviour {
 
	static private List<Combo> comboList = new List<Combo>();

	static public Combo GetNextCombo()
	{
		if (comboList == null)
			return null;

		if (comboList.Count > 0) {
			Combo combo = comboList[0];
			comboList.RemoveAt(0);

			return combo;
		}

		return null;
	}

	// Use this for initialization
	void Awake () {
		comboList = SongParser.ParseSong (Application.dataPath + "/Data/SongsDefinition/Song1.xml");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
