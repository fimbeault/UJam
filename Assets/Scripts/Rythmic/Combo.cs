using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combo {

	public List<Note> notes = new List<Note>();

	public Combo()
	{
	}

	public Combo(Combo combo)
	{
		notes = new List<Note> (combo.notes.Count);

		combo.notes.ForEach((item) => {
			notes.Add((Note)item.Clone());
		});
	}
}
