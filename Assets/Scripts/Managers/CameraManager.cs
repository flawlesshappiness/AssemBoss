using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	private CameraTarget target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetTarget(CameraTarget target)
	{
		this.target = target;
		TeleportToTarget();
	}

	public void TeleportToTarget()
	{
		var pos = target.GetPos();
		transform.position = new Vector3(pos.x, pos.y, -10f);
	}
}
