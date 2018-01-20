using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PanelGame : MonoBehaviour {

	public Slider sliderHealthBoss;
	public GameObject layoutHealthPlayer;
	public Text textTitle;
	public Text textDesc;

	//Help
	public PrefabManager mgPrefab;

	//GameObjects
	private List<GameObject> playerHealth = new List<GameObject>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region TEXT
	public void ShowTitle(string text)
	{
		textTitle.text = text;
		textTitle.color = Color.white;
	}

	public void HideTitle()
	{
		textTitle.color = Color.clear;
	}

	public void ShowDesc(string text)
	{
		textDesc.text = text;
		textDesc.color = Color.white;
	}

	public void HideDesc()
	{
		textDesc.color = Color.clear;
	}

	public void HideText()
	{
		HideTitle();
		HideDesc();
	}
	#endregion
	#region BOSS HEALTH
	public void SetHealthBoss(int min, int max, int value)
	{
		sliderHealthBoss.minValue = min;
		sliderHealthBoss.maxValue = max;
		sliderHealthBoss.value = value;
	}

	public void SetHealthBossValue(int amount)
	{
		sliderHealthBoss.value = amount;
	}
	#endregion
	#region PLAYER HEALTH
	public void SetHealthPlayer(int amount)
	{
		int rest = (amount - playerHealth.Count) * -1;
		if(rest < 0) AddHealthPlayer(Mathf.Abs(rest));
		else if(rest > 0) RemoveHealthPlayer(rest);
	}

	public void RemoveHealthPlayer(int amount)
	{
		amount = Mathf.Min(amount, playerHealth.Count);
		for(int i = 0; i < amount; i++)
		{
			var g = playerHealth.Last();
			Destroy(g);
			playerHealth.Remove(g);
		}
	}

	public void AddHealthPlayer(int amount)
	{
		for(int i = 0; i < amount; i++)
		{
			var g = mgPrefab.SpawnPrefabUI("PlayerHealth");
			g.transform.SetParent(layoutHealthPlayer.transform);
			g.transform.localScale = new Vector3(1f, 1f, 1f);
			playerHealth.Add(g);
		}
	}
	#endregion
}
