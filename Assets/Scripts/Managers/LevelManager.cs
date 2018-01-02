using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleManager))]
[RequireComponent(typeof(PrefabManager))]
public class LevelManager : MonoBehaviour {

	public Transform spawnPlayer;
	public Transform spawnBoss;

	private PrefabManager mgPrefab;
	private ParticleManager mgParticle;
	private Player player;
	private Boss boss;

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
		
	}

	public void StartLevel(DataBoss db)
	{
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
	}

	public void StopLevel()
	{
		Destroy(player.gameObject);
		player = null;

		Destroy(boss.gameObject);
		boss = null;
	}

	public void PlayerDeath()
	{

	}

	public void BossDeath()
	{

	}

	Boss BuildBoss(DataBoss data)
	{
		//Spawn
		var g = mgPrefab.SpawnPrefabGame("Boss");
		var b = g.GetComponent<Boss>();

		//Build
		b.GetComponent<Health>().Set(data.health);

		return b;
	}
}
