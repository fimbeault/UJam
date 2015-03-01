using UnityEngine;
using System.Collections;

public class SplashScreenUI : MonoBehaviour
{
    public GameObject P1P2Ready;
    public GameObject P3Ready;

    public GameObject P1P2Ok;
    public GameObject P3Ok;

    public float _DelayTillLoadGame;

	// Use this for initialization
	void Awake ()
    {
        P1P2Ok.SetActive(false);
        P3Ok.SetActive(false);

        Invoke("StartGame", _DelayTillLoadGame);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void StartGame()
    {
        Application.LoadLevel("main");
    }
}
