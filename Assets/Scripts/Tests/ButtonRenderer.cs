using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonRenderer : MonoBehaviour
{
    public List<Sprite> _SpriteList;

    private SpriteRenderer mMySpriteRenderer;

	// Use this for initialization
	void Start ()
	{
        mMySpriteRenderer = GetComponent<SpriteRenderer>();
	}

    public void SetSpriteFrame(EAxisData aButtonType)
    {
        mMySpriteRenderer.sprite = _SpriteList[aButtonType.SpriteIndex];
    }
}
