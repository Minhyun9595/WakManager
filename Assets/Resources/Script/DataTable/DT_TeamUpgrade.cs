using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DT_TeamUpgrade
{
    public static Dictionary<string, Dictionary<int, DT_TeamUpgrade>> infoDictionary = new Dictionary<string, Dictionary<int, DT_TeamUpgrade>>();
    public static List<DT_TeamUpgrade> listInfo = new List<DT_TeamUpgrade>();

    public string Type;
    public int Level;
    public int Cost;
    public string Name;
    public string Desc;
    public int Value1;
    public int Value2;
    public int Value3;
    public int Value4;
    public string StrValue1;

    public DT_TeamUpgrade() { }

    public static DT_TeamUpgrade GetInfoByIndex(string type, int level)
    {
        if (infoDictionary.TryGetValue(type, out var info))
        {
            if (info.TryGetValue(level, out var result))
            {
                return result;
            }
        }

        Debug.LogWarning($"Index {type} {level} not found in InfoManager.");
        return null;
    }

    public static bool HaveNextUpgrade(string type, int level)
    {
        if (infoDictionary.TryGetValue(type, out var info))
        {
            if (info.TryGetValue(level + 1, out var result))
            {
                return true;
            }
        }

        return false;
    }
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_TeamUpgrade()
    {
        List<DT_TeamUpgrade> infoList = DataLoader.Instance.LoadCSV<DT_TeamUpgrade>(Path.Combine(Application.dataPath, "Resources/DataSet/TeamUpgrade.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            if (!DT_TeamUpgrade.infoDictionary.ContainsKey(info.Type))
            {
                DT_TeamUpgrade.infoDictionary[info.Type] = new Dictionary<int, DT_TeamUpgrade>();
            }
            DT_TeamUpgrade.infoDictionary[info.Type][info.Level] = info;
            DT_TeamUpgrade.listInfo.Add(info);
        }
    }
}
