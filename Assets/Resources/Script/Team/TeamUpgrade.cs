using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


[System.Serializable]
public class TeamUpgrade
{
    public enum UpgradeType
    {
        FindUnit,               // 인재 발굴
        UnitAnalysis,           // 분석 능력
        Facility,               // 시설 관리
        RecruitmentOfCoachingStaff // 코칭 스태프 영입
    }

    private static readonly string[] UpgradeKeys = new string[]
    {
        "인재 발굴",
        "분석 능력",
        "시설 관리",
        "코칭 스태프 영입"
    };

    public int[] UpgradeLevels = new int[System.Enum.GetValues(typeof(UpgradeType)).Length];

    // 현재 업그레이드 정보를 가져오는 함수
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

    // 업그레이드 시도 함수
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
