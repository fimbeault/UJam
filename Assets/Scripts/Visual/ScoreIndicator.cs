using UnityEngine;
using System.Collections;

public class ScoreIndicator : MonoBehaviour {

	const float fScoreMax = 5000.0f;

	const float fYMin = -5.0f;
	const float fYAwsomeZoneStart = 3.3f;
	const float fYMax = 5.2f;

	float fCurrentScore = 0.0f;

	public void ResetScore()
	{
		fCurrentScore = 0.0f;
	}

	public void AddScore()
	{
	}
}
