using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour {

	public float moveSpeed;
	private float speed;
	private DirectionHorizontal moveDir;

	public SpriteRenderer ren;
	public Transform mainTrans;
	public Collider2D rightCollider;
	public Collider2D leftCollider;

	private bool facingRight = true;
	private bool moveDirLocked = false;
	private bool flipLocked = false;

	private float cdMoveDirLocked;
	private float cdFlipLocked;

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
		if(moveDir == DirectionHorizontal.RIGHT) MoveRight(!flipLocked); //Move right
		else if(moveDir == DirectionHorizontal.LEFT) MoveLeft(!flipLocked); //Move left

		if(Time.time > cdMoveDirLocked) moveDirLocked = false; //Disable move direction lock
		if(Time.time > cdFlipLocked) flipLocked = false; //Disable flip lock
	}

	public void MoveDirection(DirectionHorizontal d)
	{
		if(moveDirLocked) return;
		moveDir = d;
	}

	void MoveRight(bool flip){
		if(!facingRight && flip) Flip();
		else if(!HasWall(DirectionHorizontal.RIGHT)) Move(speed);
	}

	void MoveLeft(bool flip){
		if(facingRight && flip) Flip();
		else if(!HasWall(DirectionHorizontal.LEFT)) Move(-speed);
	}

	void Move(float x){
		mainTrans.position += new Vector3(x, 0.0f, 0.0f);
	}

	void Flip(){
		if(ren != null) ren.flipX = facingRight;
		facingRight = !facingRight;
	}

	public void ForceMove(DirectionHorizontal dir, float time)
	{
		float t = Time.time + time;
		moveDirLocked = true;
		flipLocked = true;
		cdMoveDirLocked = t;
		cdFlipLocked = t;
		moveDir = dir;
	}

	bool HasWall(DirectionHorizontal dir)
	{
		Collider2D[] cols = null; //Array of collisions
		if(facingRight) cols = GetCollisions(dir); //If facing right, get collisions to direction
		else cols = GetCollisions(E.Opposite(dir)); //If facing left, get collisions of opposite direction
		foreach(Collider2D c in cols) if(c.transform != transform && !c.isTrigger) return true; //If collision is not me and not a trigger, it's a wall
		return false; //No collisions
	}

	Collider2D[] GetCollisions(DirectionHorizontal dir)
	{
		if(dir == DirectionHorizontal.RIGHT) return Physics2D.OverlapBoxAll(rightCollider.transform.position, rightCollider.bounds.size, 0f);
		else if(dir == DirectionHorizontal.LEFT) return Physics2D.OverlapBoxAll(leftCollider.transform.position, leftCollider.bounds.size, 0f);
		else return new Collider2D[0];
	}

	public DirectionHorizontal GetCurrentDirection()
	{
		return (facingRight) ? DirectionHorizontal.RIGHT : DirectionHorizontal.LEFT;
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
