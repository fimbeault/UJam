using System.Collections.Generic;
using UnityEngine;

public class UJamInputManager : MonoBehaviour
{
    private Dictionary<string, bool> mActiveAxisCache;

    public GameManager _GameManager;

    void Start()
    {
        mActiveAxisCache = new Dictionary<string, bool>();

        List<EAxisData> axisDataList = EAxisData.GetList();
        List<EPlayerId> playerIdList = EPlayerId.GetList();
        foreach (EPlayerId playerId in playerIdList)
        {
            foreach (EAxisData axisData in axisDataList)
            {
                mActiveAxisCache[axisData.GetFullAxisName(playerId)] = false;
            }
        }
    }

    void Update()
    {
        List<EAxisData> axisDataList = EAxisData.GetList();
        List<EPlayerId> playerIdList = EPlayerId.GetList();

        foreach (EPlayerId playerId in playerIdList)
        {
            foreach (EAxisData axisData in axisDataList)
            {
                string axisFullName = axisData.GetFullAxisName(playerId);
                bool isAxisActive = IsAxisActive(axisData, playerId);
                bool cachedAxisActive = mActiveAxisCache[axisFullName];

                if (cachedAxisActive != isAxisActive)
                {
                    DispatchAxisChanged(axisData, playerId, isAxisActive);
                    mActiveAxisCache[axisFullName] = isAxisActive;
                    break;
                }
            }
        }
    }

    private void DispatchAxisChanged(EAxisData aAxisData, EPlayerId aPlayerId, bool aIsAxisActive)
    {
        _GameManager.OnAxisChanged(aPlayerId, aAxisData, aIsAxisActive);
    }

    private bool IsAxisActive(EAxisData aAxisData, EPlayerId aPlayerId)
    {
        string axisFullInputName = aAxisData.GetFullInputName(aPlayerId);

        if (aAxisData.MinimalValue < 0 &&
            Input.GetAxis(axisFullInputName) < aAxisData.MinimalValue)
        {
            return true;
        }
        else if (aAxisData.MinimalValue > 0 &&
            Input.GetAxis(axisFullInputName) > aAxisData.MinimalValue)
        {
            return true;
        }

        return false;
    }
}