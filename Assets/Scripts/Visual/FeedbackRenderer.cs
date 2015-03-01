using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FeedbackRenderer : MonoBehaviour
{
    public List<Sprite> _FeedbackSpriteList;
    private SpriteRenderer mMySpriteRenderer;

    public float _DisplayLifeSpan;
    public float _FadeOutPercentage;

    private float mDisplayTimeStamp;
    private float mFadeOutSpeed;

	// Use this for initialization
    void Awake()
    {
        mMySpriteRenderer = GetComponent<SpriteRenderer>();

        mFadeOutSpeed = _FadeOutPercentage / _DisplayLifeSpan;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (mDisplayTimeStamp != 0f)
        {
            mMySpriteRenderer.color -= new Color(0f, 0f, 0f, (mFadeOutSpeed * Time.deltaTime));
            
            if (Time.time - mDisplayTimeStamp > _DisplayLifeSpan)
            {
                Destroy(this.gameObject);
            }
        }
	}

    public void DisplayFeedbackType(ETimingFeedbackType aFeedbackType)
    {
        mMySpriteRenderer.sprite = _FeedbackSpriteList[aFeedbackType.Id];
        mDisplayTimeStamp = Time.time;
    }
}
