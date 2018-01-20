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
	public GameObject layoutAttackList;
	public GameObject layoutValues;
	private UIKeyValueSpawner kvSpawner;

	//Privates
	private DataBoss boss;
	private string bossName;

	private BuildState state;
	private List<DataAttack> attacks = new List<DataAttack>();
	private List<GameObject> listAttacks = new List<GameObject>();

	//AttackSelection
	private Dictionary<DialogManager.ListItem, DataAttack> dicAttackSelection = new Dictionary<DialogManager.ListItem, DataAttack>();

	//Awake
	void Awake()
	{
		kvSpawner = new UIKeyValueSpawner(mgPrefab, mgDialog, layoutValues);
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
		bossName =  boss.name.value;
		this.state = state;

		//Attacks
		attacks = new List<DataAttack>();
		if(boss.attacks != null && boss.attacks.Length > 0) attacks.AddRange(boss.attacks);
		UpdateAttackList();

		//Create Key&Values
		kvSpawner.Clear();

		kvSpawner.SpawnInputField("Name:", InputField.ContentType.Standard, boss.name);
		kvSpawner.SpawnInputField("Boss health:", InputField.ContentType.IntegerNumber, boss.health);
		kvSpawner.SpawnSlider("Size multiplier:", 0.5f, 3f, boss.sizeMult);
		kvSpawner.SpawnInputField("Player health:", InputField.ContentType.IntegerNumber, boss.player.health);
	}

	public DataAttack GetAttack(int idx)
	{
		return attacks.ElementAt(idx);
	}

	public int GetAttackIndex(DataAttack da)
	{
		return attacks.IndexOf(da);
	}

	public List<DataAttack> GetAttacks()
	{
		return attacks;
	}

	public List<string> GetAttackNames()
	{
		var list = new List<string>();
		foreach(DataAttack a in GetAttacks()) list.Add(a.name.value);
		return list;
	}

	#region ATTACKS
	public void UpdateAttackList()
	{
		ClearAttacksList();
		foreach(DataAttack attack in attacks)
		{
			AddAttackToList(attack.name.value);
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

		da.type.value = type;
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
		mgDialog.DisplayYesNo(
			"Do you want to save?",
			delegate {
				string path = Paths.GetBossDirectory();
				if(!Directory.Exists(path)) Directory.CreateDirectory(path);
				if(File.Exists(boss.GetPath()))
				{
					if(bossName != boss.name.value) //Name was changed but another file exists
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
						if(state == BuildState.EDIT) mgData.DeleteFile(path + "/" + bossName + ".boss");
						Save();
						mgPanel.Back();
					}
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
		//Save boss
		kvSpawner.Save();
		boss.attacks = attacks.ToArray();

		//Save to new file
		mgData.SaveToFile(boss, boss.GetPath());
	}
	#endregion

	List<DialogManager.ListItem> GetAttackItemList()
	{
		dicAttackSelection.Clear();
		var items = new List<DialogManager.ListItem>();
		for(int i = 0; i < attacks.Count; i++)
		{
			var attack = attacks[i];
			var item = new DialogManager.ListItem(attack.name.value, i);
			dicAttackSelection.Add(item, attack);
			items.Add(item);
		}

		return items;
	}
}
