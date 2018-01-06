using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(MovementManager))]
[RequireComponent(typeof(SpriteManager))]
[RequireComponent(typeof(PrefabManager))]
public class Boss : MonoBehaviour {

	private Health health;
	private SpriteManager mgSprite;
	private MovementManager mgMovement;
	private LevelManager mgLevel;
	private ParticleManager mgParticle;
	private Player player;

	private bool enabled;
	private new string name;

	private List<BossAttack> attacks = new List<BossAttack>();

	//Awake
	void Awake()
	{
		health = GetComponent<Health>();
		mgSprite = GetComponent<SpriteManager>();
		mgMovement = GetComponent<MovementManager>();
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
	}

	public void OnDeath()
	{
		mgParticle.SpawnParticle(2f, "BossExplode", transform.position);
		gameObject.SetActive(false);

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
}
