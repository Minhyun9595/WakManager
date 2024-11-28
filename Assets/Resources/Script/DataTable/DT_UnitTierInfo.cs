using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DT_UnitTierInfo
{
    public static Dictionary<EUnitTier, DT_UnitTierInfo> infoDictionary = new Dictionary<EUnitTier, DT_UnitTierInfo>();
    public static List<DT_UnitTierInfo> listInfo = new List<DT_UnitTierInfo>();

    public int Index;
    public string Type;
    public string Desc;
    public int ConditionStatMin;
    public int ConditionStatMax;
    public int RecurtCost;
    public int MonthCost;
    public int AddStatPointMin;
    public int AddStatPointMax;
    public int PotentialPointMin;
    public int PotentialPointMax;

    public EUnitTier eUnitTier;

    public DT_UnitTierInfo() { }

    public void Set()
    {
        eUnitTier = (EUnitTier)Enum.Parse(typeof(EUnitTier), Type);
    }


    public static DT_UnitTierInfo GetInfoByIndex(EUnitTier eUnitTier)
    {
        if (infoDictionary.TryGetValue(eUnitTier, out var info))
        {
            return info;
        }

        Debug.LogWarning($"Index {eUnitTier.ToString()} not found in InfoManager.");
        return null;
    }
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_TierInfo()
    {
        List<DT_UnitTierInfo> infoList = DataLoader.Instance.LoadCSV<DT_UnitTierInfo>(Path.Combine(Application.dataPath, "Resources/DataSet/UnitTierInfo.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            info.Set();
            DT_UnitTierInfo.infoDictionary[info.eUnitTier] = info;
            DT_UnitTierInfo.listInfo.Add(info);
        }
    }
}