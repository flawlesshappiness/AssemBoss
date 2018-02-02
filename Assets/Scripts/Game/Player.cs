using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(MovementManager))]
[RequireComponent(typeof(JumpManager))]
[RequireComponent(typeof(AttackManager))]
public class Player : MonoBehaviour {

	public enum eGraphic { TENTACLE }

	private ParticleManager mgParticle;
	private LevelManager mgLevel;
	public MovementManager mgMovement;
	public JumpManager mgJump;
	public AttackManager mgAttack;
	public SpriteManager mgSprite;
	public Health health;

	private bool enabled;

	//Axis
	private bool triggerAxisDown = false;

	//Invincibility
	private float cdInv;
	public float cdbInv; //Invincibility time
	private Lerp<float> lerpInv;

	//Stun
	private float cdStun;
	private float cdbStun; //Stun time
	private float cdbStunJump; //Jump time while stunned

	//Dash
	private float cdDash;
	private float cdbDash; //Dash cooldown
	private float cdDashEnd; //Time dash ends
	public float timeDash; //Dash time
	public float speedDash;
	private bool dashing = false;
	private bool hasDashed = false;

	//Awake
	void Awake()
	{
		//Stuns
		cdbStun = cdbInv * 0.25f;
		cdbStunJump = cdbInv * 0.25f;

		//Dash
		cdbDash = timeDash + 0.1f;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!enabled) return;
		Attack();
		CheckToDash();
		Jumping();
		Movement();

		if(lerpInv != null && lerpInv.IsFinished() && mgSprite.GetAlpha() < 1f) mgSprite.SetAlpha(1f);
	}

	public void EnablePlayer(LevelManager mgLevel, ParticleManager mgParticle)
	{
		enabled = true;
		this.mgLevel = mgLevel;
		this.mgParticle = mgParticle;
	}

	#region MOVEMENT
	void Movement()
	{
		if(Stunned()) return;

		switch(Controls.curControlType)
		{
		case Controls.controlType.XBOX360:
			if(Input.GetAxis("Horizontal") > 0.5f) mgMovement.MoveDirection(DirectionHorizontal.RIGHT);
			else if(Input.GetAxis("Horizontal") < -0.5f) mgMovement.MoveDirection(DirectionHorizontal.LEFT);
			else mgMovement.MoveDirection(DirectionHorizontal.NONE);
			break;
		case Controls.controlType.KEYBOARD:
			if(Input.GetKey(Controls.player_right)) mgMovement.MoveDirection(DirectionHorizontal.RIGHT);
			else if(Input.GetKey(Controls.player_left)) mgMovement.MoveDirection(DirectionHorizontal.LEFT);
			else mgMovement.MoveDirection(DirectionHorizontal.NONE);
			break;
		default:
			break;
		}
	}
	#endregion
	#region JUMP
	void Jumping()
	{
		if(Stunned()) return;
		if(Input.GetKeyDown(Controls.player_jump)) mgJump.HoldJump(true);
		else if(Input.GetKeyUp(Controls.player_jump)) mgJump.HoldJump(false);
	}
	#endregion
	#region ATTACK
	void Attack()
	{
		if(Stunned()) return;
		if(Input.GetKeyDown(Controls.player_attack1))
		{
			mgAttack.Attack(Controls.player_attack1, GetAnalogDirection());
		}
		else if(Input.GetKeyDown(Controls.player_attack2))
		{
			mgAttack.Attack(Controls.player_attack2, GetAnalogDirection());
		}
	}

	Direction GetAnalogDirection()
	{
		float hor = Input.GetAxisRaw("Horizontal");
		float ver = Input.GetAxisRaw("Vertical");
		float min = 0.5f;

		if(ver > min) return Direction.UP;
		else if(ver < -min) return Direction.DOWN;
		else if(hor > min) return Direction.RIGHT;
		else if(hor < -min) return Direction.LEFT;
		else return Direction.NONE;
	}
	#endregion
	#region DASH
	void CheckToDash()
	{
		if(Time.time > cdDashEnd && dashing)
		{
			mgJump.SetFloating(false);
			mgMovement.ResetSpeed();
			dashing = false;
			return;
		}
		if(hasDashed) return;
		if(Stunned() || Time.time < cdDash) return;
		if(Controls.curControlType == Controls.controlType.KEYBOARD)
		{
			if(Input.GetKeyDown(Controls.player_dodge)) Dash();
		}
		else
		{
			if(Input.GetAxisRaw("ControllerTrigger") < 0)
			{
				if(!triggerAxisDown)
				{
					Dash();
					triggerAxisDown = true;
				}
			}
			else if(Input.GetAxis("ControllerTrigger") == 0)
			{
				triggerAxisDown = false;
			}
		}
	}

	void Dash()
	{
		mgMovement.SetSpeed(speedDash);
		mgJump.SetFloating(true);
		Stun(timeDash, mgMovement.GetCurrentDirection());
		MakeInvincible(timeDash);
		dashing = true;
		hasDashed = true;
		cdDash = Time.time + cdbDash;
		cdDashEnd = Time.time + timeDash;
	}

	public void ResetDash()
	{
		hasDashed = false;
	}
	#endregion
	#region STUN
	public bool Stunned()
	{
		return Time.time < cdStun;
	}

	public void Stun(float time, DirectionHorizontal moveDirection)
	{
		cdStun = Time.time + time;
		if(moveDirection != DirectionHorizontal.NONE) mgMovement.ForceMove(moveDirection, time);
	}
	#endregion
	#region INVINCIBLE
	void MakeInvincible(float time)
	{
		float timeLeft = cdInv - Time.time;
		if(time > timeLeft)
		{
			cdInv = Time.time + time;
			lerpInv = Lerp.Get(time, 0.1f, 0.5f);
			mgSprite.SetAlphaLerp(lerpInv);
		}
	}

	bool IsInvincible()
	{
		return Time.time < cdInv;
	}
	#endregion
	#region DAMAGE
	public bool Damage(int amount, DirectionHorizontal dir)
	{
		if(IsInvincible()) return false; //Do nothing on invincible

		health.Decrease(amount);
		MakeInvincible(cdbInv); //Make player invincible
		Stun(cdbStun, dir); //Stun player
		mgJump.ForceJump(cdbStunJump); //Stun jump
		mgLevel.RemovePlayerHealth(amount);
		mgAttack.DisruptAttack();
		return true;
	}

	public void OnDeath()
	{
		mgParticle.SpawnParticle("PlayerExplode", 2f, transform.position);
		gameObject.SetActive(false);

		mgLevel.PlayerDeath();
	}
	#endregion
}
