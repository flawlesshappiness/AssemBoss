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
}
