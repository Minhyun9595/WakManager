using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DT_Const
{
    public static Dictionary<string, DT_Const> infoDictionary = new Dictionary<string, DT_Const>();
    public static List<DT_Const> listInfo = new List<DT_Const>();

    public string Name;
    public int Value;
    public DT_Const() { }

    public static int GetInfoByIndex(string name)
    {
        name = name.ToLower();
        if (infoDictionary.TryGetValue(name, out var info))
        {
            return info.Value;
        }

        Debug.LogWarning($"Index {name} not found in InfoManager.");
        return 0;
    }
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_Const()
    {
        List<DT_Const> infoList = DataLoader.Instance.LoadCSV<DT_Const>(Path.Combine(Application.dataPath, "Resources/DataSet/Const.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            info.Name = info.Name.ToLower();
            DT_Const.infoDictionary[info.Name] = info;
            DT_Const.listInfo.Add(info);
        }
    }
}
