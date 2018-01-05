using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public enum AttackType { JUMP, PROJECTILE }

[RequireComponent(typeof(PrefabManager))]
public class BuildManager : MonoBehaviour {

	public BossPicker bossPicker;
	public PanelManager mgPanel;
	public DialogManager mgDialog;
	private PrefabManager mgPrefab;
	private DataManager data;

	private bool unsavedChanges = false;
	private DataBoss boss;

	//UI elements
	public InputField inputField_name;
	public InputField inputField_health;
	public GameObject layout_attacks;

	public GameObject layout_attack_keys;
	public GameObject layout_attack_values;
	public GameObject list_attacks;

	//Attacks
	private List<DataAttack> attacks;
	private int attackCur;
	private List<GameObject> attackKeyValueList = new List<GameObject>();

	//Awake
	void Awake()
	{
		data = GetComponent<DataManager>();
		mgPrefab = GetComponent<PrefabManager>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadBoss(DataBoss boss)
	{
		this.boss = boss;
		inputField_name.text = boss.name;
		inputField_health.text = boss.health.ToString();

		//Attacks
		attacks = new List<DataAttack>();
		if(boss.attacks != null && boss.attacks.Length > 0)
		{
			attacks.AddRange(boss.attacks);
			attackCur = 0;
			DisplayAttack(attacks[attackCur]);
		}
		else
		{
			DisplayAttack(null);
		}

		unsavedChanges = false;
	}

	public void BackSave()
	{
		if(!unsavedChanges)
		{
			mgPanel.Back();
			return;
		}

		mgDialog.DisplayYesNo(
			"Do you want to save?",
			delegate {
				bossPicker.DeleteBoss();
				Save();
				mgPanel.Back();
			},
			delegate {
				mgDialog.DisplayYesNo("Are you sure?",
					delegate {
						mgPanel.Back();
					},
					delegate {
						
					}
				);
			}
		);
	}

	#region UI
	GameObject SpawnInputField(string value, AttackValueType type)
	{
		GameObject g = mgPrefab.SpawnPrefabUI("InputField");
		var t = g.GetComponent<InputField>();
		t.text = value;

		if(type == AttackValueType.INT) t.contentType = InputField.ContentType.IntegerNumber;
		else if(type == AttackValueType.STRING) t.contentType = InputField.ContentType.Standard;

		return g;
	}

	GameObject SpawnText(string text)
	{
		var g = mgPrefab.SpawnPrefabUI("Text");
		var t = g.GetComponent<Text>();
		t.text = text;
		return g;
	}

	GameObject SpawnButton(string text)
	{
		var g = mgPrefab.SpawnPrefabUI("Button");
		var t = g.GetComponentInChildren<Text>();
		t.text = text;
		return g;
	}
	#endregion

	#region ATTACKS
	private enum AttackValueType { STRING, INT }

	public void LoadAttacks()
	{
		foreach(DataAttack da in attacks)
		{
			var g = SpawnButton(da.name);
			g.transform.SetParent(list_attacks.transform);
			g.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public void NewAttack()
	{
		var list = Enum.GetNames(typeof(AttackType)).ToList();
		mgDialog.DisplayButtons("Pick attack type.",
			list,
			AddAttack
		);
	}

	void AddAttack(string type)
	{
		DataAttack da = null;
		switch(type)
		{
		case "JUMP":
			da = new DataAttackJump();
			break;
		case "PROJECTILE":
			da = new DataAttackShoot();
			break;
		default:
			break;
		}

		da.type = type;
		attacks.Add(da);
		attackCur = attacks.Count - 1;
		DisplayAttack(attacks[attackCur]);
	}

	void AddAttackKey(GameObject g)
	{
		g.transform.SetParent(layout_attack_keys.transform);
		g.transform.localScale = new Vector3(1f, 1f, 1f);
		attackKeyValueList.Add(g);
	}

	void AddAttackValue(GameObject g)
	{
		g.transform.SetParent(layout_attack_values.transform);
		g.transform.localScale = new Vector3(1f, 1f, 1f);
		attackKeyValueList.Add(g);
	}

	void ClearAttackKeyValue()
	{
		foreach(GameObject g in attackKeyValueList)
		{
			Destroy(g);
		}
		attackKeyValueList.Clear();
	}

	public void DeleteAttack()
	{
		var attack = attacks[attackCur];
		attacks.Remove(attack);
		if(attacks.Count <= 0) DisplayAttack(null);
		else DisplayAttack(attacks[--attackCur]);
	}

	public void DisplayAttack(DataAttack da)
	{
		if(da == null)
		{
			layout_attacks.SetActive(false);
		}
		else
		{
			layout_attacks.SetActive(true);
			var type = da.GetType();

			ClearAttackKeyValue();
			//Keys and values
			AddAttackKey(SpawnText("Type:"));
			AddAttackValue(SpawnText(da.type));
			AddAttackKey(SpawnText("Name:"));
			AddAttackValue(SpawnInputField("Attack Name", AttackValueType.STRING));
			if(type == typeof(DataAttackJump))
			{
				//Add values for jump attack
			}
			else if(type == typeof(DataAttackShoot))
			{
				//Add values for projectile attack
			}
		}
	}

	public void ChangeAttack(bool forward)
	{
		attackCur += forward ? 1 : -1;
		if(attackCur < 0) attackCur = attacks.Count - 1;
		else if(attackCur >= attacks.Count) attackCur = 0;

		DisplayAttack(attacks[attackCur]);
	}
	#endregion

	#region SAVE
	public void Save()
	{
		//Save boss values
		boss.name = inputField_name.text;
		boss.health = int.Parse(inputField_health.text);

		//Save attacks
		SaveAttacks();

		string path = Paths.GetBossDirectory();
		if(!Directory.Exists(path)) Directory.CreateDirectory(path);
		data.SaveToFile(boss, path + "/" + boss.name + ".boss");
	}

	void SaveAttacks()
	{
		
		boss.attacks = attacks.ToArray();
	}
	#endregion
	#region ONVALUECHANGED
	public void OnValueChanged()
	{
		unsavedChanges = true;
	}
	#endregion
}
