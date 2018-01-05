using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(PrefabManager))]
public class PanelEditAttack : MonoBehaviour {

	//Managers
	private PrefabManager mgPrefab;
	public DialogManager mgDialog;
	public PanelManager mgPanel;
	public PanelEditBoss mgEditBoss;

	//UI
	public GameObject layoutKeys;
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
		var kvName = mgPrefab.SpawnInputField(da.name, InputField.ContentType.Standard);
		saveActions.Add(delegate { attack.name = kvName.GetComponent<InputField>().text; });
		var kvType = mgPrefab.SpawnText(da.type);
		saveActions.Add(delegate { attack.type = kvType.GetComponent<Text>().text; });
		var kvTimeStart = mgPrefab.SpawnInputField(da.timeStart.ToString(), InputField.ContentType.DecimalNumber);
		saveActions.Add(delegate { attack.timeStart = float.Parse(kvTimeStart.GetComponent<InputField>().text); });
		var kvTimeEnd = mgPrefab.SpawnInputField(da.timeEnd.ToString(), InputField.ContentType.DecimalNumber);
		saveActions.Add(delegate { attack.timeEnd = float.Parse(kvTimeEnd.GetComponent<InputField>().text); });

		//Add key values
		ClearAttackKeyValue();
		AddKeyValue("Name:", kvName);
		AddKeyValue("Type:", kvType);
		AddKeyValue("Startup time:", kvTimeStart);
		AddKeyValue("Recovery time:", kvTimeEnd);

		if(type == typeof(DataAttackJump))
		{
			//Create key values
			DataAttackJump daj = (DataAttackJump)da;
			var kvJumpTime = mgPrefab.SpawnSlider(0.1f, 3f, daj.jumpTime);
			saveActions.Add(delegate { daj.jumpTime = kvJumpTime.GetComponent<Slider>().value; });
			var kvJumpSpeed = mgPrefab.SpawnSlider(0.01f, 0.1f, daj.jumpSpeed);
			saveActions.Add(delegate { daj.jumpSpeed = kvJumpSpeed.GetComponent<Slider>().value; });
			var kvFallSpeed = mgPrefab.SpawnSlider(0.01f, 0.1f, daj.fallSpeed);
			saveActions.Add(delegate { daj.fallSpeed = kvFallSpeed.GetComponent<Slider>().value; });

			//Add key values
			AddKeyValue("Jump time:", kvJumpTime);
			AddKeyValue("Jump speed:", kvJumpSpeed);
			AddKeyValue("Fall speed:", kvFallSpeed);
		}
		else if(type == typeof(DataAttackShoot))
		{
			//Add values for projectile attack
		}
	}

	public void DeleteAttack()
	{
		mgPanel.Back();
		mgEditBoss.DeleteAttack(attack);
	}

	public void SaveAndBack()
	{
		foreach(Action a in saveActions) a();
		mgPanel.Back();
	}

	#region KEYS & VALUES
	void AddKeyValue(string keyText, GameObject valueObject)
	{
		AddChildToLayout(mgPrefab.SpawnText(keyText), layoutKeys);
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
}
