using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
	public enum controlType { KEYBOARD, XBOX360 };
	public static controlType curControlType;

	public static KeyCode player_left;
	public static KeyCode player_right;
	public static KeyCode player_up;
	public static KeyCode player_down;

	public static KeyCode player_jump;
	public static KeyCode player_attack1;
	public static KeyCode player_attack2;
	public static KeyCode player_dodge;

	//Controls
	public static List<KeyCode> controlsController = new List<KeyCode>(){
		KeyCode.JoystickButton0, //A
		KeyCode.JoystickButton1, //Y
		KeyCode.JoystickButton2, //X
		KeyCode.JoystickButton3, //B
		KeyCode.JoystickButton4, //Left button
		KeyCode.JoystickButton5, //Right button
		KeyCode.JoystickButton6, //Back
		KeyCode.JoystickButton7, //Start
		KeyCode.JoystickButton8, //Left analog
		KeyCode.JoystickButton9, //Right analog
	};

	public static List<KeyCode> controlsKeyboard = new List<KeyCode>(){
		KeyCode.A,
		KeyCode.S,
		KeyCode.D,
		KeyCode.W,
		KeyCode.J,
		KeyCode.K,
		KeyCode.L,
		KeyCode.I,
	};

	// Use this for initialization
	void Start () {
		
	}

	public static void SetDefaultControls_XBox360()
	{
		Debug.Log("Set control type: XBox 360");
		curControlType = controlType.XBOX360;

		player_jump = KeyCode.JoystickButton0;
		player_attack1 = KeyCode.JoystickButton2;
		player_attack2 = KeyCode.JoystickButton1;
		player_dodge = KeyCode.JoystickButton10;
	}

	public static void SetDefaultControls_Keyboard()
	{
		Debug.Log("Set control type: Keyboard");
		curControlType = controlType.KEYBOARD;

		player_left = KeyCode.A;
		player_right = KeyCode.D;
		player_up = KeyCode.W;
		player_down = KeyCode.S;

		player_attack1 = KeyCode.J;
		player_attack2 = KeyCode.K;
		player_jump = KeyCode.I;
		player_dodge = KeyCode.L;
	}

	public static void AutoDetectController()
	{
		string[] joysticks = Input.GetJoystickNames();
		if(joysticks.Length > 0)
		{
			if(joysticks[0].Contains("Xbox 360"))
			{
				SetDefaultControls_XBox360();
			}
		}
	}
}
