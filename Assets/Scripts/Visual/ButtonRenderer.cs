using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonRenderer : MonoBehaviour
{
    public List<Sprite> _SpriteList;
    public float _FadeInSpeed;

    private SpriteRenderer mMySpriteRenderer;

    private bool mFadingIn;
    private bool mFadingOut;
    private float mFadeOutTimeRemaining;
    private float mFadeOutTimeTotal;

	// Use this for initialization
	void Awake ()
	{
        mMySpriteRenderer = GetComponent<SpriteRenderer>();
	}

    void Update()
    {
        if (mFadingIn)
        {
            mMySpriteRenderer.color += new Color(0f, 0f, 0f, _FadeInSpeed);

            if (mMySpriteRenderer.color.a >= 1)
            {
                mFadingIn = false;
            }
        }

        if (mFadingOut)
        {
            mFadeOutTimeRemaining -= Time.deltaTime;
            mMySpriteRenderer.color -= new Color(0f, 0f, 0f, (1 / mFadeOutTimeTotal * mFadeOutTimeRemaining));

            if (mMySpriteRenderer.color.a <= 0)
            {
                mFadingOut = false;
            }
        }
    }

    public void SetSpriteFrame(EAxisData aAxisData)
    {
        mMySpriteRenderer.sprite = _SpriteList[aAxisData.Id];
    }

    public void PlayFadeIn()
    {
        mMySpriteRenderer.color = new Color(1f,1f,1f,0f);
        mFadingIn = true;
    }

    public void PlayFadeOut(float aTimeElapsedRemaining)
    {
        mMySpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        mFadingOut = true;
        mFadeOutTimeRemaining = aTimeElapsedRemaining;
        mFadeOutTimeTotal = aTimeElapsedRemaining;
    }
}
