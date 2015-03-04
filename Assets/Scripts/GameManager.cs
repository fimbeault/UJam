using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public enum GameMode
	{
		SinglePlayer,
		Multiplayer
	}

	public GameMode gameMode = GameMode.SinglePlayer;

	public RythmicManager rythmicManager;
    public VisualManager visualManager;

    private EPlayerId mCurrentPlayerId;


    public static EPlayerId Winner;

	// Use this for initialization
	void Start ()
    {
		if (gameMode == GameMode.SinglePlayer)
			mCurrentPlayerId = EPlayerId.PLAYER_ONE;
		else
        	mCurrentPlayerId = EPlayerId.PLAYER_TWO;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void OnAxisChanged(EPlayerId aPlayerId, EAxisData aAxisData, bool aIsAxisActive)
    {
        Debug.Log("Input received for player : " + aPlayerId.Id);

        if (aIsAxisActive == false) return;

		rythmicManager.ProcessInput(aAxisData.AxisName, aPlayerId);
    }

    public EPlayerId CurrentActivePlayer
    {
        get
        {
            return mCurrentPlayerId;
        }
    }

    public void OnStartNextCombo()
    {
        mCurrentPlayerId = EPlayerId.GetNext(mCurrentPlayerId);
        Debug.Log("Starting next combo! Player id : " + mCurrentPlayerId.Id);
    }

    public void OnGameEnd()
    {
        Winner = visualManager.PlayGameEnd();
        Invoke("SwitchToEndGame", 1f);
    }

    private void SwitchToEndGame()
    {
        Application.LoadLevel("endgame");
    }
}
