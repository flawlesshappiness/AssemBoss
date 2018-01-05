using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(MovementManager))]
[RequireComponent(typeof(SpriteManager))]
public class Boss : MonoBehaviour {

	private Health health;
	private SpriteManager mgSprite;
	private MovementManager mgMovement;
	private LevelManager mgLevel;
	private ParticleManager mgParticle;

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
		var attack = attacks[Random.Range(0, attacks.Count)];
		attack.Enable();
	}
	#endregion
}
