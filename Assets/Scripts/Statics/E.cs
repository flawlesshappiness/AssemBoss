using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class E {

}

public enum AttackType { JUMP, PROJECTILE }
public enum Direction { NONE, UP, RIGHT, DOWN, LEFT }
public enum Size { SMALL, MEDIUM, LARGE }
public enum Approach { TOWARDS, AWAYFROM }
public enum ProjectileDirection { STRAIGHT_FORWARD, CIRCLE, CIRCLE_HALF_UP, CIRCLE_HALF_DOWN, CIRCLE_HALF_FORWARD }

public enum NextAttackCategory { HEALTH, POSITION }
public enum NextAttackValueHealth { LOW, MED, HIGH }
public enum NextAttackValuePosition { LEFT, MID, RIGHT }