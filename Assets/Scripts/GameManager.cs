using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public RythmicManager rythmicManager;

    private EPlayerId mCurrentPlayerId;

	// Use this for initialization
	void Start ()
    {
        mCurrentPlayerId = EPlayerId.PLAYER_ONE;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void OnAxisChanged(EPlayerId aPlayerId, EAxisData aAxisData, bool aIsAxisActive)
    {
        if (aIsAxisActive == false || aPlayerId != mCurrentPlayerId) return;

        rythmicManager.ProcessInput(aAxisData.AxisName);
    }
}
