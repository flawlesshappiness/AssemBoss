using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

	public Health health;
	public SpriteManager mgSprite;
	public MovementManager mgMovement;
	public JumpManager mgJump;
	public ParticleManager mgParticle;
	public PrefabManager mgPrefab;
	public SizeManager mgSize;

	private LevelManager mgLevel;
	private Player player;

	private bool enabled;
	private new string name;

	private List<BossAttack> attacks = new List<BossAttack>();

	//Awake
	void Awake()
	{
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!enabled) return;
	}

	public void EnableBoss(LevelManager mgLevel, ParticleManager mgParticle)
	{
		this.mgLevel = mgLevel;
		this.mgParticle = mgParticle;
		enabled = true;
		NextAttack();
	}

	public void OnDamage()
	{
		mgSprite.FadeColor(Color.red, Color.white, 0.5f);
		mgLevel.UpdateBossHealth(health.health);
	}

	public void OnDeath()
	{
		mgParticle.SpawnParticle("BossExplode", 2f, transform.position);
		gameObject.SetActive(false);

		mgLevel.UpdateBossHealth(health.health);
		mgLevel.BossDeath();
	}

	#region ATTACKS
	public void AddAttack(BossAttack attack)
	{
		attacks.Add(attack);
	}

	public void NextAttack()
	{
		if(attacks.Count > 0)
		{
			var attack = attacks[Random.Range(0, attacks.Count)];
			attack.Enable();
		}
	}
	#endregion

	public Player GetPlayer()
	{
		return mgLevel.GetPlayer();
	}

	public Direction GetDirectionToPlayer()
	{
		var ppos = mgLevel.GetPlayerPosition();
		var pos = transform.position;
		return (ppos.x > pos.x) ? Direction.RIGHT : Direction.LEFT;
	}

	public void FacePlayer(bool towards)
	{
		var d = GetDirectionToPlayer();
		if(towards)
		{
			if(mgMovement.GetCurrentDirection() != d) mgMovement.MoveDirection(d);
		}
		else
		{
			if(mgMovement.GetCurrentDirection() == d) mgMovement.MoveOppositeDirection(d);
		}
	}
}
