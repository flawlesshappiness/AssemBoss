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
	private DirectionHorizontal moveDir;

	public override void Init ()
	{
		var d = (DataAttackJump)data;
		mgJump.jumpSpeed = d.jumpSpeed.value;
		mgJump.jumpTime = d.jumpTime.value;
		mgJump.fallSpeed = -d.fallSpeed.value;
		mgMovement.SetSpeed(d.moveSpeed.value);
		approachToPlayer = (Approach)Enum.Parse(typeof(Approach), d.approachToPlayer.value);
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

		if(state == State.JUMPING)
		{
			mgSize.FadeSize(mgSize.GetSizeMultiplied(0.8f, 1.2f), 0.1f);
		}
	}

	void Move()
	{
		if(approachToPlayer == Approach.TOWARDS) //Movement
		{
			mgMovement.MoveDirection(moveDir);
		}
		else
		{
			mgMovement.MoveDirection(E.Opposite(moveDir));
		}
	}

	public override void AttackStarting ()
	{
		boss.FacePlayer(approachToPlayer == Approach.TOWARDS);
		moveDir = boss.GetDirectionToPlayer();
		mgSize.FadeSize(mgSize.GetSizeMultiplied(1.3f, 0.7f), data.timeStart.value);
	}

	public override void AttackEnding ()
	{
		mgSize.FadeToDefault(0.1f);
	}
}
