using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

	public int health = 1;
	private Direction hitDir = Direction.NONE;
	private float cdHit; //Cooldown before next hit
	public float cdbHit = 0.1f; //Base cooldown before next hit

	public UnityEvent onDamage;
	public UnityEvent onDeath;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Decrease(int amount)
	{
		if(!Alive()) return;
		if(Time.time < cdHit) return;
		health -= amount;
		cdHit = Time.time + cdbHit;
		if(!Alive())
		{
			onDeath.Invoke();
		}
		else
		{
			onDamage.Invoke();
		}

		print(name + " took " + amount + " damage");
	}

	public void Set(int amount)
	{
		health = amount;
	}

	public bool Alive()
	{
		return health > 0;
	}

	public void SetHitDirection(Direction dir)
	{
		hitDir = dir;
	}

	public Direction GetHitDirection()
	{
		return hitDir;
	}
}
