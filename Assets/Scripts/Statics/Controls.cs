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
	public static KeyCode player_dodge;

	public static KeyCode player_attack;

	public static KeyCode player_ability;

	// Use this for initialization
	void Start () {
		
	}

	public static void SetDefaultControls_XBox360()
	{
		Debug.Log("Set control type: XBox 360");
		curControlType = controlType.XBOX360;

		player_jump = KeyCode.JoystickButton0; //A
		player_dodge = KeyCode.Joystick8Button9; //Left trigger

		player_attack = KeyCode.JoystickButton2; //X
		//KeyCode.JoystickButton3; //Y
		//KeyCode.JoystickButton1; //B

		player_ability = KeyCode.Joystick8Button10; //Right trigger
	}

	public static void SetDefaultControls_Keyboard()
	{
		Debug.Log("Set control type: Keyboard");
		curControlType = controlType.KEYBOARD;

		player_left = KeyCode.A;
		player_right = KeyCode.D;
		player_up = KeyCode.W;
		player_down = KeyCode.S;

		player_jump = KeyCode.Space;
		player_dodge = KeyCode.LeftShift;

		player_attack = KeyCode.J;

		player_ability = KeyCode.LeftControl;
	}
}
