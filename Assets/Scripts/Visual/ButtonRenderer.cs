using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonRenderer : MonoBehaviour
{
    public List<Sprite> _SpriteList;

    private SpriteRenderer mMySpriteRenderer;

	// Use this for initialization
	void Awake ()
	{
        mMySpriteRenderer = GetComponent<SpriteRenderer>();
	}

    public void SetSpriteFrame(EAxisData aAxisData)
    {
        mMySpriteRenderer.sprite = _SpriteList[aAxisData.Id];
    }
}
