using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour {

	public float moveSpeed;
	private float speed;
	public Collider2D frontCollider;

	private bool facingRight = true;

	//Awake
	void Awake()
	{
		speed = moveSpeed;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MoveRight(){
		if(!facingRight) Flip();
		else if(!IsFacingWall()) Move(speed);
	}

	public void MoveLeft(){
		if(facingRight) Flip();
		else if(!IsFacingWall()) Move(-speed);
	}

	void Move(float x){
		transform.position += new Vector3(x, 0.0f, 0.0f);
	}

	void Flip(){
		facingRight = !facingRight;
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
	}

	bool IsFacingWall()
	{
		var cols = Physics2D.OverlapBoxAll(frontCollider.transform.position, frontCollider.bounds.size, 0f);
		foreach(Collider2D c in cols)
		{
			if(c.transform != transform && !c.isTrigger) {
				return true;
			}
		}

		return false;
	}

	public Direction GetCurrentDirection()
	{
		return (facingRight) ? Direction.RIGHT : Direction.LEFT;
	}

	public void ResetSpeed()
	{
		SetSpeed(moveSpeed);
	}

	public void SetSpeed(float speed)
	{
		this.speed = speed;
	}
}
