using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour {
	private enum State { STARTING, ACTIVE, ENDING }
	private State state;
	private bool active;
	private float cdWait;

	public DataAttack data;
	public Boss boss;

	public PrefabManager mgPrefab;
	public JumpManager mgJump;
	public MovementManager mgMovement;

	//Awake
	void Awake()
	{
		boss = GetComponent<Boss>();
		mgJump = GetComponent<JumpManager>();
		mgMovement = GetComponent<MovementManager>();
		mgPrefab = GetComponent<PrefabManager>();
	}

	void Update()
	{
		if(!active) return;
		if(Time.time > cdWait)
		{
			if(state == State.STARTING)
			{
				cdWait = Time.time + data.timeStart.value;
				SetState(State.ACTIVE);
			}
			else if(state == State.ACTIVE)
			{
				UpdateEnabled();
			}
			else if(state == State.ENDING)
			{
				NextAttack();
			}
		}
	}

	void SetState(State state)
	{
		this.state = state;
	}

	public void Enable()
	{
		SetState(State.STARTING);
		Init();
		active = true;
	}

	void NextAttack()
	{
		active = false;
		boss.NextAttack();
	}

	public void EndAttack()
	{
		SetState(State.ENDING);
		cdWait = Time.time + data.timeEnd.value;
	}

	public abstract void Init();
	public abstract void UpdateEnabled();
}
