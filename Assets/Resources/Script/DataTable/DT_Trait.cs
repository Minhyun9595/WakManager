using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DT_Trait
{
    public static Dictionary<int, DT_Trait> infoDictionary = new Dictionary<int, DT_Trait>();
    public static Dictionary<string, Dictionary<int, DT_Trait>> infoDictionary_ByType = new Dictionary<string, Dictionary<int, DT_Trait>>();
    public static List<DT_Trait> listInfo = new List<DT_Trait>();

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
        if (infoDictionary.TryGetValue(index, out var info))
        {
            return info;
        }
        Debug.LogWarning($"Index {index} not found in InfoManager.");
        return null;
    }

    public static Dictionary<int, DT_Trait> GetInfoByIndex_Type(string type)
    {
        if(infoDictionary_ByType.TryGetValue(type, out var infoDic))
        {
            return infoDic;
        }

        return null;
    }
    public static List<string> GetTypes()
    {
        return infoDictionary_ByType.Keys.ToList();
    }

    public static DT_Trait GetInfoByIndex(string type, int rank)
    {
        foreach (var trait in listInfo)
        {
            if (trait.Type == type && trait.Rank == rank)
            {
                return trait;
            }
        }
        Debug.LogWarning($"Trait with Type {type} and Rank {rank} not found in InfoManager.");
        return null;
    }

    public string GetRankString()
    {
        var result = "F";
        if(Rank == 5)       { result = "D"; }
        else if(Rank == 4) { result = "C";  }
        else if(Rank == 3) { result = "B";  }
        else if(Rank == 2) { result = "A";  }
        else if(Rank == 1) { result = "S"; }

        return result;
    }
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_Trait()
    {
        List<DT_Trait> infoList = DataLoader.Instance.LoadCSV<DT_Trait>(Path.Combine(Application.dataPath, "Resources/DataSet/Trait.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            DT_Trait.infoDictionary[info.TraitIndex] = info;
            DT_Trait.listInfo.Add(info);

            if(DT_Trait.infoDictionary_ByType.ContainsKey(info.Type) == false)
            {
                DT_Trait.infoDictionary_ByType.TryAdd(info.Type, new Dictionary<int, DT_Trait>());
            }

            DT_Trait.infoDictionary_ByType[info.Type].Add(info.TraitIndex, info);
        }
    }
}
