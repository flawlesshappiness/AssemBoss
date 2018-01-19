using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

	private int health = 1;
	private int max;

	public UnityEvent onDamage;
	public UnityEvent onDeath;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init(int amount)
	{
		Set(amount);
		max = amount;
	}

	public void Decrease(int amount)
	{
		if(!Alive()) return;
		health -= amount;
		if(!Alive())
		{
			onDeath.Invoke();
		}
		else
		{
			onDamage.Invoke();
		}
	}

	public void Set(int amount)
	{
		health = amount;
	}

	public int Get()
	{
		return health;
	}

	public float GetPerc()
	{
		return (float)health / (float)max;
	}

	public bool Alive()
	{
		return health > 0;
	}
}
