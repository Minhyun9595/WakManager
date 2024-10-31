using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPrefabType
{
    DamageFont,
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

public class ConstValue : CustomSingleton<ConstValue>
{
    public static float timeValue = 1.0f;
    public static float speedRatio = 0.5f;

}
