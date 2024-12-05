using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

        Damage_Total = unit_AI.blackboard.unitReport.Damage_Total;
        Damage_Physical = unit_AI.blackboard.unitReport.Damage_Physical;
        Damage_Magical = unit_AI.blackboard.unitReport.Damage_Magical;
        Damage_True = unit_AI.blackboard.unitReport.Damage_True;

        DamageReceive_Total = unit_AI.blackboard.unitReport.DamageReceive_Total;
        DamageReceive_Physical = unit_AI.blackboard.unitReport.DamageReceive_Physical;
        DamageReceive_Magical = unit_AI.blackboard.unitReport.DamageReceive_Magical;
        DamageReceive_True = unit_AI.blackboard.unitReport.DamageReceive_True;

        Heal_Amount = unit_AI.blackboard.unitReport.Heal_Amount;
        Heal_Receive = unit_AI.blackboard.unitReport.Heal_Receive;
        MoveDistance = unit_AI.blackboard.unitReport.MoveDistance;
        KillCount = unit_AI.blackboard.unitReport.KillCount;
        DeathCount = unit_AI.blackboard.unitReport.DeathCount;
        AssistCount = unit_AI.blackboard.unitReport.AssistCount;
    }

    public void AddDamage(EDamageType eDamageType, float damage)
    {
        switch (eDamageType)
        {
            case EDamageType.Physical:
                Damage_Physical += damage;
                break;
            case EDamageType.Magical:
                Damage_Magical += damage;
                break;
            case EDamageType.True:
                Damage_True += damage;
                break;
        }
        Damage_Total += damage;
    }

    public void AddReceiveDamage(EDamageType eDamageType, float damage)
    {
        switch (eDamageType)
        {
            case EDamageType.Physical:
                DamageReceive_Physical += damage;
                break;
            case EDamageType.Magical:
                DamageReceive_Magical += damage;
                break;
            case EDamageType.True:
                DamageReceive_True += damage;
                break;
        }
        DamageReceive_Total += damage;
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

[System.Serializable]
public class BattleResult
{
    public ScheduleDate scheduleDate;
    public string enemyTeamName;

    public BattleResult(ScheduleDate scheduleDate, string enemyTeamName)
    {
        this.scheduleDate = scheduleDate;
        this.enemyTeamName = enemyTeamName;
    }
}