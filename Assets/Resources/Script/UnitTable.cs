using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Unit
{
    public int Index;
    public string Name;
    public int Health;
    public int MeleeDamage;
    public float AttackSpeed;
    public int Range;
    public int Armor;
    public int MagicArmor;
    public float MoveSpeed;
}

public class UnitTable : CustomSingleton<UnitTable>
{
    public List<Unit> Units = new List<Unit>();
    private Dictionary<int, Unit> infoDictionary;

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        infoDictionary = new Dictionary<int, Unit>();

        Debug.Log("Initialize");

        // DataManager에서 Info 데이터를 로드
        List<Unit> infoList = DataManager.Instance.LoadCSV<Unit>(Path.Combine(Application.dataPath, "Resources/DataSet/Unit.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            infoDictionary[info.Index] = info;
            Units.Add(info);
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