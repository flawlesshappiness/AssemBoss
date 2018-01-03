using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(PrefabManager))]
public class DialogManager : MonoBehaviour {

	private PrefabManager mgPrefab;

	private GameObject curPanel;
	public GameObject background;

	//YesNo
	public GameObject panelYesNo;
	public Text titleYesNo;
	private Action aYes;
	private Action aNo;

	//Buttons
	public GameObject panelButtons;
	public Text titleButtons;
	public GameObject layoutButtons;
	private List<GameObject> buttons = new List<GameObject>();

	//Awake
	void Awake()
	{
		mgPrefab = GetComponent<PrefabManager>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region YESNO
	public void DisplayYesNo(string title, Action aYes, Action aNo)
	{
		//Title
		titleYesNo.text = title;

		//Actions
		this.aYes = aYes;
		this.aNo = aNo;

		//Display
		Display(panelYesNo);
	}

	public void ClickButtonYes()
	{
		Close();
		aYes();
	}

	public void ClickButtonNo()
	{
		Close();
		aNo();
	}
	#endregion

	#region DROP
	public void DisplayButtons(string title, List<string> options, Action<string> action)
	{
		//Title
		titleButtons.text = title;

		//Buttons
		foreach(GameObject g in buttons) Destroy(g);
		buttons.Clear();
		foreach(string s in options)
		{
			var g = mgPrefab.SpawnPrefabUI("Button");
			buttons.Add(g);
			g.transform.SetParent(layoutButtons.transform);
			g.transform.localScale = new Vector3(1f, 1f, 1f);
			g.GetComponentInChildren<Text>().text = s;
			g.GetComponent<Button>().onClick.AddListener(delegate {
				action(s);
				Close();
			});
		}

		//Display
		Display(panelButtons);
	}
	#endregion

	void Display(GameObject panel)
	{
		if(curPanel != null) curPanel.SetActive(false);
		curPanel = panel;
		curPanel.SetActive(true);

		background.SetActive(true);
	}

	void Close()
	{
		curPanel.SetActive(false);
		background.SetActive(false);
	}
}
