using UnityEngine;
using System.Collections;

public class FullScreenFadeOutUI : MonoBehaviour
{
    public float _FadeTime;
    public float _FinalAlpha;

    private float mFadeSpeed;
    private SpriteRenderer mSpriteRenderer;

	// Use this for initialization
	void Start ()
    {
        mFadeSpeed = (1 - _FinalAlpha) / _FadeTime;
        mSpriteRenderer = this.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (mSpriteRenderer.color.a <= 0)
        {
            Destroy(this);
            return;
        }

        mSpriteRenderer.color -= new Color(0f, 0f, 0f, mFadeSpeed * Time.deltaTime);
	}
}
