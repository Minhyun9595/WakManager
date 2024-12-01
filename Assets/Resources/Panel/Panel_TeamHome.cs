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
        RightText.text = $"{_teamInfo.winResults.Count}승/{_teamInfo.loseResults.Count}패";
    }
}


public class Panel_TeamHome : PanelAbstract
{
    // 팀 정보
    public TextMeshProUGUI TeamNameText;
    public TextMeshProUGUI PopulationText;

    // 팀 가치
    public TextMeshProUGUI TeamValueText;

    // 스크림 전적
    public TextMeshProUGUI RecentScreamInfoText;

    // 다음 대회
    public Transform TournamentBG;
    public TextMeshProUGUI TournamentText;
    public Image MyTeamLogo;
    public Image EnemyTeamLogo;
    public TextMeshProUGUI MyTeamName;
    public TextMeshProUGUI EnemyTeamName;
    public TextMeshProUGUI ScreamInfoText;

    // 리그 정보
    public Transform LeagueInfoBG;
    public Transform Grid_LeagueTeamInfo;
    public List<GridItem_LeagueTeamInfo> gridItem_LeagueTeamInfo;

    // 수익 정보
    public TextMeshProUGUI IncomeText;

    void Awake()
    {

        // UI 요소 초기화
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
        FrontInfoCanvas.Instance.SetPanelName("홈");
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

        // 부족한 경우에만 생성
        for (int i = gridItem_LeagueTeamInfo.Count; i < teamInfos.Count; i++)
        {
            var childItem = UIUtility.GetChildAutoCraete(Grid_LeagueTeamInfo, i);
            childItem.SetActive(true);

            var gridItem = new GridItem_LeagueTeamInfo();
            gridItem.Init(childItem);

            gridItem_LeagueTeamInfo.Add(gridItem);
        }

        // 데이터를 세팅
        for (int rank = 0; rank < teamInfos.Count; rank++)
        {
            gridItem_LeagueTeamInfo[rank].Set(rank + 1, teamInfos[rank]);
            gridItem_LeagueTeamInfo[rank].gameObject.SetActive(true);
        }

        // 초과된 아이템 비활성화
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
        stringBuilder.AppendLine($"이달의 수익: {UIUtility.GetUnitizeText(totalIncome)}$");
        stringBuilder.AppendLine($"납부할 금액: {UIUtility.GetUnitizeText((int)totalOutput)}$");
        stringBuilder.AppendLine($"");
        stringBuilder.AppendLine($"상세내역");
        stringBuilder.AppendLine($"세금: {UIUtility.GetUnitizeText((int)tax)}$");
        stringBuilder.AppendLine($"급료: {UIUtility.GetUnitizeText(totalPay)}$");
        stringBuilder.AppendLine($"사무실 {officeName}: {UIUtility.GetUnitizeText(officePay)}$");
        IncomeText.text = stringBuilder.ToString();
    }

    private void Update_ScreamInfo()
    {
        var playerTeamInfo = PlayerManager.Instance.PlayerTeamInfo;
        RecentScreamInfoText.text = $"스크림 전적: {playerTeamInfo.winResults.Count}승/{playerTeamInfo.loseResults.Count}패";

        foreach(var battleReport in playerTeamInfo.teamBattleReports)
        {
            //battleReport.firstTeamBattleReport.unitReports
        }

    }
}
