using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BossAttack : MonoBehaviour {
	private enum State { STARTING, ACTIVE, ENDING }
	private State state;
	private bool active;
	private float cdWait;
	private List<BossAttackNext> nextAttacks = new List<BossAttackNext>();
	private BossAttack elseAttack;

	public DataAttack data;
	public Boss boss;

	public PrefabManager mgPrefab;
	public JumpManager mgJump;
	public MovementManager mgMovement;
	public SizeManager mgSize;
	private LevelManager mgLevel;

	//Awake
	void Awake()
	{
		boss = GetComponent<Boss>();
		mgJump = boss.mgJump;
		mgMovement = boss.mgMovement;
		mgPrefab = boss.mgPrefab;
		mgSize = boss.mgSize;
		mgLevel = boss.mgLevel;
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

	public void SetData(DataAttack data)
	{
		this.data = data;
		foreach(DataAttackNext dan in data.nextAttacks) nextAttacks.Add(new BossAttackNext(boss, dan, mgLevel));
		elseAttack = boss.GetAttack(data.elseNextAttack);
	}

	void SetState(State state)
	{
		this.state = state;
		if(state == State.STARTING) AttackStarting();
		else if(state == State.ENDING) AttackEnding();
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
		foreach(BossAttackNext ban in nextAttacks)
		{
			if(ban.CanDoNextAttack())
			{
				ban.EnableNextAttack();
				return;
			}
		}
		elseAttack.Enable();
	}

	public void EndAttack()
	{
		SetState(State.ENDING);
		cdWait = Time.time + data.timeEnd.value;
	}

	public abstract void Init();
	public abstract void UpdateEnabled();
	public abstract void AttackStarting();
	public abstract void AttackEnding();

	private class BossAttackNext
	{
		private Func<bool> check;
		private BossAttack next;

		public BossAttackNext (Boss boss, DataAttackNext dan, LevelManager mgLevel)
		{
			next = boss.GetAttack(dan.nextAttack);
			NextAttackCategory category = (NextAttackCategory)M.GetEnum<NextAttackCategory>(dan.category);
			switch(category)
			{
			case NextAttackCategory.HEALTH:
				switch((NextAttackValueHealth)M.GetEnum<NextAttackValueHealth>(dan.value))
				{
				case NextAttackValueHealth.LOW: check = delegate { return boss.health.GetPerc() < 0.35f; };
					break;
				case NextAttackValueHealth.MED:
					check = delegate { return boss.health.GetPerc() >= 0.35f && boss.health.GetPerc() <= 0.65; };
					break;
				case NextAttackValueHealth.HIGH: check = delegate { return boss.health.GetPerc() > 0.65f; };
					break;
				}
				break;
			case NextAttackCategory.POSITION:
				switch((NextAttackValuePosition)M.GetEnum<NextAttackValuePosition>(dan.value))
				{
				case NextAttackValuePosition.LEFT: check = delegate { return mgLevel.GetSignedPercToMid(boss.gameObject) < 0f; };
					break;
				case NextAttackValuePosition.MID: check = delegate { return mgLevel.GetSignedPercToMid(boss.gameObject) > -0.3f && mgLevel.GetSignedPercToMid(boss.gameObject) < 0.3f; };
					break;
				case NextAttackValuePosition.RIGHT: check = delegate { return mgLevel.GetSignedPercToMid(boss.gameObject) > 0f; };
					break;
				}
				break;
			}
		}

		public bool CanDoNextAttack()
		{
			return next != null && check();
		}

		public void EnableNextAttack()
		{
			next.Enable();
		}
	}
}

