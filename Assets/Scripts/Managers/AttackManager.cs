using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

	public GameObject attack;
	public Sprite spAttack;

	private float cdAttack;
	private float cdbAttack = 0.1f;
	private float cdbAttackEnd = 0.1f;

	private bool hasHit = false;
	private bool attacking = false;
	private SpriteRenderer spRen;
	private Collider2D col;

	//Awake
	void Awake()
	{
		spRen = attack.GetComponent<SpriteRenderer>();
		col = attack.GetComponent<Collider2D>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(attacking) Attacking();
	}

	public void Attack()
	{
		if(Time.time > cdAttack && !attacking)
		{
			hasHit = false;
			attacking = true;
			cdAttack = Time.time + cdbAttack;
			spRen.sprite = spAttack;
		}
	}

	void Attacking()
	{
		if(Time.time < cdAttack)
		{
			if(!hasHit) CheckAttack();
		}
		else
		{
			spRen.sprite = null;
			attacking = false;
			cdAttack = Time.time + cdbAttackEnd;
		}
	}

	void CheckAttack()
	{
		var cols = Physics2D.OverlapBoxAll(col.transform.position, col.bounds.size, 0f);
		if(cols.Length > 1)
		{
			foreach(Collider2D c in cols)
			{
				if(c.isTrigger && c.tag == "Enemy") {
					c.GetComponent<Health>().Decrease(1);
				}
			}
			hasHit = true;
		}
	}
}
