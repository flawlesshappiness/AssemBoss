using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(MovementManager))]
[RequireComponent(typeof(JumpManager))]
[RequireComponent(typeof(AttackManager))]
[RequireComponent(typeof(SpriteManager))]
public class Player : MonoBehaviour {

	private ParticleManager mgParticle;
	private LevelManager mgLevel;
	private MovementManager mgMovement;
	private JumpManager mgJump;
	private AttackManager mgAttack;
	private SpriteManager mgSprite;
	private Health health;

	private bool enabled;

	//Invincibility
	private float cdInv;
	private Lerp<float> lerpInv;
	private Lerp<float> lerpInvDone;

	//Stun
	private float cdStun;
	private float cdbStun;
	private Direction stunMoveDirection = Direction.NONE;

	//Awake
	void Awake()
	{
		mgMovement = GetComponent<MovementManager>();
		mgJump = GetComponent<JumpManager>();
		mgAttack = GetComponent<AttackManager>();
		mgSprite = GetComponent<SpriteManager>();
		health = GetComponent<Health>();

		cdbStun = health.cdbHit * 0.6f;
		mgJump.cdbHitTime = health.cdbHit * 0.25f;
		lerpInv = Lerp.Get(health.cdbHit, 0.1f, 0.7f);
		lerpInvDone = Lerp.Get(0f, 1f, 1f);

		SetControls();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!enabled) return;
		Movement();
		Jumping();
		Attack();

		if(lerpInv.IsFinished() && mgSprite.GetAlpha() < 1f) mgSprite.SetAlpha(1f);
	}

	public void EnablePlayer(LevelManager mgLevel, ParticleManager mgParticle)
	{
		enabled = true;
		this.mgLevel = mgLevel;
		this.mgParticle = mgParticle;
	}

	void Movement()
	{
		if(Stunned())
		{
			if(stunMoveDirection == Direction.RIGHT) mgMovement.MoveRight();
			else if(stunMoveDirection == Direction.LEFT) mgMovement.MoveLeft();
			return;
		}

		switch(Controls.curControlType)
		{
		case Controls.controlType.XBOX360:
			if(Input.GetAxis("Horizontal") > 0.5f) mgMovement.MoveRight();
			else if(Input.GetAxis("Horizontal") < -0.5f) mgMovement.MoveLeft();
			break;
		case Controls.controlType.KEYBOARD:
			if(Input.GetKey(Controls.player_left)) mgMovement.MoveLeft();
			else if(Input.GetKey(Controls.player_right)) mgMovement.MoveRight();
			break;
		default:
			break;
		}
	}

	void Jumping()
	{
		if(Input.GetKeyDown(Controls.player_jump)) mgJump.HoldJump(true);
		else if(Input.GetKeyUp(Controls.player_jump)) mgJump.HoldJump(false);
	}

	void Attack()
	{
		if(Stunned()) return;
		if(Input.GetKeyDown(Controls.player_attack))
		{
			mgAttack.Attack();
		}
	}

	#region STUN
	public bool Stunned()
	{
		return Time.time < cdStun;
	}

	public void Stun(Direction moveDirection)
	{
		cdStun = Time.time + cdbStun;
		stunMoveDirection = moveDirection;
	}
	#endregion
	#region DAMAGE
	public void OnDamage()
	{
		if(health.Alive())
		{
			Stun(health.GetHitDirection()); //Stun player
			mgJump.OnDamaged(); //Cause hit jump

			lerpInv.Reset(); //Reset flash lerp
			mgSprite.SetAlphaLerp(lerpInv); //Set flash lerp
		}
	}

	public void OnDeath()
	{
		mgParticle.SpawnParticle(2f, "PlayerExplode", transform.position);
		gameObject.SetActive(false);

		mgLevel.PlayerDeath();
	}
	#endregion
	#region CONTROLS
	void SetControls()
	{
		string[] joysticks = Input.GetJoystickNames();
		if(joysticks.Length > 0)
		{
			if(joysticks[0].Equals(""))
			{
				Controls.SetDefaultControls_Keyboard();
			}
			else if(joysticks[0].Contains("Xbox 360"))
			{
				Controls.SetDefaultControls_XBox360();
			}
		}
		else
		{
			Controls.SetDefaultControls_Keyboard();
		}
	}
	#endregion
}
