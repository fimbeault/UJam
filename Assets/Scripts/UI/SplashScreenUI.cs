using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreenUI : MonoBehaviour
{
    public Image P1P2Ready;
    public Image P3Ready;

    public Image P1P2Ok;
    public Image P3Ok;

    public float _DelayTillLoadGame;
    public float _OnLoadGameAlpha;

    public Image _MenuImage;
    public GameObject _MenuBG;

    private bool mP1P2Validated;
    private bool mP3Validated;

	// Use this for initialization
	void Awake ()
    {
        P1P2Ok.gameObject.SetActive(false);
        P3Ok.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (!mP1P2Validated && UJamInputManager.IsAxisActive(EAxisData.START, EPlayerId.PLAYER_ONE))
        {
            OnP1P2Validated();
        }
        if (!mP3Validated && UJamInputManager.IsAxisActive(EAxisData.START, EPlayerId.PLAYER_TWO))
        {
            OnP3Validated();
        }
	}

    private void OnP1P2Validated()
    {
        P1P2Ready.gameObject.SetActive(false);
        P1P2Ok.gameObject.SetActive(true);
        mP1P2Validated = true;

        CheckStartGame();
    }

    private void OnP3Validated()
    {
        P3Ready.gameObject.SetActive(false);
        P3Ok.gameObject.SetActive(true);
        mP3Validated = true;

        CheckStartGame();
    }

    private void CheckStartGame()
    {
        if (mP1P2Validated && mP3Validated)
        {
            _MenuBG.SetActive(true);
            _MenuImage.CrossFadeAlpha(_OnLoadGameAlpha, _DelayTillLoadGame, false);
            P1P2Ok.CrossFadeAlpha(_OnLoadGameAlpha, _DelayTillLoadGame, false);
            P3Ok.CrossFadeAlpha(_OnLoadGameAlpha, _DelayTillLoadGame, false);

            Invoke("StartGame", _DelayTillLoadGame);
        }
    }

    void StartGame()
    {
        Application.LoadLevel("main");
    }
}
