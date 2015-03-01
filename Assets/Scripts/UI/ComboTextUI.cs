using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboTextUI : MonoBehaviour 
{
    public enum ECurrentColorState
    {
        INITIAL_COLOR = 1,
        SHAKE_COLOR = 2,
    };

    public Color _DuringShakeColor;
    public string _ComboTextTemplate;
    public string _ComboTextToReplace;
    public float _ColorFadeDuration;

    private Color mInitialColor;
    private Text mMyText;
    private cameraShake mMyCameraShake;

    private ECurrentColorState mCurrentColorState;

    private float mTimeStampChangeColor;

	// Use this for initialization
	void Awake ()
    {
        mMyCameraShake = this.GetComponent<cameraShake>();
        mMyText = this.GetComponent<Text>();
        mInitialColor = mMyText.color;
        mCurrentColorState = ECurrentColorState.INITIAL_COLOR;

        this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (mTimeStampChangeColor != 0)
        {
            if (mCurrentColorState == ECurrentColorState.INITIAL_COLOR &&
                Time.time - mTimeStampChangeColor >= _ColorFadeDuration)
            {
                mMyText.CrossFadeColor(mInitialColor, _ColorFadeDuration, false, true);
                mCurrentColorState = ECurrentColorState.INITIAL_COLOR;
                mTimeStampChangeColor = 0f;
            }
        }
	}

    public void DisplayComboCount(uint aCount)
    {
        if (aCount == 0) this.gameObject.SetActive(false);
        else this.gameObject.SetActive(true);

        mMyCameraShake.Shake();

        mMyText.text = _ComboTextTemplate.Replace(_ComboTextToReplace, aCount.ToString());

        mMyText.color = mInitialColor;
        mCurrentColorState = ECurrentColorState.INITIAL_COLOR;
        mMyText.CrossFadeColor(_DuringShakeColor, _ColorFadeDuration, false, false);
        mTimeStampChangeColor = Time.time;
    }
}
