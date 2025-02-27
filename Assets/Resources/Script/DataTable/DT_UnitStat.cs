using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DT_UnitStat
{
    public static Dictionary<int, DT_UnitStat> infoDictionary = new Dictionary<int, DT_UnitStat>();
    public static List<DT_UnitStat> listInfo = new List<DT_UnitStat>();

    public int Index;
    public string Name;
    public int Health;
    public string DamageType;
    public float Damage;
    public int MultiHitCount;
    public float AttackSpeed;
    public int Range;
    public int Armor;
    public int MagicArmor;
    public float MoveSpeed_X;
    public float MoveSpeed_Y;
    public int CriticalChance;
    public int CriticalRatio;

    public DT_UnitStat() { }

    public static DT_UnitStat GetInfoByIndex(int rank)
    {
        if (infoDictionary.TryGetValue(rank, out var info))
        {
            return info;
        }

        Debug.LogWarning($"Index {rank} not found in InfoManager.");
        return null;
    }

    public EDamageType GetDamageType()
    {
        switch (DamageType)
        {
            case "Physical":
                return EDamageType.Physical;
            case "Magic":
                return EDamageType.Magical;
            case "True":
                return EDamageType.True;
            default:
                return EDamageType.Physical;
        }
    }

    public string GetDamageTypeText()
    {
        switch (DamageType)
        {
            case "Physical":
                return "물리";
            case "Magic":
                return "마법";
            case "True":
                return "고정";
            default:
                return "물리";
        }
    }
    
    public string GetSpeedText()
    {
        var result = "";
        if(MoveSpeed_X <= 2.0f)
        {
            result = "느림";
        }
        else if (MoveSpeed_X <= 2.5f)
        {
            result = "보통";
        }
        else if (MoveSpeed_X <= 3.0f)
        {
            result = "빠름";
        }
        else if (MoveSpeed_X <= 3.5f)
        {
            result = "매우 빠름";
        }
        else
        {
            result = "매우 빠름";
        }

        return result;
    }
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_UnitStat()
    {
        List<DT_UnitStat> infoList = DataLoader.Instance.LoadCSV<DT_UnitStat>(Path.Combine(Application.dataPath, "Resources/DataSet/UnitStat.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            DT_UnitStat.infoDictionary[info.Index] = info;
            DT_UnitStat.listInfo.Add(info);
        }
    }
}
