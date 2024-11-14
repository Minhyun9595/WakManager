using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TeamInfo
{
    public int Money; // 돈
    public int Population; // 인기도
    public int MaintenanceCosts; // 유지비
    public int Reputation; // 평판

    public List<UnitData> player_Squad_UnitCardDatas = new List<UnitData>();
    public List<UnitData> player_InSquad_UnitCardDatas = new List<UnitData>();
    public List<BattleReport> teamBattleReports = new List<BattleReport>();

    public void Initialize()
    {
        Money = 0;
        Population = 0;
        MaintenanceCosts = 0;
        Reputation = 0;
        player_Squad_UnitCardDatas = new List<UnitData>();
        player_InSquad_UnitCardDatas = new List<UnitData>();
        teamBattleReports = new List<BattleReport>();
    }

    public void Clear()
    {
        Money = 0;
        Population = 0;
        MaintenanceCosts = 0;
        Reputation = 0;
        player_Squad_UnitCardDatas.Clear();
        player_InSquad_UnitCardDatas.Clear();
        teamBattleReports.Clear();
    }

    public void Load_UnitInit()
    {
        Load_UnitInit(player_Squad_UnitCardDatas);
        Load_UnitInit(player_InSquad_UnitCardDatas);
    }
    private void Load_UnitInit(List<UnitData> unitDatas)
    {
        foreach (UnitData unitData in unitDatas)
        {
            unitData.LoadUnit();
        }
    }

    public void AddSquadUnit(UnitData unitData)
    {
        player_Squad_UnitCardDatas.Add(unitData);
    }

    public void AddInSquadUnit(UnitData unitData)
    {
        player_InSquad_UnitCardDatas.Add(unitData);
    }

    public bool SquadActionUnitCard(string unitUniqueID, bool isAdd)
    {
        if (isAdd)
        {
            var inSquadCard = player_InSquad_UnitCardDatas.Find(x => x.unitUniqueID == unitUniqueID);

            if (inSquadCard != null)
            {
                return true;
            }

            var squadCard = player_Squad_UnitCardDatas.Find(x => x.unitUniqueID == unitUniqueID);
            player_InSquad_UnitCardDatas.Add(squadCard);
        }
        else
        {
            player_InSquad_UnitCardDatas.RemoveAll(x => x.unitUniqueID == unitUniqueID);
            return true;
        }

        return false;
    }

    public List<UnitData> GetPlayer_SquadUnitDatas()
    {
        return player_Squad_UnitCardDatas;
    }

    public List<UnitData> GetPlayer_InSquadUnitDatas()
    {
        return player_InSquad_UnitCardDatas;
    }
}
