using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


[System.Serializable]
public class TeamUpgrade
{
    public enum UpgradeType
    {
        FindUnit,               // ���� �߱�
        UnitAnalysis,           // �м� �ɷ�
        Facility,               // �ü� ����
        RecruitmentOfCoachingStaff // ��Ī ������ ����
    }

    private static readonly string[] UpgradeKeys = new string[]
    {
        "���� �߱�",
        "�м� �ɷ�",
        "�ü� ����",
        "��Ī ������ ����"
    };

    public int[] UpgradeLevels = new int[System.Enum.GetValues(typeof(UpgradeType)).Length];

    // ���� ���׷��̵� ������ �������� �Լ�
    public DT_TeamUpgrade GetCurrentUpgrade(UpgradeType type)
    {
        string key = UpgradeKeys[(int)type];
        int level = UpgradeLevels[(int)type];
        return DT_TeamUpgrade.GetInfoByIndex(key, level);
    }

    public bool CanLevelUp(UpgradeType type)
    {
        int index = (int)type;
        string key = UpgradeKeys[index];
        int level = UpgradeLevels[index];

        if (false == DT_TeamUpgrade.HaveNextUpgrade(key, level))
        {
            return false;
        }

        var dt_Current = DT_TeamUpgrade.GetInfoByIndex(key, level);
        var cost = dt_Current.Cost;

        return cost <= PlayerManager.Instance.PlayerTeamInfo.Money;
    }

    // ���׷��̵� �õ� �Լ�
    public bool TryLevelUp(UpgradeType type)
    {
        int index = (int)type;
        string key = UpgradeKeys[index];
        int level = UpgradeLevels[index];

        if (false == DT_TeamUpgrade.HaveNextUpgrade(key, level))
        {
            return false;
        }

        var dt_Current = DT_TeamUpgrade.GetInfoByIndex(key, level);
        var cost = dt_Current.Cost;

        if (!PlayerManager.Instance.PlayerTeamInfo.ReduceMoney(cost))
        {
            return false;
        }

        UpgradeLevels[index]++;
        return true;
    }

}
