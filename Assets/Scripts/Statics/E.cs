using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class E {
	public static Direction Opposite(Direction dir)
	{
		switch(dir)
		{
		case Direction.LEFT: return Direction.RIGHT;
		case Direction.RIGHT: return Direction.LEFT;
		case Direction.UP: return Direction.DOWN;
		case Direction.DOWN: return Direction.UP;
		default: return Direction.NONE;
		}
	}
	public static DirectionHorizontal Opposite(DirectionHorizontal dir)
	{
		if(dir == DirectionHorizontal.LEFT) return DirectionHorizontal.RIGHT;
		else if(dir == DirectionHorizontal.RIGHT) return DirectionHorizontal.LEFT;
		else return DirectionHorizontal.NONE;
	}
	public static DirectionVertical Opposite(DirectionVertical dir)
	{
		if(dir == DirectionVertical.UP) return DirectionVertical.DOWN;
		else if(dir == DirectionVertical.DOWN) return DirectionVertical.UP;
		else return DirectionVertical.NONE;
	}
	public static Direction ToDirection(DirectionHorizontal dir)
	{
		if(dir == DirectionHorizontal.LEFT) return Direction.LEFT;
		else if(dir == DirectionHorizontal.RIGHT) return Direction.RIGHT;
		else return Direction.NONE;
	}
	public static Direction ToDirection(DirectionVertical dir)
	{
		if(dir == DirectionVertical.UP) return Direction.UP;
		else if(dir == DirectionVertical.DOWN) return Direction.DOWN;
		else return Direction.NONE;
	}
}

public enum AttackType { JUMP, PROJECTILE }
public enum Direction { NONE, UP, RIGHT, DOWN, LEFT }
public enum DirectionHorizontal { NONE, LEFT, RIGHT }
public enum DirectionVertical { NONE, UP, DOWN }
public enum Size { SMALL, MEDIUM, LARGE }
public enum Approach { TOWARDS, AWAYFROM }
public enum ProjectileDirection { STRAIGHT_FORWARD, CIRCLE, CIRCLE_HALF_UP, CIRCLE_HALF_DOWN, CIRCLE_HALF_FORWARD }

public enum NextAttackCategory { HEALTH, POSITION }
public enum NextAttackValueHealth { LOW, MED, HIGH }
public enum NextAttackValuePosition { LEFT, MID, RIGHT }