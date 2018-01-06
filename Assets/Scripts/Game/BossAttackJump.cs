using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossAttackJump : BossAttack {

	private enum State { JUMPSTART, JUMPING, JUMPEND }
	private State state;

	private Approach approachToPlayer;

	private bool hasJumped = false;
	private bool moving = false;
	private Direction moveDir;

	public override void Init ()
	{
		var d = (DataAttackJump)data;
		mgJump.jumpSpeed = d.jumpSpeed;
		mgJump.jumpTime = d.jumpTime;
		mgJump.fallSpeed = -d.fallSpeed;
		mgMovement.moveSpeed = d.moveSpeed;
		approachToPlayer = (Approach)Enum.Parse(typeof(Approach), d.approachToPlayer);
		SetState(State.JUMPSTART);
	}

	public override void UpdateEnabled ()
	{
		if(moving) Move();

		if(state == State.JUMPSTART)
		{
			if(mgJump.GetState() != JumpManager.JumpState.GROUNDED) SetState(State.JUMPING);
			mgJump.HoldJump(true); //Jump
			moving = true; //Move
			moveDir = boss.GetDirectionToPlayer();
		}
		else if(state == State.JUMPING)
		{
			if(mgJump.GetState() == JumpManager.JumpState.GROUNDED) SetState(State.JUMPEND);
		}
		else if(state == State.JUMPEND)
		{
			mgJump.HoldJump(false);
			moving = false;
			EndAttack();
		}
	}

	void SetState(State state)
	{
		this.state = state;
	}

	void Move()
	{
		if(approachToPlayer == Approach.TOWARDS) //Movement
		{
			if(moveDir == Direction.RIGHT) mgMovement.MoveRight();
			else mgMovement.MoveLeft();
		}
		else
		{
			if(moveDir == Direction.RIGHT) mgMovement.MoveLeft();
			else mgMovement.MoveRight();
		}
	}
}
