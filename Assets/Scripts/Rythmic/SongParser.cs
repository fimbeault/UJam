using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class SongParser {

	static public List<Combo> ParseSong(string _file)
	{
		List<Combo> comboList = new List<Combo> ();

		XmlDocument xml = new XmlDocument ();
		xml.Load (_file);

		XmlNodeList xmlSections = xml.SelectNodes ("//Section");

		foreach (XmlNode xmlSection in xmlSections)
		{
			XmlNodeList xmlCombos = xmlSection.SelectNodes("Combo");

			foreach (XmlNode xmlCombo in xmlCombos)
			{
				Combo combo = new Combo();

				XmlNodeList xmlNotes = xmlCombo.SelectNodes("Note");

				foreach (XmlNode xmlNote in xmlNotes)
				{
					Note note = new Note();
					float.TryParse(xmlNote.Attributes.GetNamedItem("time").Value, out note.fTime);

					combo.notes.Add(note);
				}

				if (combo.notes.Count > 0)
					comboList.Add (combo);
			}
		}

		return comboList;
	}
}
