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
	private UIKeyValueSpawner kvSpawner;

	//Privates
	private DataAttack attack;

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

	public void LoadAttack(DataAttack da)
	{
		attack = da;
		var type = da.GetType();

		//Create key values
		kvSpawner.Clear();
		kvSpawner.SpawnInputField("Name:", InputField.ContentType.Standard, da.name);
		kvSpawner.SpawnText("Attack type:", da.type);
		kvSpawner.SpawnToggle("Can be randomly used:", da.activeAttack);
		kvSpawner.SpawnInputField("Startup time:", InputField.ContentType.DecimalNumber, da.timeStart);
		kvSpawner.SpawnInputField("Recovery time:", InputField.ContentType.DecimalNumber, da.timeEnd);

		if(type == typeof(DataAttackJump))
		{
			DataAttackJump d = (DataAttackJump)da;
			kvSpawner.SpawnSlider("Jump time:", 0.1f, 0.6f, d.jumpTime);
			kvSpawner.SpawnSlider("Jump speed:", 3f, 6f, d.jumpSpeed);
			kvSpawner.SpawnSlider("Fall speed:", 4f, 6f, d.fallSpeed);
			kvSpawner.SpawnSlider("Move speed:", 0f, 0.1f, d.moveSpeed);
			kvSpawner.SpawnListButton("Move approach to player:", M.GetListOfEnum(typeof(Approach)), d.approachToPlayer);
		}
		else if(type == typeof(DataAttackShoot))
		{
			DataAttackShoot d = (DataAttackShoot)da;
			kvSpawner.SpawnListButton("Projectile direction type:", M.GetListOfEnum(typeof(ProjectileDirection)), d.projectileDirection);
			kvSpawner.SpawnSlider("Size:", 0.5f, 1.5f, d.scale);
			kvSpawner.SpawnSlider("Movement speed:", 0.01f, 0.2f, d.speedMove);
			kvSpawner.SpawnSlider("Homing speed:", 0f, 2f, d.speedRotation);
			kvSpawner.SpawnInputField("Projectile amount:", InputField.ContentType.IntegerNumber, d.projectileAmount);
			kvSpawner.SpawnInputField("Spawn delay:", InputField.ContentType.DecimalNumber, d.spawnDelay);
		}
		else if(type == typeof(DataAttackSequence))
		{
			DataAttackSequence d = (DataAttackSequence)da;

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
		kvSpawner.Save();
		mgPanel.Back();
	}
}
