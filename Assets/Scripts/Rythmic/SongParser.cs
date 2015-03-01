using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class SongParser {

	static public List<SongManager.Section> ParseSong(string _file)
	{
		List<SongManager.Section> sectionList = new List<SongManager.Section> ();

		XmlDocument xml = new XmlDocument ();
		xml.Load (_file);

		XmlNodeList xmlSections = xml.SelectNodes ("//Section");

		foreach (XmlNode xmlSection in xmlSections)
		{
			SongManager.Section section = new SongManager.Section();
			float.TryParse(xmlSection.Attributes.GetNamedItem("startTime").Value, out section.fStartTime);
			float.TryParse(xmlSection.Attributes.GetNamedItem("endTime").Value, out section.fEndTime);
			float.TryParse(xmlSection.Attributes.GetNamedItem("BPM").Value, out section.fBPM);
			
			XmlNodeList xmlCombos = xmlSection.SelectNodes("Combo");

			foreach (XmlNode xmlCombo in xmlCombos)
			{
				Combo combo = new Combo();
				XmlNodeList xmlNotes = xmlCombo.SelectNodes("Note");

				float fWhole = section.fBPM / 240.0f;
				float fNextNoteTime = 1.0f / fWhole;

				foreach (XmlNode xmlNote in xmlNotes)
				{
					Note note = new Note();
					note.fTime = fNextNoteTime;
					note.sType = xmlNote.Attributes.GetNamedItem("type").Value;

					Note.NoteTime type = ParseNoteType(xmlNote.Attributes.GetNamedItem("time").Value);
					fNextNoteTime += 1.0f / (fWhole * (int)type);

					combo.notes.Add(note);
				}

				if (combo.notes.Count > 0)
					section.comboList.Add (combo);
			}

			sectionList.Add (section);
		}

		return sectionList;
	}

	static private Note.NoteTime ParseNoteType(string _type)
	{
		switch (_type)
		{
			case "Whole":
			{
				return Note.NoteTime.NoteType_Whole;
			}
			case "Half":
			{
				return Note.NoteTime.NoteType_Half;
			}
			case "Quarter":
			{
				return Note.NoteTime.NoteType_Quarter;
			}
			case "Eighth":
			{
				return Note.NoteTime.NoteType_Eighth;
			}
			case "Sixteenth":
			{
				return Note.NoteTime.NoteType_Sixteenth;
			}
			case "ThirtySecond":
			{
				return Note.NoteTime.NoteType_ThirtySecond;
			}
			case "SixtyFourth":
			{
				return Note.NoteTime.NoteType_SixtyFourth;
			}
		}

		return Note.NoteTime.NoteType_Quarter;
	}
}
