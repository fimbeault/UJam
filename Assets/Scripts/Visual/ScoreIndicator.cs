using UnityEngine;
using System.Collections;

public class ScoreIndicator : MonoBehaviour {

	const float fScoreMax = 10000.0f;

	const float fYMin = -5.0f;
	const float fYAwsomeZoneStart = 3.3f;
	const float fYMax = 5.2f;

	const float lerpSpeed = 1.0f;
	float lastUpdateTime = 0.0f;

	float fCurrentScore = 0.0f;
	float fYPos = fYMin;

	public void Update()
	{
		if (lastUpdateTime == 0.0f)
			return;

		Vector3 position = gameObject.transform.position;
		Vector3 wantedPos = position;
		wantedPos.y = fYPos;

		float distance = (Time.time - lastUpdateTime) * lerpSpeed;
		float frac = distance / lerpSpeed;
		
		wantedPos = Vector3.Lerp (position, wantedPos, frac);
		gameObject.transform.position = wantedPos;
	}

	public void ResetScore()
	{
		fCurrentScore = 0.0f;

		UpdateVisual ();
	}

	public void AddScore(float _fScore)
	{
		fCurrentScore += _fScore;

		if (fCurrentScore > fScoreMax)
			fCurrentScore = fScoreMax;

		if (fCurrentScore < 0.0f)
			fCurrentScore = 0.0f;

		UpdateVisual ();
	}

	public void RemoveScore(float _fScore)
	{
		fCurrentScore -= _fScore;

		if (fCurrentScore > fScoreMax)
			fCurrentScore = fScoreMax;
		
		if (fCurrentScore < 0.0f)
			fCurrentScore = 0.0f;
		
		UpdateVisual ();
	}

	public void SetScore(float _fScore)
	{
		fCurrentScore = _fScore;
		
		UpdateVisual ();
	}

	private void UpdateVisual()
	{
		lastUpdateTime = Time.time;
		fYPos = fYMin + fCurrentScore * (fYMax - fYMin) / fScoreMax;
	}

    public float CurrentScore
    {
        get
        {
            return fCurrentScore;
        }
    }
}
