using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

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
    public int Money; // ��
    public int Population; // �α⵵
    public int MaintenanceCosts; // ������
    public int Reputation; // ����
    
    public List<UnitData> player_Squad_UnitCardDatas = new List<UnitData>();
    public List<UnitData> player_InSquad_UnitCardDatas = new List<UnitData>();
    public List<BattleReport> teamBattleReports = new List<BattleReport>();

    public List<KeyValuePair<EMoneyType, int>> WeekIncomeList = new List<KeyValuePair<EMoneyType, int>>();
    // AI ���� ������
    public EUnitTier teamTier;

    public void Initialize(EUnitTier _teamTier, string teamName)
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
        FrontInfoCanvas.Instance?.SetMoneyText(Money);
    }

    public bool ReduceMoney(int _money, bool doToast = true)
    {
        if (Money < _money)
        {
            if(doToast)
            {
                Panel_ToastMessage.OpenToast("���� �����մϴ�.", false);
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
        NotificationManager.Instance.ShowNotification($"�� �α⵵�� {Population} �����߽��ϴ�.");
    }

    public void DoActivity_SoloStream()
    {
        foreach(var unitData in player_Squad_UnitCardDatas)
        {
            unitData.SoloStream(this);
        }
        AddPopulation(30);
    }

    public void DoActivity_TeamStream()
    {
        var mulPopulation = 0.0f;
        var moneyRatio = 1.5f;
        foreach (var unitData in player_Squad_UnitCardDatas)
        {
            mulPopulation += unitData.Population;
            unitData.Population += 10;
        }

        mulPopulation *= moneyRatio;
        AddPopulation(100);
        AddMoney(EMoneyType.Activity_TeamStream, (int)mulPopulation);
    }
    
    public void DoActivity_SellGoods()
    {
        Panel_ToastMessage.OpenToast("���� ��", false);
        //AddMoney(EMoneyType.Activity_SellGoods, 100000);
    }

    public void DoActivity_Ads()
    {
        Panel_ToastMessage.OpenToast("���� ��", false);
    }

    public void DoActivity_FindSponsor()
    {
        Panel_ToastMessage.OpenToast("���� ��", false);
    }
}
