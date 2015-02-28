using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonRenderer : MonoBehaviour
{
    public List<Sprite> _SpriteList;
    public float _FadeInSpeed;

    private SpriteRenderer mMySpriteRenderer;

	// Use this for initialization
	void Awake ()
	{
        mMySpriteRenderer = GetComponent<SpriteRenderer>();
	}

    void Update()
    {
        if (mMySpriteRenderer.color.a < 1)
        {
            Debug.Log("Current alpha : " + mMySpriteRenderer.color.a);
            mMySpriteRenderer.color += new Color(0f, 0f, 0f, _FadeInSpeed);
        }
    }

    public void SetSpriteFrame(EAxisData aAxisData)
    {
        mMySpriteRenderer.sprite = _SpriteList[aAxisData.Id];
    }

    public void PlayFadeIn()
    {
        mMySpriteRenderer.color = new Color(1f,1f,1f,0f);
    }
}
