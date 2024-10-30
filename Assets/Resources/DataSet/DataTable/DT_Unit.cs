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

    public Unit(in Unit other) // 값 생성
    {
        Index = other.Index;
        IsDeveloped = other.IsDeveloped;
        Name = other.Name;
        Role = other.Role;
        Health = other.Health;
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