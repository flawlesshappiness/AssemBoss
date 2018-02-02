using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

	private enum State { IDLE, STARTUP, ATTACKING, RECOVERY }
	private State state = State.IDLE;

	private enum AttackType { SLASH, STAB }

	private Player player;
	private MovementManager mgMovement;
	private JumpManager mgJump;

	public Animator animator;
	private Direction attackDir;
	private Collider2D attack;
	public Collider2D slashRight;
	public Collider2D slashLeft;
	public Collider2D slashUp;
	public Collider2D slashDown;
	public Collider2D stabRight;
	public Collider2D stabLeft;
	public Collider2D stabUp;
	public Collider2D stabDown;

	private int damage = 1;
	private bool holdAttack; //Has pressed attack button
	private bool hasHit = true;

	//Cooldowns
	private float cdWait; //General cooldown timer
	private float cdAttack;
	private float cdbAttack = 0.05f; //Time attack is active
	private float cdbAttackFinal = 0.3f; //Time before final attack in combo
	private float cdbRecovery = 0.2f; //Recovery cooldown
	private float cdbRecoveryFinal = 0.3f; //Recovery cooldown after final attack in combo
	private float cdCombo;
	private float cdbCombo; //Cooldown before combo resets

	//Animation
	private bool attackAnimRight = true;
	private string attackAnim = "";

	//Power tokens
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
			damage = 1;
			cdWait = Time.time;
			SetState(State.ATTACKING);
		}
		else if(state == State.ATTACKING)
		{
			animator.SetTrigger(attackAnim);
			hasHit = false;
			attack.gameObject.SetActive(true);
			SetState(State.RECOVERY);
			cdWait = Time.time + cdbAttack;
		}
		else if(state == State.RECOVERY)
		{
			animator.SetTrigger("idle");

			if(mgJump.IsFloating()) mgJump.SetFloating(false);
			hasHit = false;
			holdAttack = false;
			if(attack != null) attack.gameObject.SetActive(false);
			attack = null;
			SetState(State.IDLE);
			cdWait = Time.time + cdbRecovery;
		}
	}

	public void Attack(KeyCode button, Direction dir)
	{
		holdAttack = true;

		if(state != State.IDLE) return;
		attackDir = (dir == Direction.NONE) ? E.ToDirection(mgMovement.GetCurrentDirection()) : dir;
		AttackType type = (button == Controls.player_attack2) ? AttackType.STAB : AttackType.SLASH;
		attack = GetAttack(type, attackDir);
		attackAnim = GetAttackAnim(type, dir);
		attackAnimRight = !attackAnimRight;
	}

	Collider2D GetAttack(AttackType type, Direction dir)
	{
		if(type == AttackType.SLASH)
		{
			switch(dir)
			{
			case Direction.RIGHT: return slashRight;
			case Direction.LEFT: return slashLeft;
			case Direction.UP: return slashUp;
			case Direction.DOWN: 
				if(mgJump.GetState() == JumpManager.JumpState.GROUNDED) return GetAttack(type, E.ToDirection(mgMovement.GetCurrentDirection()));
				else return slashDown;
			}
		}
		else if(type == AttackType.STAB)
		{
			switch(dir)
			{
			case Direction.RIGHT: return stabRight;
			case Direction.LEFT: return stabLeft;
			case Direction.UP: return stabUp;
			case Direction.DOWN: 
				if(mgJump.GetState() == JumpManager.JumpState.GROUNDED) return GetAttack(type, E.ToDirection(mgMovement.GetCurrentDirection()));
				else return stabDown;
			}
		}

		return null;
	}

	string GetAttackAnim(AttackType type, Direction dir)
	{
		string s = type.ToString().ToLower();
		switch(dir)
		{
		case Direction.RIGHT: s += "_side_";
			break;
		case Direction.LEFT: s += "_side_";
			break;
		case Direction.UP: s += "_side_";
			break;
		case Direction.DOWN: s += "_side_";
			break;
		default: s += "_side_";
			break;
		}
		s += attackAnimRight ? "r" : "l";
		return s;
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
				OnHitEnemy(c.gameObject);
			}
		}
		hasHit = true;
	}

	void OnHitEnemy(GameObject enemy)
	{
		enemy.GetComponent<Health>().Decrease(damage);
		if(attackDir == Direction.DOWN)
		{
			mgJump.ForceJump(0.25f); //Pogo
			player.ResetDash();
		}
		else if(attackDir == Direction.RIGHT) mgMovement.ForceMove(DirectionHorizontal.LEFT, 0.2f);
		else if(attackDir == Direction.LEFT) mgMovement.ForceMove(DirectionHorizontal.RIGHT, 0.2f);
	}

	public void DisruptAttack()
	{
		SetState(State.RECOVERY);
	}

	void SetState(State state)
	{
		this.state = state;
	}
}
