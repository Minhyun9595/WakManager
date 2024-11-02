using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.PackageManager;
using UnityEngine;

[System.Serializable]
public class Unit
{
    public int Index;
    public int IsDeveloped;
    public string Name;
    public string Role;
    public int Health;
    public int DamageType;
    public int MultiHitCount;
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

    public Unit() { }

    public Unit(in Unit other) // 값 복사
    {
        Index = other.Index;
        IsDeveloped = other.IsDeveloped;
        Name = other.Name;
        Role = other.Role;
        Health = other.Health;
        DamageType = other.DamageType;
        MultiHitCount = other.MultiHitCount;
        Damage = other.Damage;
        AttackSpeed = other.AttackSpeed;
        Range = other.Range;
        Armor = other.Armor;
        MagicArmor = other.MagicArmor;
        MoveSpeed_X = other.MoveSpeed_X;
        MoveSpeed_Y = other.MoveSpeed_Y;
        CriticalChance = other.CriticalChance;
        CriticalRatio = other.CriticalRatio;
        MaxTraitCount = other.MaxTraitCount;
        FixTraitType = other.FixTraitType;
        Animation = other.Animation;
    }

    public string GetColorName(string color)
    {
        return $"<color={color}>{Name}</color>";
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
}

[System.Serializable]
public class Unit_FieldData
{
    private Unit unit;
    public float FullHp;
    public float Hp;
    public float NormalAction_LeftCoolTime;
    public bool isDead;

    private const float AddingFontHeightCoefficient = 0.3f;
    private const float AddingDelayTimeCoefficient = 0.18f;
    public Unit_FieldData(Unit unit)
    {
        this.unit = unit;
        FullHp = unit.Health;
        Hp = FullHp;
        NormalAction_LeftCoolTime = 1 / unit.AttackSpeed;
        isDead = false;
    }

    public void Update(float deltaTime)
    {
        NormalAction_LeftCoolTime -= deltaTime;
    }

    public void Attack()
    {
        NormalAction_LeftCoolTime = 1 / unit.AttackSpeed;
    }

    public bool Hit(EDamageType damageType, List<DamageInfo> damageList, Vector3 position)
    {
        if (IsDead())
            return false;

        var myArmor = unit.Armor;
        var myMagicArmor = unit.MagicArmor;

        for (int i = 0; i < damageList.Count; i++)
        {
            var damageInfo = damageList[i];
            var convertDamage = damageInfo.damage;

            if (damageType == EDamageType.Magical)
            {
                // 마법 공격 효과
                convertDamage = damageInfo.damage - myMagicArmor;
            }
            else if (damageType == EDamageType.Physical)
            {
                // 물리 공격 효과
                convertDamage = damageInfo.damage - myArmor;
            }
            else if (damageType == EDamageType.True)
            {
                convertDamage = damageInfo.damage;
            }

            Hp -= convertDamage;

            var addingFontHeight = i * AddingFontHeightCoefficient;
            var addingDelayTime = i * AddingDelayTimeCoefficient;
            DamageFont.Spawn(position + new Vector3(0, 1 + (addingFontHeight), 0), convertDamage, QUtility.UIUtility.GetDamageColor(damageInfo.isCritical), addingDelayTime);
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
    public Dictionary<int, Unit> infoDictionary = new Dictionary<int, Unit>();
    public List<Unit> listInfo = new List<Unit>();

    public void Inltialize_DT_Unit()
    {
        Debug.Log("Inltialize_DT_Unit");
        List<Unit> infoList = DataLoader.Instance.LoadCSV<Unit>(Path.Combine(Application.dataPath, "Resources/DataSet/Unit.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            infoDictionary[info.Index] = info;
            listInfo.Add(info); 
            Debug.Log($"Index: {info.Index}, Name: {info.Name}");
        }
    }

    public Unit GetInfoByIndex(int index)
    {
        if (infoDictionary.TryGetValue(index, out var info))
        {
            return info;
        }
        Debug.LogWarning($"Index {index} not found in InfoManager.");
        return null;
    }

}