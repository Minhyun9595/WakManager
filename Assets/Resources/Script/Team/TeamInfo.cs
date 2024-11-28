using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum EMoneyType
{
    Start,
    Activity_SoloStream,
    Activity_TeamStream,
    Activity_SellGoods,
    Ads,
    Sponsor,
}

[System.Serializable]
public class TeamInfo
{
    public string TeamIndex;
    public string Name;
    public int Money; // 돈
    public int Population; // 인기도
    public int MaintenanceCosts; // 유지비
    public int Reputation; // 평판
    
    public List<UnitData> player_Squad_UnitCardDatas = new List<UnitData>();
    public List<UnitData> player_InSquad_UnitCardDatas = new List<UnitData>();
    public List<BattleReport> teamBattleReports = new List<BattleReport>();
    public List<BattleResult> winResults = new List<BattleResult>();
    public List<BattleResult> loseResults = new List<BattleResult>();

    public List<KeyValuePair<EMoneyType, int>> MonthIncomeList = new List<KeyValuePair<EMoneyType, int>>();
    // AI 팀용 데이터
    public ETeamTier teamTier;

    public void Initialize(ETeamTier _teamTier, string teamName)
    {
        TeamIndex = System.Guid.NewGuid().ToString();
        teamTier = _teamTier;
        Name = teamName;
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
                return false;
            }

            if (5 < player_InSquad_UnitCardDatas.Count)
            { 
                return false;
            }

            var squadCard = player_Squad_UnitCardDatas.Find(x => x.unitUniqueID == unitUniqueID);
            player_InSquad_UnitCardDatas.Add(squadCard);
        }
        else
        {
            player_InSquad_UnitCardDatas.RemoveAll(x => x.unitUniqueID == unitUniqueID);
        }

        return true;
    }

    public List<UnitData> GetPlayer_SquadUnitDatas()
    {
        return player_Squad_UnitCardDatas;
    }

    public UnitData GetUnitData_ByUniqueID(string uniqueID)
    {
        return player_Squad_UnitCardDatas.Find(x => x.unitUniqueID.Equals(uniqueID));
    }

    public List<UnitData> GetPlayer_InSquadUnitDatas()
    {
        return player_InSquad_UnitCardDatas;
    }

    public void AddMoney(EMoneyType eMoneyType, float _money)
    {
        Money += (int)_money;
        MonthIncomeList.Add(new KeyValuePair<EMoneyType, int>(eMoneyType, (int)_money));
        FrontInfoCanvas.Instance?.SetMoneyText(Money);
    }

    public bool ReduceMoney(int _money, bool doNeedMoneyToast = true)
    {
        if (Money < _money)
        {
            if(doNeedMoneyToast)
            {
                Panel_ToastMessage.OpenToast("돈이 부족합니다.", false);
            }

            return false;
        }

        Money -= _money;
        FrontInfoCanvas.Instance?.SetMoneyText(Money);

        return true;
    }

    public void AddPopulation(int _population)
    {
        Population += _population;
        NotificationManager.Instance.ShowNotification($"팀 인기도가 {Population} 증가했습니다.");
    }

    public void DoActivity_SoloStream()
    {
        if (false == PlayerManager.Instance.gameSchedule.AddScheduleToday(EScheduleType.Activity_SoloStream, "솔로 스트리밍"))
        {
            return;
        }

        foreach (var unitData in player_Squad_UnitCardDatas)
        {
            unitData.SoloStream(this);
        }
        AddPopulation(30);
        PlayerManager.Instance.gameSchedule.AdvanceDay();
    }

    public void DoActivity_TeamStream()
    {
        if (false == PlayerManager.Instance.gameSchedule.AddScheduleToday(EScheduleType.Activity_TeamStream, "팀 스트리밍"))
        {
            return;
        }

        var mulPopulation = 0.0f;
        var moneyRatio = 1.5f;
        foreach (var unitData in player_Squad_UnitCardDatas)
        {
            mulPopulation += unitData.Population;
            unitData.AddPopulation(10);
        }

        mulPopulation *= moneyRatio;
        AddPopulation(100);
        AddMoney(EMoneyType.Activity_TeamStream, (int)mulPopulation);
        PlayerManager.Instance.gameSchedule.AdvanceDay();
    }
    
    public void DoActivity_SellGoods()
    {
        Panel_ToastMessage.OpenToast("개발 중", false);
        //AddMoney(EMoneyType.Activity_SellGoods, 100000);
    }

    public void DoActivity_Ads()
    {
        Panel_ToastMessage.OpenToast("개발 중", false);
    }

    public void DoActivity_FindSponsor()
    {
        Panel_ToastMessage.OpenToast("개발 중", false);
    }

    public void AddBattleResult(bool isWin, TeamInfo opponentTeamInfo)
    {
        var currentDate = PlayerManager.Instance.gameSchedule.CurrentDate;
        var schedule = new ScheduleDate(EUnitScheduleType.None, currentDate.Year, currentDate.Month, currentDate.Day);
        if (isWin)
        {
            winResults.Add(new BattleResult(schedule, opponentTeamInfo.Name));
        }
        else
        {
            loseResults.Add(new BattleResult(schedule, opponentTeamInfo.Name));
        }
    }

    public int GetTax()
    {
        var totalIncome = MonthIncomeList.Sum(x => x.Value);
        var tax = totalIncome * 0.25f;

        return (int)tax;
    }

    public int GetUnitPay()
    {
        return player_Squad_UnitCardDatas.Sum(x => x.Pay);
    }
}
