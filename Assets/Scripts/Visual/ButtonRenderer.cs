using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonRenderer : MonoBehaviour
{
    public List<Sprite> _SpriteList;
    public float _FadeInSpeed;
    public float _FadeOutSpeed;

    private SpriteRenderer mMySpriteRenderer;

    private bool mFadingIn;
    private bool mFadingOut;

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
            mMySpriteRenderer.color -= new Color(0f, 0f, 0f, _FadeOutSpeed);

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

    public void PlayFadeOut()
    {
        mMySpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        mFadingOut = true;
    }
}
