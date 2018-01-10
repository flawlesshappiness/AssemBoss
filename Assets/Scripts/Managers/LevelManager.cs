using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ParticleManager))]
[RequireComponent(typeof(PrefabManager))]
public class LevelManager : MonoBehaviour {

	private enum State { IDLE, SETUP, PLAYING, ENDING }
	private State state;

	public PanelManager mgPanel;

	public Transform spawnPlayer;
	public Transform spawnBoss;

	//UI
	public Text textEnd;

	//Privates
	private PrefabManager mgPrefab;
	private ParticleManager mgParticle;
	private Player player;
	private Boss boss;

	private float cdEnd;
	private float cdbEnd = 2f;
	private bool fightEnd = false;

	//Awake
	void Awake()
	{
		mgPrefab = GetComponent<PrefabManager>();
		mgParticle = GetComponent<ParticleManager>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(state == State.SETUP)
		{

		}
		else if(state == State.PLAYING)
		{

		}
		else if(state == State.ENDING)
		{
			if(Time.time > cdEnd)
			{
				StopLevel();
				state = State.IDLE;
			}
		}
	}

	public void StartLevel(DataBoss db)
	{
		state = State.SETUP;
		fightEnd = false;

		//Spawn boss
		boss = BuildBoss(db);
		boss.transform.position = spawnBoss.position;

		//Spawn player
		var g = mgPrefab.SpawnPrefabGame("Player");
		g.transform.position = spawnPlayer.position;
		player = g.GetComponent<Player>();

		//Enable
		player.EnablePlayer(this, mgParticle);
		boss.EnableBoss(this, mgParticle);

		state = State.PLAYING;
	}

	public void StopLevel()
	{
		Destroy(player.gameObject);
		player = null;

		Destroy(boss.gameObject);
		boss = null;

		state = State.IDLE;
		textEnd.color = Color.clear;
		mgPanel.Back();
	}

	public void PlayerDeath()
	{
		if(!fightEnd)
		{
			textEnd.text = "DEFEAT";
			End();
		}
	}

	public void BossDeath()
	{
		if(!fightEnd)
		{
			textEnd.text = "VICTORY";
			End();
		}
	}

	void End()
	{
		fightEnd = true;
		textEnd.color = Color.white;
		state = State.ENDING;
		cdEnd = Time.time + cdbEnd;
	}

	Boss BuildBoss(DataBoss data)
	{
		//Spawn
		var g = mgPrefab.SpawnPrefabGame("Boss");
		var b = g.GetComponent<Boss>();

		//Build stats
		b.GetComponent<Health>().Set(data.health);

		//Build attacks
		foreach(DataAttack da in data.attacks)
		{
			b.AddAttack(da.AddComponent(g));
		}

		return b;
	}

	public void GiveUp()
	{
		StopLevel();
	}

	public Vector3 GetPlayerPosition()
	{
		return player.transform.position;
	}

	public Vector3 GetBossPosition()
	{
		return boss.transform.position;
	}

	public Player GetPlayer()
	{
		return player;
	}
}
