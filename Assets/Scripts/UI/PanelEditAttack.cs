using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
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
	public GameObject layoutNextAttack;
	public Button buttonElseAttackNext;

	//Privates
	private DataAttack attack;
	private UIKeyValueSpawner kvSpawner;
	private LinkedList<PanelAttackIfStatement> nextAttackList = new LinkedList<PanelAttackIfStatement>();
	private GameObject nameInputField;

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

	#region ATTACK
	public void LoadAttack(DataAttack da)
	{
		attack = da;
		var type = da.GetType();

		//Create key values
		kvSpawner.Clear();
		nameInputField = kvSpawner.SpawnInputField("Name:", InputField.ContentType.Standard, da.name);
		kvSpawner.SpawnText("Attack type:", da.type);
		kvSpawner.SpawnInputField("Startup time:", InputField.ContentType.DecimalNumber, da.timeStart);
		kvSpawner.SpawnInputField("Recovery time:", InputField.ContentType.DecimalNumber, da.timeEnd);
		kvSpawner.SpawnToggle("Can be first attack:", da.firstAttack);

		if(type == typeof(DataAttackJump))
		{
			DataAttackJump d = (DataAttackJump)da;
			kvSpawner.SpawnSlider("Jump time:", 0.1f, 0.6f, d.jumpTime);
			kvSpawner.SpawnSlider("Jump speed:", 3f, 6f, d.jumpSpeed);
			kvSpawner.SpawnSlider("Fall speed:", 4f, 6f, d.fallSpeed);
			kvSpawner.SpawnSlider("Move speed:", 0f, 0.2f, d.moveSpeed);
			kvSpawner.SpawnListButton("Move approach to player:", M.GetListOfEnum(typeof(Approach)), d.approachToPlayer);
		}
		else if(type == typeof(DataAttackShoot))
		{
			DataAttackShoot d = (DataAttackShoot)da;
			kvSpawner.SpawnListButton("Projectile direction type:", M.GetListOfEnum(typeof(ProjectileDirection)), d.projectileDirection);
			kvSpawner.SpawnSlider("Amount:", 1, 50, d.projectileAmount);
			kvSpawner.SpawnSlider("Size:", 0.05f, 0.5f, d.scale);
			kvSpawner.SpawnSlider("Movement speed:", 0.01f, 0.2f, d.speedMove);
			kvSpawner.SpawnSlider("Homing speed:", 0f, 2f, d.speedRotation);
			kvSpawner.SpawnInputField("Spawn delay:", InputField.ContentType.DecimalNumber, d.spawnDelay);
			kvSpawner.SpawnToggle("Turn towards player:", d.facePlayer);
		}

		//Load Next Attacks
		ClearNextAttacks();
		foreach(DataAttackNext next in da.nextAttacks)
		{
			LoadNextAttackPanel(next);
		}

		var elseAttack = (da.elseNextAttack == null) ? attack : da.elseNextAttack;
		SetElseAttackNext(elseAttack);
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
		//Save values
		kvSpawner.Save();

		//Save next attack
		attack.nextAttacks = new List<DataAttackNext>();
		foreach(PanelAttackIfStatement p in nextAttackList) attack.nextAttacks.Add(p.GetData());

		//Back
		mgPanel.Back();
	}
	#endregion
	#region NEXT ATTACK
	PanelAttackIfStatement CreateNextAttackPanel()
	{
		var g = mgPrefab.SpawnPrefabUI("PanelNextAttack");
		g.transform.SetParent(layoutNextAttack.transform);
		g.transform.SetAsLastSibling();
		g.transform.SetSiblingIndex(g.transform.GetSiblingIndex() - 1);
		g.transform.localScale = new Vector3(1f, 1f, 1f);

		var c = g.GetComponent<PanelAttackIfStatement>();
		c.Init(this, mgDialog);
		nextAttackList.AddLast(c);
		return c;
	}

	public void AddNextAttackPanel()
	{
		CreateNextAttackPanel();
	}

	public void LoadNextAttackPanel(DataAttackNext next)
	{
		var c = CreateNextAttackPanel();
		c.Load(next);
	}

	public void ClearNextAttacks()
	{
		foreach(PanelAttackIfStatement p in nextAttackList)
		{
			Destroy(p.gameObject);
		}

		nextAttackList.Clear();
	}

	public void RemoveNextAttack(PanelAttackIfStatement p)
	{
		nextAttackList.Remove(p);
		Destroy(p.gameObject);
	}

	public void MoveUpNextAttack(PanelAttackIfStatement p)
	{
		if(!M.IsFirstSibling(p.transform)) p.transform.SetSiblingIndex(p.transform.GetSiblingIndex() - 1);
	}

	public void MoveDownNextAttack(PanelAttackIfStatement p)
	{
		if(!M.IsLastSibling(p.transform)) p.transform.SetSiblingIndex(Mathf.Min(p.transform.parent.childCount - 2, p.transform.GetSiblingIndex() + 1));
	}

	public void SetElseAttackNext()
	{
		var list = GetAttackNames();
		var items = new List<DialogManager.ListItem>();
		for(int i = 0; i < list.Count; i++) items.Add(new DialogManager.ListItem(list[i], i));
		mgDialog.DisplayList("", items, delegate(DialogManager.ListItem obj) {
			SetElseAttackNext(GetAttacks().ElementAt(obj.id));
		});
	}

	void SetElseAttackNext(DataAttack da)
	{
		buttonElseAttackNext.GetComponentInChildren<Text>().text = (da == attack) ? "This attack" : da.name.value;
		attack.elseNextAttack = da;
	}
	#endregion

	public DataAttack GetCurrentAttack()
	{
		return attack;
	}

	public string GetCurrentAttackName()
	{
		return nameInputField.GetComponent<InputField>().text;
	}

	public List<DataAttack> GetAttacks()
	{
		return mgEditBoss.GetAttacks();
	}

	public List<string> GetAttackNames()
	{
		var list = new List<string>();
		foreach(DataAttack a in GetAttacks())
		{
			if(a == attack) list.Add("This attack");
			else list.Add(a.name.value);
		}
		return list;
	}
}
