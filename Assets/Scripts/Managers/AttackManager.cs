using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

	private enum State { IDLE, STARTUP, ATTACKING, RECOVERY }
	private State state = State.IDLE;

	private Player player;

	private Collider2D attack;
	public Collider2D attack1;
	public Collider2D attack2;
	public Collider2D attack3;

	private int damage = 1;

	private float cdAttack;
	private float cdbAttack = 0.1f; //Attack cooldown
	private float cdbRecovery = 0.1f; //Recovery cooldown

	private bool hasHit = true;

	private float cdWait; //General cooldown timer
	private bool holdAttack; //Has pressed attack button

	//Combo
	private int comboCur = 0;
	private float cdbComboFinal = 0.3f; //Time before the final attack in the combo
	private float cdCombo;
	private float cdbCombo; //Cooldown before combo resets

	//Power tokens
	private bool tokenHit = false;
	private int tokens = 0;

	//Awake
	void Awake()
	{
		player = GetComponent<Player>();
		cdbCombo = (cdbAttack + cdbRecovery) * 2;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(state == State.RECOVERY && !hasHit) CheckAttack();

		if(Time.time < cdWait) return;
		if(state == State.IDLE)
		{
			if(holdAttack)
			{
				SetState(State.STARTUP);
				holdAttack = false;
			}
		}
		else if(state == State.STARTUP)
		{
			IncreaseCombo();
			if(comboCur == 1)
			{
				attack = attack1;
				damage = 1;
				cdWait = Time.time;
			}
			else if(comboCur == 2)
			{
				attack = attack2;
				damage = 1;
				cdWait = Time.time;
			}
			else if(comboCur == 3)
			{
				attack = attack3;
				damage = 2;
				tokenHit = true;
				cdWait = Time.time + cdbComboFinal;
			}
			SetState(State.ATTACKING);
		}
		else if(state == State.ATTACKING)
		{
			hasHit = false;
			attack.gameObject.SetActive(true);
			SetState(State.RECOVERY);
			cdWait = Time.time + cdbAttack;
		}
		else if(state == State.RECOVERY)
		{
			attack.gameObject.SetActive(false);
			attack = null;
			SetState(State.IDLE);
			cdWait = Time.time + cdbRecovery;
		}
	}

	public void Attack()
	{
		holdAttack = true;
	}

	void CheckAttack()
	{
		if(attack == null) return;
		var cols = Physics2D.OverlapBoxAll(attack.transform.position, attack.bounds.size, 0f);
		if(cols.Length > 1)
		{
			OnHit(cols);
		}
	}

	void OnHit(Collider2D[] cols)
	{
		foreach(Collider2D c in cols)
		{
			if(c.isTrigger && c.tag == "Enemy") {
				c.GetComponent<Health>().Decrease(damage);
			}
		}
		hasHit = true;
		if(tokenHit) tokens++;
	}

	void SetCombo(int amount)
	{
		comboCur = amount;
		if(comboCur > 3) comboCur = 1;
		print(comboCur);
	}

	void IncreaseCombo()
	{
		if(Time.time > cdCombo) SetCombo(1);
		else SetCombo(comboCur + 1);
		cdCombo = Time.time + cdbCombo;
	}

	void SetState(State state)
	{
		this.state = state;
	}
}
