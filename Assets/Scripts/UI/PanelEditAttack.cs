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
		var kvName = mgPrefab.SpawnInputField(da.name, InputField.ContentType.Standard);
		saveActions.Add(delegate { attack.name = kvName.GetComponent<InputField>().text; });
		AddKeyValue("Name:", kvName);

		var kvType = mgPrefab.SpawnText(da.type);
		saveActions.Add(delegate { attack.type = kvType.GetComponent<Text>().text; });
		AddKeyValue("Attack type:", kvType);

		var kvTimeStart = mgPrefab.SpawnInputField(da.timeStart.ToString(), InputField.ContentType.DecimalNumber);
		saveActions.Add(delegate { attack.timeStart = GetInputFieldValueFloat(kvTimeStart); });
		AddKeyValue("Startup time:", kvTimeStart);

		var kvTimeEnd = mgPrefab.SpawnInputField(da.timeEnd.ToString(), InputField.ContentType.DecimalNumber);
		saveActions.Add(delegate { attack.timeEnd = GetInputFieldValueFloat(kvTimeEnd); });
		AddKeyValue("Recovery time:", kvTimeEnd);

		if(type == typeof(DataAttackJump))
		{
			//Create key values
			DataAttackJump d = (DataAttackJump)da;
			var kvJumpTime = mgPrefab.SpawnSlider(0.1f, 0.6f, d.jumpTime);
			saveActions.Add(delegate { d.jumpTime = kvJumpTime.GetComponent<Slider>().value; });
			AddKeyValue("Jump time:", kvJumpTime);

			var kvJumpSpeed = mgPrefab.SpawnSlider(3f, 6f, d.jumpSpeed);
			saveActions.Add(delegate { d.jumpSpeed = kvJumpSpeed.GetComponent<Slider>().value; });
			AddKeyValue("Jump speed:", kvJumpSpeed);

			var kvFallSpeed = mgPrefab.SpawnSlider(4f, 6f, d.fallSpeed);
			saveActions.Add(delegate { d.fallSpeed = kvFallSpeed.GetComponent<Slider>().value; });
			AddKeyValue("Fall speed:", kvFallSpeed);

			var kvMoveSpeed = mgPrefab.SpawnSlider(0.0f, 0.1f, d.moveSpeed);
			saveActions.Add(delegate { d.moveSpeed = kvMoveSpeed.GetComponent<Slider>().value; });
			AddKeyValue("Move speed:", kvMoveSpeed);

			var kvMoveApproach = mgPrefab.SpawnButton(d.approachToPlayer);
			var kvMoveApproachT = kvMoveApproach.GetComponentInChildren<Text>();
			kvMoveApproach.GetComponent<Button>().onClick.AddListener(delegate {
				var list = M.GetListOfEnum(typeof(Approach));
				var items = new List<DialogManager.ListItem>();
				for(int i = 0; i < list.Count; i++) items.Add(new DialogManager.ListItem(list[i], i));
				mgDialog.DisplayList("Select approach", items, delegate(DialogManager.ListItem obj) {
					kvMoveApproachT.text = obj.name;
				});
			});
			saveActions.Add(delegate { d.approachToPlayer = kvMoveApproachT.text; });
			AddKeyValue("Move approach to player:", kvMoveApproach);
		}
		else if(type == typeof(DataAttackShoot))
		{
			//Add values for projectile attack
			DataAttackShoot d = (DataAttackShoot)da;

			var kvProjectileDir = mgPrefab.SpawnButton(d.projectileDirection);
			var kvProjectileDirT = kvProjectileDir.GetComponentInChildren<Text>();
			kvProjectileDir.GetComponent<Button>().onClick.AddListener(delegate {
				var list = M.GetListOfEnum(typeof(ProjectileDirection));
				var items = new List<DialogManager.ListItem>();
				for(int i = 0; i < list.Count; i++) items.Add(new DialogManager.ListItem(list[i], i));
				mgDialog.DisplayList("Select direction type", items, delegate(DialogManager.ListItem obj) {
					kvProjectileDirT.text = obj.name;
				});
			});
			saveActions.Add(delegate { d.projectileDirection = kvProjectileDirT.text; });
			AddKeyValue("Projectile direction type:", kvProjectileDir);

			var kvScale = mgPrefab.SpawnSlider(0.5f, 1.5f, d.scale);
			saveActions.Add(delegate { d.scale = kvScale.GetComponent<Slider>().value; });
			AddKeyValue("Size:", kvScale);

			var kvSpeed = mgPrefab.SpawnSlider(0.01f, 0.2f, d.speedMove);
			saveActions.Add(delegate { d.speedMove = kvSpeed.GetComponent<Slider>().value; });
			AddKeyValue("Movement speed:", kvSpeed);

			var kvRotation = mgPrefab.SpawnSlider(0.0f, 2f, d.speedRotation);
			saveActions.Add(delegate { d.speedRotation = kvRotation.GetComponent<Slider>().value; });
			AddKeyValue("Homing speed:", kvRotation);

			var kvAmount = mgPrefab.SpawnInputField(d.projectileAmount.ToString(), InputField.ContentType.IntegerNumber);
			saveActions.Add(delegate { d.projectileAmount = GetInputFieldValueInt(kvAmount); });
			AddKeyValue("Projectile amount:", kvAmount);

			var kvSpawnDelay = mgPrefab.SpawnInputField(d.spawnDelay.ToString(), InputField.ContentType.DecimalNumber);
			saveActions.Add(delegate { d.spawnDelay = GetInputFieldValueFloat(kvSpawnDelay); });
			AddKeyValue("Spawn delay:", kvSpawnDelay);
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
