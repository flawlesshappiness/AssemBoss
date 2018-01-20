using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public PrefabManager mgPrefab;
	public ParticleManager mgParticle;

	private bool active;
	private float speed;
	private float rotSpeed = 1f;
	private Player player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!active) return;
		MoveForward();
		if(player != null && rotSpeed > 0f) AimTowardsPlayer();
	}

	void MoveForward()
	{
		transform.position += transform.right * speed;
	}

	void AimTowardsPlayer()
	{
		Vector3 vectorToTarget = player.transform.position - transform.position;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotSpeed);
	}

	public void Fire(float speedMove, float speedRotation, Player player)
	{
		active = true;
		this.speed = speedMove;
		this.rotSpeed = speedRotation;
		this.player = player;
	}

	public void Despawn()
	{
		mgParticle.SpawnParticle("ProjectileHit", 0.2f, transform.position);
		gameObject.SetActive(false);
		Destroy(gameObject);
	}
}
