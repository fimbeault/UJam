using UnityEngine;
using System.Collections;

public class RythmicTests : MonoBehaviour {

	public InputResult mGoodResult;
	public InputResult mBadResult;

	private const float kfShowTime = 0.2f;
	private const float kfBadDelay = 0.2f;
	private const float kfNextTiming = 1.0f;
	
	private float mfMusicTimer = 0.0f;
	private float mfVisualTimer = 0.0f;
	private bool mbVisible = false;

	// Use this for initialization
	void Start () {
		ToggleVisual (mbVisible);
	}
	
	// Update is called once per frame
	void Update ()
	{
		mfMusicTimer += Time.deltaTime;

		UpdateVisual ();
		UpdateInput ();

		UpdateDebug ();

		if (mfMusicTimer >= kfNextTiming + kfBadDelay) {
			mfMusicTimer = 0.0f;
			mBadResult.Show();
		}
	}

	void UpdateInput()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (mbVisible)
			{
				mGoodResult.Show ();
				mfMusicTimer = 0.0f;
			}
			else
			{
				mBadResult.Show ();
			}
		}
	}

	void UpdateVisual()
	{
		if (mbVisible)
		{
			mfVisualTimer += Time.deltaTime;

			if (mfVisualTimer >= kfShowTime)
			{
				mfVisualTimer = 0.0f;

				mbVisible = false;
				ToggleVisual (mbVisible);
			}
		}
		else
		{
			if (mfMusicTimer >= kfNextTiming - kfShowTime)
			{
				mbVisible = true;
				ToggleVisual(mbVisible);
			}
		}
	}

	void UpdateDebug()
	{
	}

	void ToggleVisual(bool _bVisible)
	{
		gameObject.GetComponent<Renderer> ().enabled = _bVisible;
	}
}
