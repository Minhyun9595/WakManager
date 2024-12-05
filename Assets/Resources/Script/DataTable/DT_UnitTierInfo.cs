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
    public string TraitCountRatio;
    public int Value;

    public EUnitTier eUnitTier;
    private List<KeyValuePair<int, int>> traitCountProbability = new List<KeyValuePair<int, int>>();


    public DT_UnitTierInfo() { }

    public void Set()
    {
        eUnitTier = (EUnitTier)Enum.Parse(typeof(EUnitTier), Type);
        InitializeTraitCountProbability();
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

    private void InitializeTraitCountProbability()
    {
        if (string.IsNullOrWhiteSpace(TraitCountRatio))
        {
            Debug.LogWarning("TraitCountRatio is empty or null.");
            return;
        }

        string[] parts = TraitCountRatio.Split(';');
        int cumulativeProbability = 0;

        foreach (var part in parts)
        {
            string[] keyValue = part.Split(':');
            if (keyValue.Length == 2 && int.TryParse(keyValue[0], out int value) && int.TryParse(keyValue[1], out int probability))
            {
                cumulativeProbability += probability;
                traitCountProbability.Add(new KeyValuePair<int, int>(value, cumulativeProbability));
            }
            else
            {
                Debug.LogWarning($"Invalid TraitCountRatio format: {part}");
            }
        }
    }

    public int GetRandomTraitCount()
    {
        if (traitCountProbability.Count == 0)
        {
            Debug.LogError("TraitCountProbability is not initialized.");
            return -1;
        }

        int randomValue = UnityEngine.Random.Range(1, traitCountProbability[^1].Value + 1); // 1부터 총 확률까지의 값
        foreach (var pair in traitCountProbability)
        {
            if (randomValue <= pair.Value)
            {
                return pair.Key;
            }
        }

        Debug.LogError("Random value did not match any trait count.");
        return -1;
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