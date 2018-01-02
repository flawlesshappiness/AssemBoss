using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollisionDamage : MonoBehaviour {

	public int damage = 1;

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
			var h = c.GetComponent<Health>();
			h.SetHitDirection(GetHitDirection(c.transform.position));
			h.Decrease(damage);
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
