using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DT_Skill
{
    public static Dictionary<string, DT_Skill> infoDictionary = new Dictionary<string, DT_Skill>();
    public static List<DT_Skill> listInfo = new List<DT_Skill>();

    // Name	Description	IconName	SkillType	PrefabName	CastingTime	CastingAnimation	CoolTime	Damage	
    // Value1	Value2	Value3	Value4	Value5	Value6	Value7

    public string Name;
    public string Description;
    public string IconName;
    public string SkillType;
    public string PrefabName;
    public float CastingTime;
    public string CastingAnimation;
    public float CoolTime;
    public float Damage;
    public float Value1;
    public float Value2;
    public float Value3;
    public float Value4;
    public float Value5;
    public float Value6;
    public float Value7;

    public DT_Skill() { }

    public static DT_Skill GetInfoByIndex(string name)
    {
        if (infoDictionary.TryGetValue(name, out var info))
        {
            return info;
        }

        Debug.LogWarning($"Index {name} not found in InfoManager.");
        return null;
    }
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_Skill()
    {
        List<DT_Skill> infoList = DataLoader.Instance.LoadCSV<DT_Skill>(Path.Combine(Application.dataPath, "Resources/DataSet/Skill.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            DT_Skill.infoDictionary[info.Name] = info;
            DT_Skill.listInfo.Add(info);
        }
    }
}
