using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(PrefabManager))]
public class PanelEditAttack : MonoBehaviour {

	//Managers
	public PrefabManager mgPrefab;
	public DialogManager mgDialog;
	public PanelManager mgPanel;
	public PanelEditBoss mgEditBoss;

	//UI
	public GameObject layoutValues;
	private List<GameObject> listKeyValue = new List<GameObject>();

	//Privates
	private DataAttack attack;
	private List<Action> saveActions = new List<Action>();

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

	public void LoadAttack(DataAttack da)
	{
		attack = da;
		var type = da.GetType();
		saveActions.Clear(); //prepare for save actions

		//Create key values
		ClearAttackKeyValue();
		SpawnKeyValueInputField("Name:", InputField.ContentType.Standard, da.name);
		SpawnKeyValueText("Attack type:", da.type);
		SpawnKeyValueInputField("Startup time:", InputField.ContentType.DecimalNumber, da.timeStart);
		SpawnKeyValueInputField("Recovery time:", InputField.ContentType.DecimalNumber, da.timeEnd);

		if(type == typeof(DataAttackJump))
		{
			//Create key values
			DataAttackJump d = (DataAttackJump)da;
			SpawnKeyValueSlider("Jump time:", 0.1f, 0.6f, d.jumpTime);
			SpawnKeyValueSlider("Jump speed:", 3f, 6f, d.jumpSpeed);
			SpawnKeyValueSlider("Fall speed:", 4f, 6f, d.fallSpeed);
			SpawnKeyValueSlider("Move speed:", 0f, 0.1f, d.moveSpeed);
			SpawnKeyValueListButton("Move approach to player:", M.GetListOfEnum(typeof(Approach)), d.approachToPlayer);
		}
		else if(type == typeof(DataAttackShoot))
		{
			//Add values for projectile attack
			DataAttackShoot d = (DataAttackShoot)da;
			SpawnKeyValueListButton("Projectile direction type:", M.GetListOfEnum(typeof(ProjectileDirection)), d.projectileDirection);
			SpawnKeyValueSlider("Size:", 0.5f, 1.5f, d.scale);
			SpawnKeyValueSlider("Movement speed:", 0.01f, 0.2f, d.speedMove);
			SpawnKeyValueSlider("Homing speed:", 0f, 2f, d.speedRotation);
			SpawnKeyValueInputField("Projectile amount:", InputField.ContentType.IntegerNumber, d.projectileAmount);
			SpawnKeyValueInputField("Spawn delay:", InputField.ContentType.DecimalNumber, d.spawnDelay);
		}
	}

	public void DeleteAttack()
	{
		mgDialog.DisplayYesNo("Are you sure?",
			delegate {
				mgEditBoss.DeleteAttack(attack);
				mgPanel.Back();
			},
			delegate {
				
			});
	}

	public void SaveAndBack()
	{
		foreach(Action a in saveActions) a();
		mgPanel.Back();
	}

	#region KEYS & VALUES
	void SpawnKeyValueText(string name, DataValue<string> dv)
	{
		AddKeyValue("Attack type:", mgPrefab.SpawnText(dv.value));
	}

	void SpawnKeyValueListButton(string name, List<string> list, DataValue<string> dv)
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
	}

	void SpawnKeyValueInputField(string name, InputField.ContentType type, DataValue<int> dv)
	{
		var g = mgPrefab.SpawnInputField(dv.value.ToString(), type);
		saveActions.Add(delegate { dv.value = GetInputFieldValueInt(g); });
		AddKeyValue(name, g);
	}

	void SpawnKeyValueInputField(string name, InputField.ContentType type, DataValue<float> dv)
	{
		var g = mgPrefab.SpawnInputField(dv.value.ToString(), type);
		saveActions.Add(delegate { dv.value = GetInputFieldValueFloat(g); });
		AddKeyValue(name, g);
	}

	void SpawnKeyValueInputField(string name, InputField.ContentType type, DataValue<string> dv)
	{
		var g = mgPrefab.SpawnInputField(dv.value.ToString(), type);
		saveActions.Add(delegate { dv.value = g.GetComponent<InputField>().text; });
		AddKeyValue(name, g);
	}

	void SpawnKeyValueSlider(string name, float min, float max, DataValue<float> dv)
	{
		var g = mgPrefab.SpawnSlider(min, max, dv.value);
		saveActions.Add(delegate { dv.value = g.GetComponent<Slider>().value; });
		AddKeyValue(name, g);
	}

	void AddKeyValue(string keyText, GameObject valueObject)
	{
		AddChildToLayout(mgPrefab.SpawnText(keyText), layoutValues);
		AddChildToLayout(valueObject, layoutValues);
	}

	void AddChildToLayout(GameObject child, GameObject layout)
	{
		child.transform.SetParent(layout.transform);
		child.transform.localScale = new Vector3(1f, 1f, 1f);
		listKeyValue.Add(child);
	}

	void ClearAttackKeyValue()
	{
		foreach(GameObject g in listKeyValue)
		{
			Destroy(g);
		}
		listKeyValue.Clear();
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
}
