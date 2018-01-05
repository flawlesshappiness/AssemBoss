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
	private List<GameObject> childrenButtons = new List<GameObject>();

	//List
	public GameObject panelList;
	public Text titleList;
	public GameObject layoutList;
	private Action<ListItem> aListSelect;
	private List<GameObject> childrenList = new List<GameObject>();
	private ListItem selectedListItem;
	private Button listSelectedButton;

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
	#region BUTTONS
	public void DisplayButtons(string title, List<string> options, Action<string> action)
	{
		//Title
		titleButtons.text = title;

		//Buttons
		foreach(GameObject g in childrenButtons) Destroy(g);
		childrenButtons.Clear();
		foreach(string s in options)
		{
			var b = SpawnButton(s, layoutButtons.transform);
			b.onClick.AddListener(delegate {
				action(s);
				Close();
			});
		}

		var bCancel = SpawnButton("Cancel", layoutButtons.transform);
		bCancel.onClick.AddListener(delegate {
			Close();
		});


		//Display
		Display(panelButtons);
	}
	#endregion
	#region LIST
	public class ListItem
	{
		public string name;
		public int id;

		public ListItem (string name, int id)
		{
			this.name = name;
			this.id = id;
		}
	}

	public void DisplayList(string title, List<DialogManager.ListItem> items, Action<ListItem> action)
	{
		//Title
		titleList.text = title;

		//Items
		foreach(GameObject g in childrenList) Destroy(g);
		childrenList.Clear();
		foreach(ListItem i in items)
		{
			var g = mgPrefab.SpawnPrefabUI("ButtonList");
			childrenList.Add(g);
			g.transform.SetParent(layoutList.transform);
			g.transform.localScale = new Vector3(1f, 1f, 1f);
			g.GetComponentInChildren<Text>().text = i.name;
			var b = g.GetComponent<Button>();
			b.onClick.AddListener(delegate {
				if(listSelectedButton != null) listSelectedButton.interactable = true;
				listSelectedButton = b;
				listSelectedButton.interactable = false;
				selectedListItem = i;
			});
			aListSelect = action;
		}

		//Display
		Display(panelList);
	}

	public void ClickButtonListSelect()
	{
		if(selectedListItem != null)
		{
			aListSelect(selectedListItem);
			selectedListItem = null;
			Close();
		}
	}
	#endregion

	void Display(GameObject panel)
	{
		if(curPanel != null) curPanel.SetActive(false);
		curPanel = panel;
		curPanel.SetActive(true);

		background.SetActive(true);
	}

	public void Close()
	{
		curPanel.SetActive(false);
		background.SetActive(false);
	}

	Button SpawnButton(string text, Transform parent)
	{
		var g = mgPrefab.SpawnPrefabUI("Button");
		childrenButtons.Add(g);
		g.transform.SetParent(parent);
		g.transform.localScale = new Vector3(1f, 1f, 1f);
		g.GetComponentInChildren<Text>().text = text;
		return g.GetComponent<Button>();
	}
}
