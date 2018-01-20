using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class CollisionDamage : MonoBehaviour {

	public int damage = 1;
	public UnityEvent onHitPlayer;
	public UnityEvent onHitWall;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		
	}

	void OnTriggerStay2D(Collider2D c)
	{
		if(c.tag == "Player")
		{
			var p = c.GetComponent<Player>();
			bool hit = p.Damage(damage, GetHitDirection(c.transform.position));
			if(hit) onHitPlayer.Invoke();
		}
		else if(c.tag == "Wall")
		{
			onHitWall.Invoke();
		}
	}

	void OnTriggerExit2D(Collider2D c)
	{
		
	}

	Direction GetHitDirection(Vector3 oPos)
	{
		var pos = transform.position;
		if(oPos.x > pos.x) return Direction.RIGHT;
		else return Direction.LEFT;
	}
}
