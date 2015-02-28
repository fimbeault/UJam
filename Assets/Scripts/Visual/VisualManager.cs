using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisualManager : MonoBehaviour
{
    public List<ButtonRenderer> _ButtonRendererList;

	// Use this for initialization
	void Start ()
    {
	    foreach(ButtonRenderer buttonRenderer in _ButtonRendererList)
        {
            buttonRenderer.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void OnAxisChanged(EPlayerId aPlayerId, EAxisData aAxisData, bool aIsAxisActive)
    {
        Debug.Log("OnAxisChanged :: " + aAxisData.GetFullInputName(aPlayerId) + ", IsActive : " + aIsAxisActive);

        ButtonRenderer buttonRenderer = _ButtonRendererList[aPlayerId.InputIndex];
        buttonRenderer.SetSpriteFrame(aAxisData);
        buttonRenderer.gameObject.SetActive(aIsAxisActive);
    }
}
