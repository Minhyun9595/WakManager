using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitReport
{
    public int unitIndex;
    public string unitUniqueID;
    public int DamagePoint;
    public int GetDamagePoint;
    public int HealPoint;

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
        UnitReport unitReport = new UnitReport();
        unitReport.unitIndex = unitAI.blackboard.realUnitData.unitIndex;
        unitReport.unitUniqueID = unitAI.blackboard.realUnitData.unitUniqueID;

        //unitReport.DamagePoint = unitAI.DamagePoint;
        //unitReport.GetDamagePoint = unitAI.GetDamagePoint;
        //unitReport.HealPoint = unitAI.HealPoint;

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
