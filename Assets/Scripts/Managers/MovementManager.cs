using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour {

	public float moveSpeed;
	private float speed;
	private SpriteRenderer ren;

	public Transform mainTrans;
	public Collider2D rightCollider;
	public Collider2D leftCollider;

	private bool facingRight = true;

	//Awake
	void Awake()
	{
		speed = moveSpeed;
		ren = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MoveDirection(Direction d)
	{
		if(d == Direction.RIGHT) MoveRight();
		else if(d == Direction.LEFT) MoveLeft();
	}

	public void MoveOppositeDirection(Direction d)
	{
		if(d == Direction.RIGHT) MoveLeft();
		else if(d == Direction.LEFT) MoveRight();
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
		mainTrans.position += new Vector3(x, 0.0f, 0.0f);
	}

	void Flip(){
		if(ren != null) ren.flipX = facingRight;
		facingRight = !facingRight;
	}

	bool IsFacingWall()
	{
		Collider2D[] cols = null;
		var dir = GetCurrentDirection();
		if(dir == Direction.RIGHT) cols = Physics2D.OverlapBoxAll(rightCollider.transform.position, rightCollider.bounds.size, 0f);
		if(dir == Direction.LEFT) cols = Physics2D.OverlapBoxAll(leftCollider.transform.position, leftCollider.bounds.size, 0f);
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
