using UnityEngine;
using System.Collections;

public class Note {

	public enum NoteTime
	{
		NoteType_Whole,
		NoteType_Half,
		NoteType_Quarter,
		NoteType_Eighth,
		NoteType_Sixteenth,
		NoteType_ThirtySecond,
		NoteType_SixtyFourth
	};

    public string sName;
	public float fTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
