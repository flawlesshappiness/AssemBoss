using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Controls.SetDefaultControls_Keyboard();
	}
	
	// Update is called once per frame
	void Update () {
		DetectInput();
	}

	void PrintButton(KeyCode k)
	{
		if(Input.GetKeyDown(k))
		{
			print(k.ToString());
		}
	}

	void DetectInput()
	{
		if(Controls.curControlType == Controls.controlType.KEYBOARD)
		{
			DetectController();
		}
		else if(Controls.curControlType == Controls.controlType.XBOX360)
		{
			DetectKeyboard();
		}
	}

	void DetectController()
	{
		foreach(KeyCode k in Controls.controlsController)
		{
			if(Input.GetKey(k)) Controls.AutoDetectController();
		}
	}

	void DetectKeyboard()
	{
		foreach(KeyCode k in Controls.controlsKeyboard)
		{
			if(Input.GetKey(k)) Controls.SetDefaultControls_Keyboard();
		}
	}
}
