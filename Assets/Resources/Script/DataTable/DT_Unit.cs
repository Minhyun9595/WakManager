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
    public int MeleeDamageType;
    public int MeleeDamageCount;
    public int MeleeDamage;
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

    public Unit() { }

    public Unit(in Unit other) // 값 복사
    {
        Index = other.Index;
        IsDeveloped = other.IsDeveloped;
        Name = other.Name;
        Role = other.Role;
        Health = other.Health;
        MeleeDamageType = other.MeleeDamageType;
        MeleeDamageCount = other.MeleeDamageCount;
        MeleeDamage = other.MeleeDamage;
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
    }

    public string GetColorName(string color)
    {
        return $"<color={color}>{Name}</color>";
    }

    public EDamageType GetDamageType()
    {
        return (EDamageType)MeleeDamageType;
    }
}

public struct Unit_FieldData
{
    private Unit unit;
    public float FullHp;
    public float Hp;
    public float NormalAction_LeftCoolTime;

    public Unit_FieldData(Unit unit)
    {
        this.unit = unit;
        FullHp = unit.Health;
        Hp = FullHp;
        NormalAction_LeftCoolTime = 1 / unit.AttackSpeed;
    }

    public void Update(float deltaTime)
    {
        NormalAction_LeftCoolTime -= deltaTime;
    }

    public bool Hit(EDamageType damageType, float damage)
    {
        if (IsDead())
            return false;

        var myArmor = unit.Armor;
        var myMagicArmor = unit.MagicArmor;
        
        if(damageType == EDamageType.Magical)
        {
            // 마법 공격 효과
            Hp -= damage - myMagicArmor;
        }
        else if(damageType == EDamageType.Physical)
        {
            // 물리 공격 효과
            Hp -= damage - myArmor;
        }
        else if(damageType == EDamageType.True)
        {
            // 물리 공격 효과
            Hp -= damage;
        }

        // 죽었는지 체크
        if(IsDead())
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