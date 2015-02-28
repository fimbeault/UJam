using UnityEngine;
using System.Collections;

public class InputResult : MonoBehaviour {

	private const float kfShowTime = 0.2f;

	private float mfTimer = 0.0f;
	private bool mbVisible = false;

	public void Show()
	{
		mbVisible = true;
		gameObject.GetComponent<Renderer> ().enabled = true;
	}

	public void Hide()
	{
		mbVisible = false;
		gameObject.GetComponent<Renderer> ().enabled = false;
	}

	// Use this for initialization
	void Start () {
		Hide ();
	}
	
	// Update is called once per frame
	void Update () {
		if (mbVisible) {
			mfTimer += Time.deltaTime;

			if (mfTimer > kfShowTime)
			{
				mfTimer = 0.0f;
				Hide ();
			}
		}
	}
}
