using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

	public int health = 1;

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
		health -= amount;
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
}
