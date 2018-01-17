using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(PrefabManager))]
public class DialogManager : MonoBehaviour {

	private PrefabManager mgPrefab;

	private GameObject curPanel;
	public GameObject background;

	//YesNo
	public GameObject yesNoPanel;
	public Text yesNoTitle;
	private Action yesAction;
	private Action noAction;

	//Buttons
	public GameObject buttonsPanel;
	public Text buttonsTitle;
	public GameObject buttonsLayout;
	private List<GameObject> buttonsChildren = new List<GameObject>();

	//List
	public GameObject listPanel;
	public Text listTitle;
	public GameObject listLayout;
	private Action<ListItem> listSelectAction;
	private List<GameObject> listChildren = new List<GameObject>();
	private ListItem listSelectedItem;
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
		yesNoTitle.text = title;

		//Actions
		this.yesAction = aYes;
		this.noAction = aNo;

		//Display
		Display(yesNoPanel);
	}

	public void ClickButtonYes()
	{
		Close();
		yesAction();
	}

	public void ClickButtonNo()
	{
		Close();
		noAction();
	}
	#endregion
	#region BUTTONS
	public void DisplayButtons(string title, List<string> options, Action<string> action)
	{
		//Title
		buttonsTitle.text = title;

		//Buttons
		foreach(GameObject g in buttonsChildren) Destroy(g);
		buttonsChildren.Clear();
		foreach(string s in options)
		{
			var b = SpawnButton(s, buttonsLayout.transform);
			b.onClick.AddListener(delegate {
				action(s);
				Close();
			});
		}

		var bCancel = SpawnButton("Cancel", buttonsLayout.transform);
		bCancel.onClick.AddListener(delegate {
			Close();
		});


		//Display
		Display(buttonsPanel);
	}

	Button SpawnButton(string text, Transform parent)
	{
		var g = mgPrefab.SpawnPrefabUI("Button");
		buttonsChildren.Add(g);
		g.transform.SetParent(parent);
		g.transform.localScale = new Vector3(1f, 1f, 1f);
		g.GetComponentInChildren<Text>().text = text;
		return g.GetComponent<Button>();
	}
	#endregion
	#region LIST
	public void DisplayList(string title, List<DialogManager.ListItem> items, Action<ListItem> action)
	{
		//Title
		listTitle.text = title;

		//Items
		foreach(GameObject g in listChildren) Destroy(g);
		listChildren.Clear();
		foreach(ListItem i in items)
		{
			var g = mgPrefab.SpawnPrefabUI("ButtonList");
			listChildren.Add(g);
			g.transform.SetParent(listLayout.transform);
			g.transform.localScale = new Vector3(1f, 1f, 1f);
			g.GetComponentInChildren<Text>().text = i.name;
			var b = g.GetComponent<Button>();
			b.onClick.AddListener(delegate {
				if(listSelectedButton != null) listSelectedButton.interactable = true;
				listSelectedButton = b;
				listSelectedButton.interactable = false;
				listSelectedItem = i;
			});
		}
		listSelectAction = action;

		//Display
		Display(listPanel);
	}

	public void ClickButtonListSelect()
	{
		if(listSelectedItem != null)
		{
			listSelectAction(listSelectedItem);
			listSelectedItem = null;
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
}
