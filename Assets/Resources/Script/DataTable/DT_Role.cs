using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DT_Role
{
    public static Dictionary<int, DT_Role> infoDictionary = new Dictionary<int, DT_Role>();
    public static List<DT_Role> listInfo = new List<DT_Role>();

    public int Index;
    public string Name;
    public string Characteristic_1;
    public string Characteristic_2;

    public DT_Role() { }

    public static DT_Role GetInfoByIndex(int index)
    {
        if (infoDictionary.TryGetValue(index, out var info))
        {
            return info;
        }
        Debug.LogWarning($"Index {index} not found in InfoManager.");
        return null;
    }
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_Role()
    {
        Debug.Log("Inltialize_DT_Role");
        List<DT_Role> infoList = DataLoader.Instance.LoadCSV<DT_Role>(Path.Combine(Application.dataPath, "Resources/DataSet/Role.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            DT_Role.infoDictionary[info.Index] = info;
            DT_Role.listInfo.Add(info);
        }
    }
}
