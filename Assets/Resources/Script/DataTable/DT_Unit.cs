using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

[System.Serializable]
public class DT_Unit
{
    public static Dictionary<int, DT_Unit> unitInfoDictionary = new Dictionary<int, DT_Unit>();
    public static List<DT_Unit> unitListInfo = new List<DT_Unit>();

    public int Index;
    public int IsDeveloped;
    public string Name;
    public int RoleIndex;
    public int Health;
    public int DamageType;
    public int MultiHitCount;
    public string AttackType;
    public List<string> AttackTypes;
    public float Damage;
    public float AttackSpeed;
    public int Range;
    public int Armor;
    public int MagicArmor;
    public float MoveSpeed_X;
    public float MoveSpeed_Y;
    public int CriticalChance;
    public int CriticalRatio;
    public int MaxTraitCount;
    public int FixTraitType;
    public string Animation;

    public DT_Unit() { }

    public void Initialize()
    {
        AttackTypes = new List<string>();
        if (string.IsNullOrEmpty(AttackType) == false)
        {
            var attackTypeSplit = AttackType.Split(":");
            AttackTypes.AddRange(attackTypeSplit);
        }
    }

    public DT_Unit(in DT_Unit _other) // 값 복사
    {
        Index = _other.Index;
        IsDeveloped = _other.IsDeveloped;
        Name = _other.Name;
        RoleIndex = _other.RoleIndex;
        Health = _other.Health;
        DamageType = _other.DamageType;
        MultiHitCount = _other.MultiHitCount;
        AttackType = _other.AttackType;
        Damage = _other.Damage;
        AttackSpeed = _other.AttackSpeed;
        Range = _other.Range;
        Armor = _other.Armor;
        MagicArmor = _other.MagicArmor;
        MoveSpeed_X = _other.MoveSpeed_X;
        MoveSpeed_Y = _other.MoveSpeed_Y;
        CriticalChance = _other.CriticalChance;
        CriticalRatio = _other.CriticalRatio;
        MaxTraitCount = _other.MaxTraitCount;
        FixTraitType = _other.FixTraitType;
        Animation = _other.Animation;

        Initialize();
    }

    public static DT_Unit GetInfoByIndex(int _index)
    {
        if (unitInfoDictionary.TryGetValue(_index, out var info))
        {
            return info;
        }
        Debug.LogWarning($"Index {_index} not found in InfoManager.");
        return null;
    }

    public string GetColorName(string _color)
    {
        return $"<color={_color}>{Name}</color>";
    }

    public EDamageType GetDamageType()
    {
        return (EDamageType)DamageType;
    }

    public bool IsCritical()
    {
        var rand = Random.Range(0, 9999);

        return rand < CriticalChance;
    }

    public float GetRange()
    {
        return (float)Range * ConstValue.RangeCoefficient;
    }

    public string GetRoleName()
    {
        return DT_Role.GetInfoByIndex(RoleIndex).Name;
    }

    public string GetAttackType(int _index)
    {
        if(_index < AttackTypes.Count)
        {
            return AttackTypes[_index];
        }

        return string.Empty;
    }
}

[System.Serializable]
public class Unit_FieldData
{
    private DT_Unit unit;
    public float FullHp;
    public float Hp;
    public float NormalAction_LeftCoolTime;
    public bool isDead;
    public Dictionary<int, DT_Trait> traits;

    private const float AddingFontHeightCoefficient = 0.3f;
    private const float AddingDelayTimeCoefficient = 0.18f;

    public Unit_FieldData(DT_Unit _unit)
    {
        this.unit = _unit;
        FullHp = _unit.Health;
        Hp = FullHp;
        NormalAction_LeftCoolTime = 1 / _unit.AttackSpeed;
        isDead = false;
    }

    public void Update(float _deltaTime)
    {
        NormalAction_LeftCoolTime -= _deltaTime;
    }

    public void Attack()
    {
        NormalAction_LeftCoolTime = 1 / unit.AttackSpeed;
    }

    public bool Hit(EDamageType _damageType, List<DamageInfo> _damageList, Vector3 _position)
    {
        if (IsDead())
            return false;

        var myArmor = unit.Armor;
        var myMagicArmor = unit.MagicArmor;

        for (int i = 0; i < _damageList.Count; i++)
        {
            var damageInfo = _damageList[i];
            var convertDamage = damageInfo.damage;

            if (_damageType == EDamageType.Magical)
            {
                // 마법 공격 효과
                convertDamage = damageInfo.damage - myMagicArmor;
            }
            else if (_damageType == EDamageType.Physical)
            {
                // 물리 공격 효과
                convertDamage = damageInfo.damage - myArmor;
            }
            else if (_damageType == EDamageType.True)
            {
                convertDamage = damageInfo.damage;
            }

            // 1 보다 작은 피해는 무시한다.
            if(1 <= convertDamage)
            {
                Hp -= convertDamage;
            }

            var addingFontHeight = i * AddingFontHeightCoefficient;
            var addingDelayTime = i * AddingDelayTimeCoefficient;
            DamageFont.Spawn(_position + new Vector3(0, 1 + (addingFontHeight), 0), convertDamage, QUtility.UIUtility.GetDamageColor(damageInfo.isCritical), addingDelayTime);
        }

        // 죽었는지 체크
        if (IsDead())
        {
            Debug.Log($"{unit.Name} 사망");
        }

        return true;
    }

    public bool IsDead()
    {
        return Hp <= 0;
    }
}


public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_Unit()
    {
        Debug.Log("Inltialize_DT_Unit");
        List<DT_Unit> infoList = DataLoader.Instance.LoadCSV<DT_Unit>(Path.Combine(Application.dataPath, "Resources/DataSet/Unit.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            DT_Unit.unitInfoDictionary[info.Index] = info;
            if(info.IsDeveloped == 1)
            {
                info.Initialize();
                DT_Unit.unitListInfo.Add(info);
                Debug.Log($"Index: {info.Index}, Name: {info.Name}");
            }
        }
    }
}