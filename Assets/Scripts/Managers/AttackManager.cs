using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

	private enum State { IDLE, STARTUP, ATTACKING, RECOVERY }
	private State state = State.IDLE;

	private Player player;
	private MovementManager mgMovement;
	private JumpManager mgJump;

	private Direction attackDir;
	private Collider2D attack;
	public Collider2D attackRight;
	public Collider2D attackLeft;
	public Collider2D attackUp;
	public Collider2D attackDown;
	public Color cCombo1;
	public Color cCombo2;
	public Color cCombo3;

	private int damage = 1;

	private float cdAttack;
	private float cdbAttack = 0.05f; //Time attack is active
	private float cdbAttackFinal = 0.3f; //Time before final attack in combo
	private float cdbRecovery = 0.1f; //Recovery cooldown
	private float cdbRecoveryFinal = 0.3f; //Recovery cooldown after final attack in combo

	private bool hasHit = true;

	private float cdWait; //General cooldown timer
	private bool holdAttack; //Has pressed attack button

	//Combo
	private int comboCur = 0;
	private float cdCombo;
	private float cdbCombo; //Cooldown before combo resets

	//Power tokens
	private bool tokenHit = false;
	private int tokens = 0;

	//Awake
	void Awake()
	{
		player = GetComponent<Player>();
		mgMovement = GetComponent<MovementManager>();
		mgJump = GetComponent<JumpManager>();
		cdbCombo = (cdbAttack + cdbRecovery) * 3;
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
			}
		}
		else if(state == State.STARTUP)
		{
			IncreaseCombo();
			if(comboCur == 1)
			{
				damage = 1;
				attack.GetComponent<SpriteRenderer>().color = cCombo1;
				cdWait = Time.time;
			}
			else if(comboCur == 2)
			{
				damage = 1;
				attack.GetComponent<SpriteRenderer>().color = cCombo2;
				cdWait = Time.time;
			}
			else if(comboCur == 3)
			{
				damage = 2;
				attack.GetComponent<SpriteRenderer>().color = cCombo3;
				tokenHit = true;
				cdWait = Time.time + cdbAttackFinal;
				player.Stun(cdbAttackFinal, Direction.NONE);
				mgJump.SetFloating(true);
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
			if(mgJump.IsFloating()) mgJump.SetFloating(false);
			hasHit = false;
			holdAttack = false;
			if(attack != null) attack.gameObject.SetActive(false);
			attack = null;
			SetState(State.IDLE);
			cdWait = (tokenHit) ? Time.time + cdbRecoveryFinal : Time.time + cdbRecovery;
			tokenHit = false;
		}
	}

	public void Attack(Direction dir)
	{
		holdAttack = true;

		if(state != State.IDLE) return;
		if(dir == Direction.RIGHT) attack = attackRight;
		else if(dir == Direction.LEFT) attack = attackLeft;
		else if(dir == Direction.UP) attack = attackUp;
		else if(dir == Direction.DOWN && mgJump.GetState() != JumpManager.JumpState.GROUNDED) attack = attackDown;
		else attack = (mgMovement.GetCurrentDirection() == Direction.RIGHT) ? attackRight : attackLeft;

		attackDir = (dir == Direction.NONE) ? mgMovement.GetCurrentDirection() : dir;
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
				if(attackDir == Direction.DOWN)
				{
					mgJump.ForceJump(0.25f); //Pogo
					player.ResetDash();
				}
			}
		}
		hasHit = true;
		if(tokenHit) tokens++;
	}

	public void DisruptCombo()
	{
		SetState(State.RECOVERY);
	}

	void SetCombo(int amount)
	{
		comboCur = amount;
		if(comboCur > 3) comboCur = 1;
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
