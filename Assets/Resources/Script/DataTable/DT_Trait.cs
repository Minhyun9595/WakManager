using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DT_Trait
{
    public static Dictionary<int, DT_Trait> traitInfoDictionary = new Dictionary<int, DT_Trait>();
    public static List<DT_Trait> traitListInfo = new List<DT_Trait>();

    public int TraitIndex;
    public string Type;
    public int Rank;
    public string Name;
    public string IconSprite;
    public string Desc1;
    public string Desc2;
    public string Desc3;
    public int Value1;
    public int Value2;

    public DT_Trait() { }

    public static DT_Trait GetInfoByIndex(int index)
    {
        if (traitInfoDictionary.TryGetValue(index, out var info))
        {
            return info;
        }
        Debug.LogWarning($"Index {index} not found in InfoManager.");
        return null;
    }

    public static DT_Trait GetInfoByIndex(string type, int rank)
    {
        foreach (var trait in traitListInfo)
        {
            if (trait.Type == type && trait.Rank == rank)
            {
                return trait;
            }
        }
        Debug.LogWarning($"Trait with Type {type} and Rank {rank} not found in InfoManager.");
        return null;
    }
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_Trait()
    {
        Debug.Log("Inltialize_DT_Trait");
        List<DT_Trait> infoList = DataLoader.Instance.LoadCSV<DT_Trait>(Path.Combine(Application.dataPath, "Resources/DataSet/Trait.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            DT_Trait.traitInfoDictionary[info.TraitIndex] = info;
            DT_Trait.traitListInfo.Add(info);
            Debug.Log($"Index: {info.TraitIndex}, Name: {info.Name}");
        }
    }
}
