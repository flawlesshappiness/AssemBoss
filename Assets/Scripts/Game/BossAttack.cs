using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour {
	public enum State { STARTING, ENABLED, ENDING }
	public State state;

	private bool enabled = false;

	void Update()
	{
		if(!enabled) return;
		UpdateEnabled();
	}

	public void Enable()
	{
		this.enabled = true;
		state = State.STARTING;
		Init();
	}

	public void Disable()
	{
		this.enabled = false;
	}

	public abstract void Init();
	public abstract void UpdateEnabled();
}
