using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandleValue : MonoBehaviour {

	private Slider slider;
	public Text textValue;

	//Awake
	void Awake()
	{
		slider = GetComponent<Slider>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnValueChange()
	{
		textValue.text = slider.value.ToString();
	}
}
