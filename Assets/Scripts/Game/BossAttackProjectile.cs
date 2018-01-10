using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossAttackProjectile : BossAttack {

	private int amount;
	private float speedMove;
	private float speedRotation;
	private float delay;
	private float scale;
	private ProjectileDirection direction;

	private float cdWait;

	private int circleState = 0;
	private float circleAngle;

	public override void Init ()
	{
		DataAttackShoot d = (DataAttackShoot)data;
		amount = d.projectileAmount.value;
		speedMove = d.speedMove.value;
		speedRotation = d.speedRotation.value;
		delay = d.spawnDelay.value;
		scale = d.scale.value;
		direction = (ProjectileDirection)Enum.Parse(typeof(ProjectileDirection), d.projectileDirection.value);

		if(direction == ProjectileDirection.CIRCLE)
		{
			circleState = 1;
			circleAngle = 360f / amount;
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

	void SpawnProjectile()
	{
		var g = mgPrefab.SpawnPrefabGame("Projectile");
		g.transform.position = transform.position;
		g.transform.localScale = new Vector3(scale, scale, scale);
		g.transform.RotateAround(g.transform.position, Vector3.forward, circleState++ * circleAngle);

		//Fire
		var p = g.GetComponent<Projectile>();
		p.Fire(speedMove, speedRotation, boss.GetPlayer());
	}
}
