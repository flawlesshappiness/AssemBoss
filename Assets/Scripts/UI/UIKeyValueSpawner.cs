using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIKeyValueSpawner : MonoBehaviour {

	private PrefabManager mgPrefab;
	private DialogManager mgDialog;
	private List<Action> saveActions = new List<Action>();
	private List<GameObject> listKeyValue = new List<GameObject>();
	private GameObject layout;

	public UIKeyValueSpawner (PrefabManager mgPrefab, DialogManager mgDialog, GameObject layout)
	{
		this.mgPrefab = mgPrefab;
		this.mgDialog = mgDialog;
		this.layout = layout;
	}

	#region SPAWN
	public GameObject SpawnText(string name, DataValue<string> dv)
	{
		var g = mgPrefab.SpawnText(dv.value);
		AddKeyValue("Attack type:", g);
		return g;
	}

	public GameObject SpawnListButton(string name, List<string> list, DataValue<string> dv)
	{
		var g = mgPrefab.SpawnButton(dv.value);
		var t = g.GetComponentInChildren<Text>();
		g.GetComponent<Button>().onClick.AddListener(delegate {
			var items = new List<DialogManager.ListItem>();
			for(int i = 0; i < list.Count; i++) items.Add(new DialogManager.ListItem(list[i], i));
			mgDialog.DisplayList("", items, delegate(DialogManager.ListItem obj) {
				t.text = obj.name;
			});
		});
		saveActions.Add(delegate { dv.value = t.text; });
		AddKeyValue(name, g);
		return g;
	}

	public GameObject SpawnInputField(string name, InputField.ContentType type, DataValue<int> dv)
	{
		var g = mgPrefab.SpawnInputField(dv.value.ToString(), type);
		saveActions.Add(delegate { dv.value = GetInputFieldValueInt(g); });
		AddKeyValue(name, g);
		return g;
	}

	public GameObject SpawnInputField(string name, InputField.ContentType type, DataValue<float> dv)
	{
		var g = mgPrefab.SpawnInputField(dv.value.ToString(), type);
		saveActions.Add(delegate { dv.value = GetInputFieldValueFloat(g); });
		AddKeyValue(name, g);
		return g;
	}

	public GameObject SpawnInputField(string name, InputField.ContentType type, DataValue<string> dv)
	{
		var g = mgPrefab.SpawnInputField(dv.value.ToString(), type);
		saveActions.Add(delegate { dv.value = g.GetComponent<InputField>().text; });
		AddKeyValue(name, g);
		return g;
	}

	public GameObject SpawnSlider(string name, float min, float max, DataValue<float> dv)
	{
		var g = mgPrefab.SpawnSlider(min, max, dv.value, false);
		saveActions.Add(delegate { dv.value = g.GetComponent<Slider>().value; });
		AddKeyValue(name, g);
		return g;
	}

	public GameObject SpawnSlider(string name, int min, int max, DataValue<int> dv)
	{
		var g = mgPrefab.SpawnSlider(min, max, dv.value, true);
		saveActions.Add(delegate { dv.value = (int)g.GetComponent<Slider>().value; });
		AddKeyValue(name, g);
		return g;
	}

	public GameObject SpawnToggle(string name, DataValue<bool> dv)
	{
		var g = mgPrefab.SpawnToggle(dv.value);
		saveActions.Add(delegate { dv.value = g.GetComponent<Toggle>().isOn; });
		AddKeyValue(name, g);
		return g;
	}
	#endregion
	#region PARENTING
	void AddKeyValue(string keyText, GameObject valueObject)
	{
		AddChildToLayout(mgPrefab.SpawnText(keyText), layout);
		AddChildToLayout(valueObject, layout);
	}

	void AddChildToLayout(GameObject child, GameObject layout)
	{
		child.transform.SetParent(layout.transform);
		child.transform.localScale = new Vector3(1f, 1f, 1f);
		listKeyValue.Add(child);
	}
	#endregion
	#region PARSING
	float GetInputFieldValueFloat(GameObject g)
	{
		var ip = g.GetComponent<InputField>();
		if(ip.text == "") return 0f;
		else return float.Parse(ip.text);
	}

	int GetInputFieldValueInt(GameObject g)
	{
		var ip = g.GetComponent<InputField>();
		if(ip.text == "") return 0;
		else return int.Parse(ip.text);
	}
	#endregion

	public void Clear()
	{
		foreach(GameObject g in listKeyValue)
		{
			Destroy(g);
		}
		listKeyValue.Clear();
		saveActions.Clear();
	}

	public void Save()
	{
		foreach(Action a in saveActions) a();
	}
}
