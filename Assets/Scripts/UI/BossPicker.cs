using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DataManager))]
public class BossPicker : MonoBehaviour {

	public LevelManager mgLevel;
	public BuildManager mgBuild;
	private DataManager mgData;

	private FileInfo[] files;
	private int fileCur;
	private DataBoss bossCur;

	//UI elements
	public GameObject layoutDisplay;

	public Button bEdit;
	public Button bDelete;
	public Button bFight;

	public Text text_name;
	public Text text_health;

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

	public void OnForwardedTo()
	{
		files = GetBossFiles();
		if(files.Length > 0)
		{
			if(fileCur >= files.Length) fileCur = 0;
			bossCur = GetBoss(files[fileCur]);
			DisplayBoss(bossCur);
		}
		else
		{
			DisplayNothing();
		}
	}

	public void ChangeBoss(bool forward)
	{
		fileCur += forward ? 1 : -1;
		if(fileCur < 0) fileCur = files.Length - 1;
		else if(fileCur >= files.Length) fileCur = 0;

		bossCur = GetBoss(files[fileCur]);
		DisplayBoss(bossCur);
	}

	void DisplayBoss(DataBoss boss)
	{
		//Enable UI
		layoutDisplay.SetActive(true);
		bFight.interactable = true;
		bEdit.interactable = true;
		bDelete.interactable = true;

		//Set values
		text_name.text = boss.name;
		text_health.text = boss.health.ToString();
	}

	void DisplayNothing()
	{
		//Disable UI
		layoutDisplay.SetActive(false);
		bFight.interactable = false;
		bEdit.interactable = false;
		bDelete.interactable = false;
	}

	public void FightBoss()
	{
		mgLevel.StartLevel(bossCur);
	}

	public void EditBoss()
	{
		mgBuild.LoadBoss(bossCur);
	}

	public void NewBoss()
	{
		bossCur = null;
		mgBuild.LoadBoss(new DataBoss());
	}

	public void DeleteBoss()
	{
		if(bossCur == null || files.Length == 0) return; //If no files, do nothing

		mgData.DeleteFile(files[fileCur].FullName);
		files = GetBossFiles();
		if(files.Length > 0) ChangeBoss(false);
		else DisplayNothing();
	}
}
