using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DT_Condition
{
    public static Dictionary<EUnitConditionType, DT_Condition> infoDictionary = new Dictionary<EUnitConditionType, DT_Condition>();
    public static List<DT_Condition> listInfo = new List<DT_Condition>();

    public int Index;
    public string Type;
    public string Range;
    public string Desc;
    public int Value1;
    public int Value2;

    public EUnitConditionType eUnitConditionType;
    public int RangeMin;
    public int RangeMax;

    public DT_Condition() { }

    public void Set()
    {
        eUnitConditionType = (EUnitConditionType)Enum.Parse(typeof(EUnitConditionType), Type);
        var rangeSplit = Range.Split("~");
        RangeMin = int.Parse(rangeSplit[0]);
        RangeMax = int.Parse(rangeSplit[1]);
    }

    public static DT_Condition GetInfoByIndex(EUnitConditionType eUnitConditionType)
    {
        if (infoDictionary.TryGetValue(eUnitConditionType, out var info))
        {
            return info;
        }

        Debug.LogWarning($"DT_Condition Index {eUnitConditionType.ToString()} not found");
        return null;
    }

    public static DT_Condition GetCondition(int _conditionPoint)
    {
        foreach(var condition in listInfo)
        {
            if (condition.RangeMin <= _conditionPoint && _conditionPoint <= condition.RangeMax)
                return condition;
        }

        return null;
    }
}
public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_Condition()
    {
        List<DT_Condition> infoList = DataLoader.Instance.LoadCSV<DT_Condition>(Path.Combine(Application.dataPath, "Resources/DataSet/Condition.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            info.Set();
            DT_Condition.infoDictionary[info.eUnitConditionType] = info;
            DT_Condition.listInfo.Add(info);
        }
    }
}