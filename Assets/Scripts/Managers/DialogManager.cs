using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

	public Text textTitle;
	public Button buttonYes;
	public Button buttonNo;
	private static Text tTitle;
	private static Button bYes;
	private static Button bNo;
	private static GameObject panel;

	//Awake
	void Awake()
	{
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init()
	{
		if(tTitle == null) tTitle = textTitle;
		if(bYes == null) bYes = buttonYes;
		if(bNo == null) bNo = buttonNo;
		if(panel == null) panel = gameObject;

		Close();
	}

	public static void Display(string title, Action aYes, Action aNo)
	{
		//Title
		tTitle.text = title;

		//Buttons
		bYes.onClick.RemoveAllListeners();
		bNo.onClick.RemoveAllListeners();
		bYes.onClick.AddListener(() => Close() );
		bNo.onClick.AddListener(() => Close() );

		bYes.onClick.AddListener(() => aYes() );
		bNo.onClick.AddListener(() => aNo() );

		//Display
		panel.SetActive(true);
	}

	static void Close()
	{
		panel.SetActive(false);
	}
}
