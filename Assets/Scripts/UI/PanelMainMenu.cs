using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;

[RequireComponent(typeof(DataManager))]
public class PanelMainMenu : MonoBehaviour {

	private DataManager mgData;
	public LevelManager mgLevel;
	public PanelEditBoss mgEditBoss;
	public DialogManager mgDialog;
	public PanelManager mgPanel;
	public Panel panelFight;
	public Panel panelBuild;

	//BossSelection
	private Dictionary<DialogManager.ListItem, DataBoss> dicBossSelection = new Dictionary<DialogManager.ListItem, DataBoss>();

	//Awake
	void Awake()
	{
		mgData = GetComponent<DataManager>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FightBoss()
	{
		mgDialog.DisplayList("Select boss to fight",
			GetBossItemList(),
			delegate(DialogManager.ListItem obj) {
				FightBoss(dicBossSelection[obj]);
			}
		);
	}

	void FightBoss(DataBoss boss)
	{
		mgPanel.Forward(panelFight);
		mgLevel.StartLevel(boss);
	}

	public void EditBoss()
	{
		mgDialog.DisplayList("Select boss to edit",
			GetBossItemList(),
			delegate(DialogManager.ListItem obj) {
				EditBoss(dicBossSelection[obj]);
			}
		);
	}

	void EditBoss(DataBoss boss)
	{
		mgPanel.Forward(panelBuild);
		mgEditBoss.LoadBoss(boss, BuildState.EDIT);
	}

	public void CreateBoss()
	{
		mgPanel.Forward(panelBuild);
		mgEditBoss.LoadBoss(new DataBoss(), BuildState.CREATE);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	List<DialogManager.ListItem> GetBossItemList()
	{
		dicBossSelection.Clear();
		var items = new List<DialogManager.ListItem>();
		var files = GetBossFiles();
		for(int i = 0; i < files.Length; i++)
		{
			var boss = GetBoss(files[i]);
			if(boss != null)
			{
				var item = new DialogManager.ListItem(boss.name.value, i);
				dicBossSelection.Add(item, boss);
				items.Add(item);
			}
		}

		return items;
	}

	#region FILES
	FileInfo[] GetBossFiles()
	{
		string path = Paths.GetBossDirectory();
		if(Directory.Exists(path))
		{
			var info = new DirectoryInfo(Paths.GetBossDirectory());
			return info.GetFiles("*.boss");
		}
		else return new FileInfo[0];
	}

	DataBoss GetBoss(FileInfo file)
	{
		return mgData.LoadFromFile<DataBoss>(file.FullName);
	}
	#endregion
}
