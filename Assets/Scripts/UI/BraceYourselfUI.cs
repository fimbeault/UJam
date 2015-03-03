using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BraceYourselfUI : MonoBehaviour
{
    public List<float> _FlickerTimes;

    private List<float> mFlickerTimesCopy;
    private float mDisplayTime;

	// Use this for initialization
	void Start ()
    {
	    
	}

    //3 sec

    //84
    //122
    //183
	
	// Update is called once per frame
	void Update ()
    {
        if (mDisplayTime != 0)
        {
            if (mFlickerTimesCopy[0] > (Time.time - mDisplayTime))
            {
                this.gameObject.SetActive(!this.gameObject.activeInHierarchy);
                mFlickerTimesCopy.RemoveRange(0, 1);

                if (mFlickerTimesCopy.Count == 0)
                {
                    Hide();
                }
            }
        }
	}

    public void Display()
    {
        mFlickerTimesCopy = new List<float>(_FlickerTimes);
        mDisplayTime = Time.time;
        //this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        mDisplayTime = 0;
        //this.gameObject.SetActive(false);
    }
}
