using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPrefabType
{
    // ÀÌÆåÆ® °ü·Ã
    DamageFont,
    Projectile_Wizzard,
    Projectile_Wizzard_Hit,



    // À¯´Ö
    Unit,
}

public enum EJobType
{
    Tanker,
    Warrior,
    Magician,
    Assassin,
    Archer,
    Supporter
}

public enum EDamageType
{
    Physical,
    Magical,
    True,
}

public enum EAnimationType
{ 
    Idle,
    Move,
    Jump,
    Hit,
    Death,
    Attack1,
    Attack2
}

public class ConstValue : CustomSingleton<ConstValue>
{
    public static float timeValue = 1.0f;
    public static float speedRatio = 0.5f;
    public static float CriticalDamageCoefficient = 0.0001f;
    public static float RangeCoefficient = 0.01f;
}
