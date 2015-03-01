using UnityEngine;
using System.Collections;

public class FullScreenFadeUI : MonoBehaviour
{
    public float _FadeTime;

    public bool _FadeInOnStart = false;
    public bool _FadeOutOnStart = true;

    public float _DelayStartFade;

    private float mFadeSpeed;
    private SpriteRenderer mSpriteRenderer;

    private bool _DisplayOnStart = true;

    private bool mFadingIn;
    private bool mFadingOut;

	// Use this for initialization
	void Start ()
    {
        mFadeSpeed = 1 / _FadeTime;
        mSpriteRenderer = this.GetComponent<SpriteRenderer>();

        if (_DisplayOnStart == false)
        {
            mSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);
        }
        else
        {
            mSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }

        if (_FadeInOnStart) Invoke("PlayFadeIn", _DelayStartFade);
        if (_FadeOutOnStart) Invoke("PlayFadeOut", _DelayStartFade);
    }

    public void PlayFadeIn()
    {
        mFadingIn = true;
        mSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);
    }

    public void PlayFadeOut()
    {
        mFadingOut = true;
        mSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

	// Update is called once per frame
	void Update ()
    {
        if (mFadingOut)
        {
            mSpriteRenderer.color -= new Color(0f, 0f, 0f, mFadeSpeed * Time.deltaTime);

            if (mSpriteRenderer.color.a <= 0)
            {
                mFadingOut = false;
            }
        }

        if (mFadingIn)
        {
            mSpriteRenderer.color += new Color(0f, 0f, 0f, mFadeSpeed * Time.deltaTime);

            if (mSpriteRenderer.color.a >= 1)
            {
                mFadingIn = false;
            }
        }
	}
}
