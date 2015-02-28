using UnityEngine;
using System.Collections;

public class Note {

	public enum NoteTime
	{
		NoteType_Whole = 1,
		NoteType_Half = 2,
		NoteType_Quarter = 4,
		NoteType_Eighth = 8,
		NoteType_Sixteenth = 16,
		NoteType_ThirtySecond = 32,
		NoteType_SixtyFourth = 64
	};

	public float fTime;
	public string sType;
}
