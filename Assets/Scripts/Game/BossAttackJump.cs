using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackJump : BossAttack {

	private Boss boss;
	private JumpManager mgJump;
	private MovementManager mgMovement;

	private bool hasJumped = false;
	private float cdWait;

	public DataAttackJump data;

	//Awake
	void Awake()
	{
		boss = GetComponent<Boss>();
		mgJump = GetComponent<JumpManager>();
		mgMovement = GetComponent<MovementManager>();
	}

	public override void Init ()
	{
		mgJump.jumpSpeed = data.jumpSpeed;
		mgJump.jumpTime = data.jumpTime;
		mgJump.fallSpeed = data.fallSpeed;
	}

	public override void UpdateEnabled ()
	{
		if(Time.time > cdWait)
		{
			if(state == State.STARTING)
			{
				cdWait = Time.time + data.timeStart;
				state = State.ENABLED;
			}
			else if(state == State.ENABLED)
			{
				mgJump.HoldJump(true);
				if(!hasJumped && mgJump.GetState() != JumpManager.JumpState.GROUNDED)
				{
					hasJumped = true;
				}
				else if(mgJump.GetState() == JumpManager.JumpState.GROUNDED)
				{
					state = State.ENDING;
					cdWait = Time.time + data.timeEnd;
				}
			}
			else if(state == State.ENDING)
			{
				Disable();
				boss.NextAttack();
			}
		}
	}
}
