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

	public Wall wallLeft;
	public Wall wallRight;

	//UI
	public PanelGame panelGame;

	//Privates
	private PrefabManager mgPrefab;
	private ParticleManager mgParticle;
	private Player player;
	private DataBoss data;
	private Boss boss;

	private float cdEnd;
	private float cdbEnd = 0.5f;
	private bool fightEnd = false;

	private float width;

	//Awake
	void Awake()
	{
		mgPrefab = GetComponent<PrefabManager>();
		mgParticle = GetComponent<ParticleManager>();

		width = Mathf.Abs(wallRight.transform.position.x) - Mathf.Abs(wallLeft.transform.position.x); 
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(state == State.SETUP)
		{
			if(Input.GetKeyDown(Controls.player_attack1))
			{
				player.EnablePlayer(this, mgParticle);
				boss.EnableBoss();
				state = State.PLAYING;
				panelGame.HideText();
			}
		}
		else if(state == State.PLAYING)
		{

		}
		else if(state == State.ENDING)
		{
			if(Time.time < cdEnd) return;
			if(Input.GetKeyDown(Controls.player_attack1))
			{
				panelGame.HideText();
				StartLevel(data);
			}
			else if(Input.GetKeyDown(Controls.player_jump))
			{
				panelGame.HideText();
				mgPanel.Back();
			}
		}
	}

	public void StartLevel(DataBoss db)
	{
		state = State.SETUP;
		fightEnd = false;

		//Spawn boss
		data = db;
		boss = BuildBoss(db);
		boss.transform.position = spawnBoss.position;

		//Spawn player
		player = BuildPlayer(db.player);
		player.transform.position = spawnPlayer.position;

		//UI
		panelGame.SetHealthBoss(0, db.health.value, db.health.value);
		panelGame.SetHealthPlayer(db.player.health.value);
		panelGame.ShowDesc("Press SLASH to start");
	}

	public void ClearLevel()
	{
		Destroy(player.gameObject);
		player = null;

		Destroy(boss.gameObject);
		boss = null;

		foreach(Projectile p in GameObject.FindObjectsOfType<Projectile>()) Destroy(p.gameObject);
	}

	public void PlayerDeath()
	{
		if(!fightEnd)
		{
			panelGame.ShowTitle("DEFEAT");
			End();
		}
	}

	public void BossDeath()
	{
		if(!fightEnd)
		{
			panelGame.ShowTitle("VICTORY");
			End();
		}
	}

	void End()
	{
		ClearLevel();
		fightEnd = true;
		state = State.ENDING;
		cdEnd = Time.time + cdbEnd;
		panelGame.ShowDesc("Press JUMP to quit or SLASH to retry");
	}

	Player BuildPlayer(DataPlayer d)
	{
		var g = mgPrefab.SpawnPrefabGame("Players/" + d.graphic.value);
		var p = g.GetComponent<Player>();

		//Health
		var h = g.GetComponent<Health>();
		h.Init(d.health.value);

		return p;
	}

	Boss BuildBoss(DataBoss data)
	{
		//Spawn
		var g = mgPrefab.SpawnPrefabGame("Boss");
		var b = g.GetComponent<Boss>();
		b.mgLevel = this;
		b.mgParticle = mgParticle;

		//Build stats
		var h = b.health;
		h.Init(data.health.value);

		//Size
		var s = b.mgSize;
		s.SetDefaultSize(b.transform.localScale);

		//Build attacks
		foreach(DataAttack da in data.attacks) b.AddAttack(da);

		//Setup after load
		b.Setup();

		return b;
	}

	public void GiveUp()
	{
		ClearLevel();
		mgPanel.Back();
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

	public Boss GetBoss()
	{
		return boss;
	}

	#region NEXT ATTACK VALUE
	public float GetSignedPercToMid(GameObject g)
	{
		return (Mathf.Abs(transform.position.x) - Mathf.Abs(g.transform.position.x));
	}
	#endregion
	#region UI
	public void UpdateBossHealth(int amount)
	{
		panelGame.SetHealthBossValue(amount);
	}

	public void RemovePlayerHealth(int amount)
	{
		panelGame.RemoveHealthPlayer(amount);
	}

	public void AddPlayerHealth(int amount)
	{
		panelGame.AddHealthPlayer(amount);
	}

	public void ClearPlayerHealth()
	{
		panelGame.SetHealthPlayer(0);
	}
	#endregion
}
