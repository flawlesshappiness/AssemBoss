using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class PrefabManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject SpawnPrefabGame(string prefab)
	{
		return GameObject.Instantiate(Resources.Load("Prefabs/Game/"+prefab)) as GameObject;
	}

	public GameObject SpawnPrefabUI(string prefab)
	{
		return GameObject.Instantiate(Resources.Load("Prefabs/UI/"+prefab)) as GameObject;
	}

	#region UI
	public GameObject SpawnInputField(string value, InputField.ContentType type)
	{
		GameObject g = SpawnPrefabUI("InputField");
		var t = g.GetComponent<InputField>();
		t.text = value;
		t.contentType = type;

		return g;
	}

	public GameObject SpawnText(string text)
	{
		var g = SpawnPrefabUI("Text");
		var t = g.GetComponent<Text>();
		t.text = text;
		return g;
	}

	public GameObject SpawnButton(string text)
	{
		var g = SpawnPrefabUI("Button");
		var t = g.GetComponentInChildren<Text>();
		t.text = text;
		return g;
	}

	public GameObject SpawnSlider(float minVal, float maxVal, float val)
	{
		var g = SpawnPrefabUI("Slider");
		var s = g.GetComponent<Slider>();
		s.minValue = minVal;
		s.maxValue = maxVal;
		s.value = val;
		s.wholeNumbers = false;
		return g;
	}

	public GameObject SpawnDropdown(List<string> options, int value)
	{
		var g = SpawnPrefabUI("Dropdown");
		var d = g.GetComponent<Dropdown>();
		d.AddOptions(options);
		return g;
	}

	public GameObject SpawnDropdown(System.Type enumType, int value)
	{
		return SpawnDropdown(M.GetListOfEnum(enumType), value);
	}
	#endregion
}
