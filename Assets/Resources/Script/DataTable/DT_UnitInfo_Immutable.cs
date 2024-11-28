using QUtility;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DT_UnitInfo_Immutable
{
    public static Dictionary<int, DT_UnitInfo_Immutable> infoDictionary = new Dictionary<int, DT_UnitInfo_Immutable>();
    public static List<DT_UnitInfo_Immutable> listInfo = new List<DT_UnitInfo_Immutable>();

    public int Index;
    public string Name;
    public int IsDeveloped;
    public int RoleIndex;
    public int MaxTraitCount;
    public int FixTraitType;
    public int IsRangeUnit;
    public string Animator;
    public string AttackType;
    public string AttackPrefabName;
    public string NormalSkills;
    public string SpecialSkills;
    public string OfficeDialogs;

    // Initialize 데이터
    public List<string> SkillNameList;
    public List<string> OfficeDialogList;

    public DT_UnitInfo_Immutable() { }

    internal void Init()
    {
        // 스킬 이름 List
        SkillNameList = new List<string>();
        StringUtility.AddSplitList(ref SkillNameList, NormalSkills, ":");
        StringUtility.AddSplitList(ref SkillNameList, SpecialSkills, ":");

        // 사무실 대사 List
        OfficeDialogList = new List<string>();
        StringUtility.AddSplitList(ref OfficeDialogList, OfficeDialogs, ":");
    }

    public static DT_UnitInfo_Immutable GetInfoByIndex(int rank)
    {
        if (infoDictionary.TryGetValue(rank, out var info))
        {
            return info;
        }

        Debug.LogWarning($"Index {rank} not found in InfoManager.");
        return null;
    }

    public static List<int> GetKeys()
    {
        return infoDictionary.Keys.ToList();
    }

    public bool CheckRangeUnit()
    {
        return IsRangeUnit == 1;
    }
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_UnitInfo_Immutable()
    {
        List<DT_UnitInfo_Immutable> infoList = DataLoader.Instance.LoadCSV<DT_UnitInfo_Immutable>(Path.Combine(Application.dataPath, "Resources/DataSet/UnitInfo_Immutable.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            if(info.IsDeveloped == 1)
            {
                info.Init();
                DT_UnitInfo_Immutable.infoDictionary[info.Index] = info;
                DT_UnitInfo_Immutable.listInfo.Add(info);
            }
        }
    }
}
