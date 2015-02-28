using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UJamInputManager : MonoBehaviour
{
    public ButtonRenderer _ButtonRenderer;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        List<EAxisData> axisDataList = EAxisData.GetList();
        //List<EPlayerId> playerIdList = EPlayerId.GetList();

        bool isInputEntered = false;

        foreach (EAxisData axisData in axisDataList)
        {
            if (IsAxisActive(EPlayerId.PLAYER_ONE, axisData))
            {
                Debug.Log("Input is down : " + axisData.ToString());
                isInputEntered = true;
                _ButtonRenderer.SetSpriteFrame(axisData);
                break;
            }
        }

        if (_ButtonRenderer.gameObject.activeInHierarchy != isInputEntered)
        {
            _ButtonRenderer.gameObject.SetActive(isInputEntered);
        }
	}

    private bool IsAxisActive(EPlayerId aPlayerId, EAxisData aAxisData)
    {
        //Debug.Log("IsKeyActive : " + aAxisData.ToString() + ", suffix : " + aPlayerId.InputSuffix);

        if (aAxisData.MinimalValue < 0)
        {
            if (Input.GetAxis(aAxisData.AxisInputString + aPlayerId.InputSuffix) < aAxisData.MinimalValue)
            {
                return true;
            }
        }
        else
        {
            if (Input.GetAxis(aAxisData.AxisInputString + aPlayerId.InputSuffix) > aAxisData.MinimalValue)
            {
                return true;
            }
        }
        
        return false;
    }
}
