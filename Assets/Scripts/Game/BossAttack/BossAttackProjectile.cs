using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossAttackProjectile : BossAttack {

	private int amount;
	private float delay;
	private ProjectileSpawner spawner;
	private bool facePlayer;

	private float cdWait;

	public override void Init ()
	{
		DataAttackShoot d = (DataAttackShoot)data;
		amount = d.projectileAmount.value;
		delay = d.spawnDelay.value;
		facePlayer = d.facePlayer.value;

		var speedMove = d.speedMove.value;
		var speedRotation = d.speedRotation.value;
		var scale = d.scale.value;
		var direction = (ProjectileDirection)Enum.Parse(typeof(ProjectileDirection), d.projectileDirection.value);
		switch(direction)
		{
		case ProjectileDirection.STRAIGHT_FORWARD: spawner = new ProjectileSpawnerForward(boss, scale, speedMove, speedRotation);
			break;
		case ProjectileDirection.CIRCLE: spawner = new ProjectileSpawnerCircle(boss, scale, amount, speedMove, speedRotation);
			break;
		case ProjectileDirection.CIRCLE_HALF_UP: spawner = new ProjectileSpawnerCircleHalfUp(boss, scale, amount, speedMove, speedRotation);
			break;
		case ProjectileDirection.CIRCLE_HALF_DOWN: spawner = new ProjectileSpawnerCircleHalfDown(boss, scale, amount, speedMove, speedRotation);
			break;
		case ProjectileDirection.CIRCLE_HALF_FORWARD: spawner = new ProjectileSpawnerCircleHalfForward(boss, scale, amount, speedMove, speedRotation);
			break;
		default: spawner = new ProjectileSpawnerForward(boss, scale, speedMove, speedRotation);
			break;
		}
	}

	public override void UpdateEnabled ()
	{
		if(Time.time > cdWait)
		{
			if(delay == 0f)
			{
				for(int i = 0; i < amount; i++)
				{
					SpawnProjectile();
				}
				EndAttack();
			}
			else if(amount > 0)
			{
				SpawnProjectile();
				amount--;
				cdWait = Time.time + delay;
			}
			else
			{
				EndAttack();
			}
		}
	}

	public override void AttackStarting ()
	{
		if(facePlayer) boss.FacePlayer(true);
		//throw new NotImplementedException ();
	}

	public override void AttackEnding ()
	{
		//throw new NotImplementedException ();
	}

	void SpawnProjectile()
	{
		spawner.Spawn();
	}

	private abstract class ProjectileSpawner
	{
		public Boss boss;
		public float scale;
		public float speedMove;
		public float speedRotation;

		public ProjectileSpawner (Boss boss, float scale, float speedMove, float speedRotation)
		{
			this.boss = boss;
			this.scale = scale;
			this.speedMove = speedMove;
			this.speedRotation = speedRotation;
		}

		public Projectile CreateProjectile()
		{
			var g = boss.mgPrefab.SpawnPrefabGame("Projectile");
			g.transform.position = boss.middleTrans.position;
			g.transform.localScale = new Vector3(scale, scale, scale);
			return g.GetComponent<Projectile>();
		}

		public void RotateProjectile(Transform t, float angle)
		{
			t.RotateAround(t.position, Vector3.forward, angle);
		}

		public void FireProjectile(Projectile p)
		{
			p.Fire(speedMove, speedRotation, boss.GetPlayer());
		}

		public abstract void Spawn();
	}

	private class ProjectileSpawnerCircle : ProjectileSpawner
	{
		private int circleState = 1;
		private float anglePerProjectile;
		private int amount;

		public ProjectileSpawnerCircle (Boss boss, float scale, int amount, float speedMove, float speedRotation) : base(boss, scale, speedMove, speedRotation)
		{
			circleState = 1;
			anglePerProjectile = (360f / (float)amount);
			this.amount = amount;
		}

		public override void Spawn ()
		{
			var p = CreateProjectile();
			float startAngle = (boss.mgMovement.GetCurrentDirection() == Direction.RIGHT) ? 0f : 180f;
			float angle = ((circleState+1) * anglePerProjectile);
			RotateProjectile(p.transform, startAngle + angle);
			FireProjectile(p);

			circleState = (circleState + 1) % (amount);
		}
	}

	private class ProjectileSpawnerCircleHalfUp : ProjectileSpawner
	{
		private int circleState = 1;
		private float anglePerProjectile;
		private int amount;

		public ProjectileSpawnerCircleHalfUp (Boss boss, float scale, int amount, float speedMove, float speedRotation) : base(boss, scale, speedMove, speedRotation)
		{
			circleState = 1;
			anglePerProjectile = (180f / (float)amount);
			this.amount = amount;
		}

		public override void Spawn ()
		{
			var p = CreateProjectile();
			var dir = (boss.mgMovement.GetCurrentDirection() == Direction.RIGHT);
			float startAngle = dir ? 0f : 180f;
			float angleMult = dir ? 1f : -1f;
			float angle = ((circleState+1) * anglePerProjectile) * angleMult;
			RotateProjectile(p.transform, startAngle + angle);
			FireProjectile(p);

			circleState = (circleState + 1) % (amount);
		}
	}

	private class ProjectileSpawnerCircleHalfDown : ProjectileSpawner
	{
		private int circleState = 1;
		private float anglePerProjectile;
		private int amount;

		public ProjectileSpawnerCircleHalfDown (Boss boss, float scale, int amount, float speedMove, float speedRotation) : base(boss, scale, speedMove, speedRotation)
		{
			circleState = 1;
			anglePerProjectile = (180f / (float)amount);
			this.amount = amount;
		}

		public override void Spawn ()
		{
			var p = CreateProjectile();
			var dir = (boss.mgMovement.GetCurrentDirection() == Direction.RIGHT);
			float startAngle = dir ? 0f : 180f;
			float angleMult = dir ? -1f : 1f;
			float angle = ((circleState+1) * anglePerProjectile) * angleMult;
			RotateProjectile(p.transform, startAngle + angle);
			FireProjectile(p);

			circleState = (circleState + 1) % (amount);
		}
	}

	private class ProjectileSpawnerCircleHalfForward : ProjectileSpawner
	{
		private int circleState = 1;
		private float anglePerProjectile;
		private int amount;

		public ProjectileSpawnerCircleHalfForward (Boss boss, float scale, int amount, float speedMove, float speedRotation) : base(boss, scale, speedMove, speedRotation)
		{
			circleState = 1;
			anglePerProjectile = (180f / (float)amount);
			this.amount = amount;
		}

		public override void Spawn ()
		{
			var p = CreateProjectile();
			float startAngle = 90f;
			float angleMult = (boss.mgMovement.GetCurrentDirection() == Direction.RIGHT) ? -1f : 1f;
			float angle = ((circleState+1) * anglePerProjectile) * angleMult;
			RotateProjectile(p.transform, startAngle + angle);
			FireProjectile(p);

			circleState = (circleState + 1) % (amount);
		}
	}

	private class ProjectileSpawnerForward : ProjectileSpawner
	{
		public ProjectileSpawnerForward (Boss boss, float scale, float speedMove, float speedRotation) : base(boss, scale, speedMove, speedRotation)
		{
			
		}

		public override void Spawn ()
		{
			var p = CreateProjectile();
			if(boss.mgMovement.GetCurrentDirection() == Direction.LEFT) RotateProjectile(p.transform, 180f);
			FireProjectile(p);
		}
	}
}
