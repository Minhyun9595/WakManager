using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUnitTier
{ 
    WorldClass, // 월드 클래스
    LeagueStar, // 리그 최상급
    FirstTeam, // 주전급
    Rotation, // 로테이션
    Prospect, // 유망주
    SurplustoRequirements // 팀 수준 미달
}

public enum EUnitConditionType
{
    Superb, // 90 ~ 100
    Good, // 80 ~ 89
    Okay, // 65 ~ 79
    Poor, // 40 ~ 64
    VeryPoor, // 0 ~ 39
}

public enum ESceneType
{
    Menu,
    Lobby,
    InGame,
}


public enum EPrefabType
{
    // 이펙트 관련
    DamageFont,
    Projectile_Wizzard,
    Projectile_Wizzard_Hit,
    Projectile_Bezier,
    Projectile_Bezier_Hit,



    // 유닛
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
    Attack2,
    Skill,
}

public enum EAxsType
{
    X,
    Y,
    Z
}

public enum EPanelPrefabType
{
    Panel_SaveData,
    Panel_Market,
    Panel_TraitPopup,
    Panel_Squad,
}


public class ConstValue : CustomSingleton<ConstValue>
{
    public static float timeValue = 1.0f;
    public static float speedRatio = 0.5f;
    public static float CriticalDamageCoefficient = 0.0001f;
    public static float RangeCoefficient = 0.015f;
    public static int Layer_Character = 1000;
    public static int Layer_Effect = 10000;
}


public class StringUtility
{
    public static void AddSplitList(ref List<string> list, string input, string cut)
    {
        if (string.IsNullOrEmpty(input) == false)
        {
            var skillSplit = input.Split(cut);

            foreach (var skill in skillSplit)
            {
                list.Add(skill);
            }
        }
    }
}