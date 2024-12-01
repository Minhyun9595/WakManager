using QUtility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridItem_LeagueTeamInfo : GridAbstract, GridInterface
{
    public TextMeshProUGUI LeftText;
    public TextMeshProUGUI RightText;

    public new void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);

        LeftText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "LeftText");
        RightText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "RightText");
    }

    public void Set(int _rank, TeamInfo _teamInfo)
    {
        LeftText.text = $"#{_rank} {_teamInfo.Name}";
        RightText.text = $"{_teamInfo.winResults.Count}��/{_teamInfo.loseResults.Count}��";
    }
}


public class Panel_TeamHome : PanelAbstract
{
    // �� ����
    public TextMeshProUGUI TeamNameText;
    public TextMeshProUGUI PopulationText;

    // �� ��ġ
    public TextMeshProUGUI TeamValueText;

    // ��ũ�� ����
    public TextMeshProUGUI RecentScreamInfoText;

    // ���� ��ȸ
    public Transform TournamentBG;
    public TextMeshProUGUI TournamentText;
    public Image MyTeamLogo;
    public Image EnemyTeamLogo;
    public TextMeshProUGUI MyTeamName;
    public TextMeshProUGUI EnemyTeamName;
    public TextMeshProUGUI ScreamInfoText;

    // ���� ����
    public Transform LeagueInfoBG;
    public Transform Grid_LeagueTeamInfo;
    public List<GridItem_LeagueTeamInfo> gridItem_LeagueTeamInfo;

    // ���� ����
    public TextMeshProUGUI IncomeText;

    void Awake()
    {

        // UI ��� �ʱ�ȭ
        TeamNameText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TeamNameText");
        PopulationText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "PopulationText");
        TeamValueText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TeamValueText");
        RecentScreamInfoText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "RecentScreamInfoText");

        TournamentBG = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "TournamentBG");
        TournamentText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TournamentText");
        MyTeamLogo = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "MyTeamLogo");
        EnemyTeamLogo = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "EnemyTeamLogo");
        MyTeamName = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "MyTeamName");
        EnemyTeamName = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "EnemyTeamName");

        ScreamInfoText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "ScreamInfoText");

        Grid_LeagueTeamInfo = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Grid_LeagueTeamInfo");
        gridItem_LeagueTeamInfo = new List<GridItem_LeagueTeamInfo>();
        LeagueInfoBG = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "LeagueInfoBG");

        IncomeText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "IncomeText");

    }

    public override void Open()
    {
        base.Open();
        FrontInfoCanvas.Instance.SetPanelName("Ȩ");
        Update_LeagueTeamInfo();
        Update_Income();
        Update_ScreamInfo();
    }

    public override void Close()
    {
        base.Close();
    }

    private void Update_LeagueTeamInfo()
    {
        var teamInfos = PlayerManager.Instance.GetTeamInfos(PlayerManager.Instance.PlayerTeamInfo.teamTier);

        // ������ ��쿡�� ����
        for (int i = gridItem_LeagueTeamInfo.Count; i < teamInfos.Count; i++)
        {
            var childItem = UIUtility.GetChildAutoCraete(Grid_LeagueTeamInfo, i);
            childItem.SetActive(true);

            var gridItem = new GridItem_LeagueTeamInfo();
            gridItem.Init(childItem);

            gridItem_LeagueTeamInfo.Add(gridItem);
        }

        // �����͸� ����
        for (int rank = 0; rank < teamInfos.Count; rank++)
        {
            gridItem_LeagueTeamInfo[rank].Set(rank + 1, teamInfos[rank]);
            gridItem_LeagueTeamInfo[rank].gameObject.SetActive(true);
        }

        // �ʰ��� ������ ��Ȱ��ȭ
        for (int i = teamInfos.Count; i < Grid_LeagueTeamInfo.childCount; i++)
        {
            Grid_LeagueTeamInfo.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Update_Income()
    {
        var playerTeamInfo = PlayerManager.Instance.PlayerTeamInfo;
        var totalIncome = playerTeamInfo.MonthIncomeList.Sum(x => x.Value);
        var tax = playerTeamInfo.GetTax();

        var totalPay = playerTeamInfo.GetUnitPay();

        var facilityInfo = PlayerManager.Instance.PlayerTeamUpgrade.GetCurrentUpgrade(TeamUpgrade.UpgradeType.Facility);
        var officePay = facilityInfo.Value2;
        var officeName = facilityInfo.StrValue1;

        var totalOutput = totalPay + officePay + tax;

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"�̴��� ����: {UIUtility.GetUnitizeText(totalIncome)}$");
        stringBuilder.AppendLine($"������ �ݾ�: {UIUtility.GetUnitizeText((int)totalOutput)}$");
        stringBuilder.AppendLine($"");
        stringBuilder.AppendLine($"�󼼳���");
        stringBuilder.AppendLine($"����: {UIUtility.GetUnitizeText((int)tax)}$");
        stringBuilder.AppendLine($"�޷�: {UIUtility.GetUnitizeText(totalPay)}$");
        stringBuilder.AppendLine($"�繫�� {officeName}: {UIUtility.GetUnitizeText(officePay)}$");
        IncomeText.text = stringBuilder.ToString();
    }

    private void Update_ScreamInfo()
    {
        var playerTeamInfo = PlayerManager.Instance.PlayerTeamInfo;
        RecentScreamInfoText.text = $"��ũ�� ����: {playerTeamInfo.winResults.Count}��/{playerTeamInfo.loseResults.Count}��";

        foreach(var battleReport in playerTeamInfo.teamBattleReports)
        {
            //battleReport.firstTeamBattleReport.unitReports
        }

    }
}
