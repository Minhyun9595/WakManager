using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DT_Market
{
    public static Dictionary<int, DT_Market> infoDictionary = new Dictionary<int, DT_Market>();
    public static List<DT_Market> listInfo = new List<DT_Market>();

    public int Index;
    public string Name;

    // 확률
    public int Challenger;
    public int Master;
    public int Gold;
    public int Silver;
    public int Bronze;
    public int Iron;

    public int SearchPrice;

    public Dictionary<EUnitTier, int> unitCounts = new Dictionary<EUnitTier, int>();

    public DT_Market() { }

    public void Set()
    {
        unitCounts[EUnitTier.Challenger] = Challenger;
        unitCounts[EUnitTier.Master] = Master;
        unitCounts[EUnitTier.Gold] = Gold;
        unitCounts[EUnitTier.Silver] = Silver;
        unitCounts[EUnitTier.Bronze] = Bronze;
        unitCounts[EUnitTier.Iron] = Iron;
    }

    public EUnitTier GetRandomCardTier()
    {
        // 확률의 총합 계산
        int totalProbability = 0;
        foreach (var pair in unitCounts)
        {
            totalProbability += pair.Value;
        }

        if (totalProbability == 0)
        {
            Debug.LogError("Total probability is 0. No cards can be selected.");
            return EUnitTier.Iron; // None은 기본값으로 사용됩니다. 필요시 정의해야 합니다.
        }

        // 랜덤 값 생성
        int randomValue = Random.Range(1, totalProbability + 1);
        int cumulativeProbability = 0;

        // 확률에 따라 등급 선택
        foreach (var pair in unitCounts)
        {
            cumulativeProbability += pair.Value;
            if (randomValue <= cumulativeProbability)
            {
                return pair.Key;
            }
        }

        Debug.LogError("No card tier selected. This should not happen.");
        return EUnitTier.Iron;
    }

    public static DT_Market GetInfoByIndex(int index)
    {
        if (infoDictionary.TryGetValue(index, out var info))
        {
            return info;
        }

        Debug.LogWarning($"Index {index} not found in Market Info.");
        return null;
    }
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Initialize_DT_Market()
    {
        List<DT_Market> infoList = DataLoader.Instance.LoadCSV<DT_Market>(Path.Combine(Application.dataPath, "Resources/DataSet/Market.csv"));

        foreach (var info in infoList)
        {
            info.Set();
            DT_Market.infoDictionary[info.Index] = info;
            DT_Market.listInfo.Add(info);
        }
    }
}