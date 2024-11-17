using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitReport
{
    public int unitIndex;
    public string unitUniqueID;

    public float Damage_Total;
    public float Damage_Physical;
    public float Damage_Magical;
    public float Damage_True;
    public float DamageReceive_Total;
    public float DamageReceive_Physical;
    public float DamageReceive_Magical;
    public float DamageReceive_True;

    public float Heal_Amount;
    public float Heal_Receive;
    public float MoveDistance;

    public int KillCount;
    public int DeathCount;
    public int AssistCount;

    public void Set(Unit_AI unit_AI)
    {
        var bb = unit_AI.blackboard;
        unitIndex = unit_AI.blackboard.realUnitData.unitIndex;
        unitUniqueID = unit_AI.blackboard.realUnitData.unitUniqueID;

    }
}

[System.Serializable]
public class TeamBattleReport
{
    public List<UnitReport> unitReports;

    public TeamBattleReport()
    {
        unitReports = new List<UnitReport>();
    }

    public void AddUnitReport(Unit_AI unitAI)
    {
        UnitReport unitReport = unitAI.blackboard.unitReport;
        unitReport.Set(unitAI);
        unitReports.Add(unitReport);
    }
}

[System.Serializable]
public class BattleReport
{
    public long battleIndex;
    public string time;
    [SerializeField] public TeamBattleReport firstTeamBattleReport;
    [SerializeField] public TeamBattleReport secondTeamBattleReport;

    public BattleReport(List<Unit_AI> firstUnitAIList, List<Unit_AI> secondUnitAIList)
    {
        battleIndex = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        time = DateTime.Now.ToString("yyyy³â-MM¿ù-ddÀÏ HH:mm");

        firstTeamBattleReport = new TeamBattleReport();
        secondTeamBattleReport = new TeamBattleReport();

        foreach (Unit_AI unitAI in firstUnitAIList)
        {
            firstTeamBattleReport.AddUnitReport(unitAI);
        }

        foreach (Unit_AI unitAI in secondUnitAIList)
        {
            secondTeamBattleReport.AddUnitReport(unitAI);
        }
    }
}
