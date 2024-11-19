using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DT_TierInfo
{
    public static Dictionary<EUnitTier, DT_TierInfo> infoDictionary = new Dictionary<EUnitTier, DT_TierInfo>();
    public static List<DT_TierInfo> listInfo = new List<DT_TierInfo>();

    public int Index;
    public string Type;
    public string Desc;
    public int ConditionStatMin;
    public int ConditionStatMax;
    public int RecurtCost;

    public EUnitTier eUnitTier;

    public DT_TierInfo() { }

    public void Set()
    {
        eUnitTier = (EUnitTier)Enum.Parse(typeof(EUnitTier), Type);
    }


    public static DT_TierInfo GetInfoByIndex(EUnitTier eUnitTier)
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
        List<DT_TierInfo> infoList = DataLoader.Instance.LoadCSV<DT_TierInfo>(Path.Combine(Application.dataPath, "Resources/DataSet/TierInfo.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            info.Set();
            DT_TierInfo.infoDictionary[info.eUnitTier] = info;
            DT_TierInfo.listInfo.Add(info);
        }
    }
}