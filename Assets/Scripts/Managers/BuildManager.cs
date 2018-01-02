using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour {

	public BossPicker bossPicker;
	public PanelManager mgPanel;
	private DataManager data;

	private bool unsavedChanges = false;

	//UI elements
	public InputField inputField_name;
	public Slider slider_health;
	public Text text_health;
	public GameObject layout_attacks;

	//Attacks
	private List<DataAttack> attacks;
	private int attackCur;

	//Awake
	void Awake()
	{
		data = GetComponent<DataManager>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadBoss(DataBoss boss)
	{
		inputField_name.text = boss.name;
		slider_health.value = boss.health;

		//Attacks
		if(boss.attacks != null && boss.attacks.Length > 0)
		{
			attacks = new List<DataAttack>(boss.attacks);
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

		DialogManager.Display(
			"Do you want to save?",
			delegate {
				bossPicker.DeleteBoss();
				Save();
				mgPanel.Back();
			},
			delegate {
				DialogManager.Display("Are you sure?",
					delegate {
						mgPanel.Back();
					},
					delegate {
						
					}
				);
			}
		);
	}

	#region ATTACKS
	public void NewAttack()
	{
		//attacks.Add(new DataAttack());
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
			print(type);
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
		DataBoss boss = new DataBoss();
		boss.name = inputField_name.text;
		boss.health = (int)slider_health.value;

		string path = Paths.GetBossDirectory();
		if(!Directory.Exists(path)) Directory.CreateDirectory(path);
		data.SaveToFile(boss, path + "/" + boss.name + ".boss");
	}
	#endregion
	#region ONVALUECHANGED
	public void OnValueChanged()
	{
		unsavedChanges = true;
	}

	public void UpdateTextHealth()
	{
		text_health.text = "" + slider_health.value;
	}
	#endregion
}
