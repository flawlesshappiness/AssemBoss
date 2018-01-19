using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class E {

}

public enum AttackType { JUMP, PROJECTILE, SEQUENCE }
public enum Direction { NONE, UP, RIGHT, DOWN, LEFT }
public enum Approach { TOWARDS, AWAYFROM }
public enum ProjectileDirection { FORWARDS, CIRCLE }

public enum NextAttackCategory { HEALTH, POSITION }
public enum NextAttackValueHealth { LOW, MED, HIGH }
public enum NextAttackValuePosition { LEFT, MID, RIGHT }