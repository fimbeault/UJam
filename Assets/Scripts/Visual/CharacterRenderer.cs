using UnityEngine;
using System.Collections;

public class CharacterRenderer : MonoBehaviour
{
    private const string PLAY_WIN_STATE = "PlayWinState";

    private Animator mCharacterAnimator;
    public RythmicManager _RythmicManager;

	// Use this for initialization
	void Start ()
    {
        mCharacterAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void PlayWinAnim()
    {
        mCharacterAnimator.SetTrigger(PLAY_WIN_STATE);
    }
}
