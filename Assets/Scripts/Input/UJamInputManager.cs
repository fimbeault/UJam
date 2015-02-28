using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UJamInputManager : MonoBehaviour
{
    public List<ButtonRenderer> _ButtonRendererList;

	void Start ()
    {
	
	}
	
	void Update ()
    {
        //UpdatePressedCache();
	}

    private void UpdatePressedCache()
    {
        List<EAxisData> axisDataList = EAxisData.GetList();
        List<EPlayerId> playerIdList = EPlayerId.GetList();

        for (int i = 0; i < _ButtonRendererList.Count; i++)
        {
            ButtonRenderer buttonRenderer = _ButtonRendererList[i];
        
            bool isInputEntered = false;

            foreach (EAxisData axisData in axisDataList)
            {
                if (IsAxisActive(playerIdList[i], axisData))
                {
                    isInputEntered = true;
                    buttonRenderer.SetSpriteFrame(axisData);
                    break;
                }
            }

            if (buttonRenderer.gameObject.activeInHierarchy != isInputEntered)
            {
                buttonRenderer.gameObject.SetActive(isInputEntered);
            }
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
