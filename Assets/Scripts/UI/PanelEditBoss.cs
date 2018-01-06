using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.IO;

public enum BuildState { EDIT, CREATE };

[RequireComponent(typeof(DataManager))]
[RequireComponent(typeof(PrefabManager))]
public class PanelEditBoss : MonoBehaviour {

	//Managers
	public DataManager mgData;
	public PrefabManager mgPrefab;
	public DialogManager mgDialog;
	public PanelManager mgPanel;
	public PanelEditAttack mgEditAttack;

	//UI
	public Panel panelEditAttack;
	public GameObject layoutValues;
	public GameObject layoutAttackList;

	//Privates
	private DataBoss saveBoss; //Boss that might be saved
	private DataBoss boss;
	private List<Action<DataBoss>> saveActions = new List<Action<DataBoss>>();

	private BuildState state;
	private List<DataAttack> attacks = new List<DataAttack>();
	private List<GameObject> listAttacks = new List<GameObject>();
	private List<GameObject> listKeyValue = new List<GameObject>();

	//AttackSelection
	private Dictionary<DialogManager.ListItem, DataAttack> dicAttackSelection = new Dictionary<DialogManager.ListItem, DataAttack>();

	//Awake
	void Awake()
	{
		mgPrefab = GetComponent<PrefabManager>();
		mgData = GetComponent<DataManager>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadBoss(DataBoss boss, BuildState state)
	{
		this.boss = boss;
		this.state = state;

		//Create Key&Values
		ClearAttackKeyValue();

		saveActions.Clear();
		var kvName = mgPrefab.SpawnInputField(boss.name, InputField.ContentType.Standard);
		saveActions.Add(delegate(DataBoss obj) { obj.name = kvName.GetComponent<InputField>().text; });
		AddKeyValue("Name:", kvName);

		var kvHealth = mgPrefab.SpawnInputField(boss.health.ToString(), InputField.ContentType.IntegerNumber);
		saveActions.Add(delegate(DataBoss obj) { obj.health = GetInputFieldValueInt(kvHealth); });
		AddKeyValue("Health:", kvHealth);

		//Attacks
		attacks = new List<DataAttack>();
		if(boss.attacks != null && boss.attacks.Length > 0) attacks.AddRange(boss.attacks);
	}

	#region ATTACKS
	public void UpdateAttackList()
	{
		ClearAttacksList();
		foreach(DataAttack attack in attacks)
		{
			AddAttackToList(attack.name);
		}
	}

	void AddAttackToList(string name)
	{
		var g = mgPrefab.SpawnText(name);
		g.transform.SetParent(layoutAttackList.transform);
		g.transform.localScale = new Vector3(1f, 1f, 1f);
		listAttacks.Add(g);
	}

	void ClearAttacksList()
	{
		foreach(GameObject g in listAttacks)
		{
			Destroy(g);
		}
		listAttacks.Clear();
	}
	#endregion
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
	#region BUTTONS
	public void CreateAttack()
	{
		var list = Enum.GetNames(typeof(AttackType)).ToList();
		mgDialog.DisplayButtons("Pick attack type.",
			list,
			CreateAttack
		);
	}

	void CreateAttack(string type)
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
		mgPanel.Forward(panelEditAttack);
		mgEditAttack.LoadAttack(da);
	}

	public void EditAttack()
	{
		mgDialog.DisplayList("Select attack to edit",
			GetAttackItemList(),
			delegate(DialogManager.ListItem obj) {
				EditAttack(dicAttackSelection[obj]);
			}
		);
	}

	void EditAttack(DataAttack da)
	{
		mgPanel.Forward(panelEditAttack);
		mgEditAttack.LoadAttack(da);
	}

	public void DeleteAttack(DataAttack da)
	{
		attacks.Remove(da);
	}

	public void DeleteBoss()
	{
		mgDialog.DisplayYesNo("Are you sure?",
			delegate {
				mgPanel.Back();
				mgData.DeleteFile(boss.GetPath());
			},
			delegate {

			});
	}

	public void SaveAndBack()
	{
		saveBoss = SaveToBoss();
		mgDialog.DisplayYesNo(
			"Do you want to save?",
			delegate {
				string path = Paths.GetBossDirectory();
				if(!Directory.Exists(path)) Directory.CreateDirectory(path);
				if(boss.name != saveBoss.name && File.Exists(saveBoss.GetPath()))
				{
					mgDialog.DisplayYesNo("A boss with that name already exists. Overwrite?",
						delegate {
							Save();
							mgPanel.Back();
						},
						delegate {

						}
					);
				}
				else
				{
					Save();
					mgPanel.Back();
				}
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

	void Save()
	{
		//Delete previous file if editting it
		if(state == BuildState.EDIT) mgData.DeleteFile(saveBoss.GetPath());

		//Save to new file
		mgData.SaveToFile(saveBoss, saveBoss.GetPath());
	}

	DataBoss SaveToBoss()
	{
		var b = new DataBoss();
		foreach(Action<DataBoss> a in saveActions) a(b);
		b.attacks = attacks.ToArray();
		return b;
	}
	#endregion

	List<DialogManager.ListItem> GetAttackItemList()
	{
		dicAttackSelection.Clear();
		var items = new List<DialogManager.ListItem>();
		for(int i = 0; i < attacks.Count; i++)
		{
			var attack = attacks[i];
			var item = new DialogManager.ListItem(attack.name, i);
			dicAttackSelection.Add(item, attack);
			items.Add(item);
		}

		return items;
	}
}
