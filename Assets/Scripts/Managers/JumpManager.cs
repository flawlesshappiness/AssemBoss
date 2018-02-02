using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpManager : MonoBehaviour {

	public Transform mainTrans;
	public Collider2D bottomCollider;

	public enum JumpState { JUMPING, FALLING, GROUNDED, FLOATING }
	private JumpState state = JumpState.FALLING;

	//Times
	public float jumpTime = 0.8f;

	//Speeds
	public float fallSpeed = 0.2f;
	public float jumpSpeed = 0.1f;
	public float jumpToFallSpeed = 0.001f;
	private float curSpeed;

	//Cooldowns
	private float cdJump; //Jump cooldown
	private float cdbJump = 0.1f; //Base jump cooldown

	//Jump
	private bool holdingJump = false;
	private bool jumpDisabledUntilGrounded = false;
	private float jumpEnd; //Time when jump will end

	//Event
	public UnityEvent onGrounded;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(state == JumpState.GROUNDED)
		{
			if(IsGrounded())
			{
				if(holdingJump && Time.time > cdJump) Jump();
			}
			else state = JumpState.FALLING;
		}
		else if(state == JumpState.JUMPING)
		{
			if(holdingJump)
			{
				if(Time.time < jumpEnd) MoveUp();
				else 
				{
					if(jumpDisabledUntilGrounded) curSpeed = 0f;
					state = JumpState.FALLING;
				}
			}
			else
			{
				state = JumpState.FALLING;
				curSpeed = 0f;
			}
		}
		else if(state == JumpState.FALLING)
		{
			if(IsGrounded())
			{
				onGrounded.Invoke();
				state = JumpState.GROUNDED;
				jumpDisabledUntilGrounded = false;
			}
			else Fall();
		}
	}

	public void HoldJump(bool holding)
	{
		if(jumpDisabledUntilGrounded) return;
		holdingJump = holding;
	}

	public void SetFloating(bool floating)
	{
		state = floating ? JumpState.FLOATING : JumpState.FALLING;
		if(!floating) curSpeed = 0f;
	}

	public bool IsFloating()
	{
		return state == JumpState.FLOATING;
	}

	void Jump()
	{
		curSpeed = jumpSpeed;
		cdJump = Time.time + cdbJump;
		jumpEnd = Time.time + jumpTime;
		state = JumpState.JUMPING;
	}

	void MoveUp()
	{
		mainTrans.position += new Vector3(0f, curSpeed * Time.deltaTime, 0f);
	}

	void Fall()
	{
		if(curSpeed > fallSpeed) curSpeed -= jumpToFallSpeed;
		else curSpeed = fallSpeed;

		holdingJump = false;
		mainTrans.position += new Vector3(0f, curSpeed * Time.deltaTime, 0f);
	}

	bool IsGrounded()
	{
		var cols = Physics2D.OverlapBoxAll(bottomCollider.transform.position, bottomCollider.bounds.size, 0f);
		foreach(Collider2D c in cols)
		{
			if(c.transform != transform && !c.isTrigger) {
				return true;
			}
		}

		return false;
	}

	public void ForceJump(float jumpTime)
	{
		curSpeed = jumpSpeed;
		jumpEnd = Time.time + jumpTime;
		state = JumpState.JUMPING;

		jumpDisabledUntilGrounded = false;
		HoldJump(true);
		jumpDisabledUntilGrounded = true;
	}

	public JumpState GetState()
	{
		return state;
	}
}
