using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameScreenUI : MonoBehaviour
{
    public Image _FearWin;
    public Image _FearLose;

    public Image _DevilWin;
    public Image _DevilLose;

    public float _DelayTillInputReady;

    public float _DelayTillLoadSplash;

    public GameObject _MenuBG;
    public Image _MenuImage;

    private float mAwakeTime;

    private bool mIsLoadingSplash;
    private bool mIsReadyForInput;


    void Awake()
    {
        mAwakeTime = Time.time;

        if (GameManager.Winner == EPlayerId.PLAYER_ONE)
        {
            _FearWin.gameObject.SetActive(true);
            _FearLose.gameObject.SetActive(false);

            _DevilWin.gameObject.SetActive(false);
            _DevilLose.gameObject.SetActive(true);
        }
        else
        {
            _FearWin.gameObject.SetActive(false);
            _FearLose.gameObject.SetActive(true);

            _DevilWin.gameObject.SetActive(true);
            _DevilLose.gameObject.SetActive(false);
        }

        PlayFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mIsReadyForInput && (Time.time - mAwakeTime) >= _DelayTillInputReady)
        {
            mIsReadyForInput = true;
        }

        if (!mIsLoadingSplash && mIsReadyForInput && UJamInputManager.IsAxisActive(EAxisData.START, EPlayerId.PLAYER_TWO) || UJamInputManager.IsAxisActive(EAxisData.START, EPlayerId.PLAYER_ONE))
        {
            ReturnToSplash();
        }
    }
    private void ReturnToSplash()
    {
        mIsLoadingSplash = true;
        _MenuBG.SetActive(true);

        PlayFadeOut();

        Invoke("StartGame", _DelayTillLoadSplash);
    }

    private void PlayFadeIn()
    {
        _MenuBG.SetActive(true);

        _MenuImage.CrossFadeAlpha(0f, 0f, true);
        _FearWin.CrossFadeAlpha(0f, 0f, true);
        _FearLose.CrossFadeAlpha(0f, 0f, true);
        _DevilWin.CrossFadeAlpha(0f, 0f, true);
        _DevilLose.CrossFadeAlpha(0f, 0f, true);

        FadeImage(_MenuImage, 1f);
        FadeImage(_FearWin, 1f);
        FadeImage(_FearLose, 1f);
        FadeImage(_DevilWin, 1f);
        FadeImage(_DevilLose, 1f);

        Invoke("RemoveBG", _DelayTillLoadSplash);
    }

    private void RemoveBG()
    {
        _MenuBG.SetActive(false);
    }

    private void PlayFadeOut()
    {
        _MenuImage.CrossFadeAlpha(1f, 0f, true);
        _FearWin.CrossFadeAlpha(1f, 0f, true);
        _FearLose.CrossFadeAlpha(1f, 0f, true);
        _DevilWin.CrossFadeAlpha(1f, 0f, true);
        _DevilLose.CrossFadeAlpha(1f, 0f, true);

        FadeImage(_MenuImage, 0f);
        FadeImage(_FearWin, 0f);
        FadeImage(_FearLose, 0f);
        FadeImage(_DevilWin, 0f);
        FadeImage(_DevilLose, 0f);
    }

    private void FadeImage(Image aImage, float aAlphaToReach)
    {
        aImage.CrossFadeAlpha(aAlphaToReach, _DelayTillLoadSplash, true);
    }

    void StartGame()
    {
        Application.LoadLevel("splash");
    }
}
