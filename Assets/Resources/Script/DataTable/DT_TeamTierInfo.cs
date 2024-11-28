using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DT_TeamTierInfo
{
    public static Dictionary<ETeamTier, DT_TeamTierInfo> infoDictionary = new Dictionary<ETeamTier, DT_TeamTierInfo>();
    public static List<DT_TeamTierInfo> listInfo = new List<DT_TeamTierInfo>();

    // Index	Type	Desc	�� ���� ����
    public int Index;
    public string Type;
    public string Desc;

    public string Challenger;
    public string Master;
    public string Gold;
    public string Silver;
    public string Bronze;
    public string Iron;

    public Dictionary<EUnitTier, KeyValuePair<int, int>> keyValuePairs = new Dictionary<EUnitTier, KeyValuePair<int, int>>();

    public DT_TeamTierInfo() { }

    public void Set()
    {
        // �� ����� MinMax �� ����
        SetMinMax(EUnitTier.Challenger, Challenger);
        SetMinMax(EUnitTier.Master, Master);
        SetMinMax(EUnitTier.Gold, Gold);
        SetMinMax(EUnitTier.Silver, Silver);
        SetMinMax(EUnitTier.Bronze, Bronze);
        SetMinMax(EUnitTier.Iron, Iron);
    }

    private void SetMinMax(EUnitTier tier, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            // �����Ͱ� ���� ��� ����
            return;
        }

        if (value.Equals("Left", StringComparison.OrdinalIgnoreCase))
        {
            // "Left"�� ��� MinMax�� 99�� ����
            keyValuePairs[tier] = new KeyValuePair<int, int>(99, 99);
            return;
        }

        string[] parts = value.Split(':'); // MinMax ���� "Min:Max" �������� ���޵�
        if (parts.Length == 2 &&
            int.TryParse(parts[0], out int min) &&
            int.TryParse(parts[1], out int max))
        {
            keyValuePairs[tier] = new KeyValuePair<int, int>(min, max);
        }
        else
        {
            Debug.LogWarning($"Invalid MinMax format for {tier}: {value}");
        }
    }

    public static DT_TeamTierInfo GetInfoByIndex(ETeamTier eTeamTier)
    {
        if (infoDictionary.TryGetValue(eTeamTier, out var info))
        {
            return info;
        }

        Debug.LogWarning($"Index {eTeamTier.ToString()} not found in InfoManager.");
        return null;
    }
}
public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_TeamTierInfo()
    {
        List<DT_TeamTierInfo> infoList = DataLoader.Instance.LoadCSV<DT_TeamTierInfo>(Path.Combine(Application.dataPath, "Resources/DataSet/TeamTierInfo.csv"));

        // Dictionary�� �����͸� ����
        foreach (var info in infoList)
        {
            info.Set();
            DT_TeamTierInfo.infoDictionary[(ETeamTier)info.Index] = info;
            DT_TeamTierInfo.listInfo.Add(info);
        }
    }
}