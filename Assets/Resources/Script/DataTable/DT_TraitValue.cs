using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DT_TraitValue
{
    public static Dictionary<int, DT_TraitValue> infoDictionary = new Dictionary<int, DT_TraitValue>();
    public static List<DT_TraitValue> listInfo = new List<DT_TraitValue>();

    public int Rank;
    public int Value;

    public DT_TraitValue() { }

    public static DT_TraitValue GetInfoByIndex(int rank)
    {
        if (infoDictionary.TryGetValue(rank, out var info))
        {
            return info;
        }

        Debug.LogWarning($"Index {rank} not found in InfoManager.");
        return null;
    }
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_TraitValue()
    {
        List<DT_TraitValue> infoList = DataLoader.Instance.LoadCSV<DT_TraitValue>(Path.Combine(Application.dataPath, "Resources/DataSet/TraitValue.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            DT_TraitValue.infoDictionary[info.Rank] = info;
            DT_TraitValue.listInfo.Add(info);
        }
    }
}
