using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpManager : MonoBehaviour {

	public Collider2D bottomCollider;

	public enum JumpState { JUMPING, FALLING, GROUNDED, DAMAGED }
	private JumpState state = JumpState.FALLING;

	//Times
	public float jumpTime = 0.8f;

	//Speeds
	public float fallSpeed = 0.2f;
	public float jumpSpeed = 0.1f;
	public float jumpToFallSpeed = 0.01f;
	private float curSpeed;

	//Cooldowns
	private float cdJump; //Jump cooldown
	private float cdbJump = 0.1f; //Base jump cooldown
	private float cdHitTime; //Hit cooldown
	[HideInInspector]
	public float cdbHitTime = 2f; //Base hit cooldown

	//Jump
	private bool holdingJump = false;
	private float jumpEnd; //Time when jump will end

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(state == JumpState.GROUNDED)
		{
			if(IsGrounded())
			{
				if(holdingJump) Jump();
			}
			else state = JumpState.FALLING;
		}
		else if(state == JumpState.JUMPING)
		{
			if(holdingJump && Time.time < jumpEnd || Time.time < cdJump) MoveUp();
			else state = JumpState.FALLING;
		}
		else if(state == JumpState.DAMAGED)
		{
			if(Time.time < cdHitTime) MoveUp();
			else
			{
				state = JumpState.FALLING;
			}
		}
		else if(state == JumpState.FALLING)
		{
			if(IsGrounded()) state = JumpState.GROUNDED;
			else Fall();
		}
	}

	public void HoldJump(bool holding)
	{
		holdingJump = holding;
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
		transform.position += new Vector3(0f, curSpeed * Time.deltaTime, 0f);
	}

	void Fall()
	{
		if(curSpeed > fallSpeed) curSpeed -= jumpToFallSpeed;
		else curSpeed = fallSpeed;

		holdingJump = false;
		transform.position += new Vector3(0f, curSpeed * Time.deltaTime, 0f);
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

	public void OnDamaged()
	{
		curSpeed = jumpSpeed;
		cdHitTime = Time.time + cdbHitTime;
		state = JumpState.DAMAGED;
	}

	public JumpState GetState()
	{
		return state;
	}
}
