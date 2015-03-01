using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SongManager : MonoBehaviour {
 
	public class Section
	{
		public float fStartTime = 0.0f;
		public float fEndTime = 0.0f;
		public float fBPM = 0.0f;

		public List<Combo> comboList = new List<Combo>();

		public Combo GetRandomCombo()
		{
			if (comboList.Count == 0)
				return null;

			int index = Random.Range (0, comboList.Count);
			return new Combo(comboList[index]);
		}
	}

	private static List<Section> sectionList = new List<Section>();
	

	static public Combo GetNextCombo(float _songTime)
	{
		if (sectionList.Count == 0)
			return null;

		while (_songTime > sectionList [0].fEndTime)
			sectionList.RemoveAt (0);

		if (_songTime < sectionList [0].fStartTime)
			return null;

		return sectionList[0].GetRandomCombo ();
	}

	static public float GetCurrentSectionBPM()
	{
		return sectionList [0].fBPM;
	}

	// Use this for initialization
	void Awake () {
		sectionList = SongParser.ParseSong (Application.streamingAssetsPath + "/SongsDefinition/Song1.xml");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
