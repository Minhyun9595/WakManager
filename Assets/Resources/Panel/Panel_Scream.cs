using QUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class GridItem_ScreamTeam : GridAbstract, GridInterface
{
    private TeamInfo teamInfo;
    public TextMeshProUGUI TeamNameText;
    public TextMeshProUGUI SquadInfoText;

    public Button ScreamButton;

    public new void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);

        TeamNameText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TeamNameText");
        SquadInfoText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "SquadInfoText");

        ScreamButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "ScreamButton");

        ScreamButton.onClick.AddListener(OnClick_Scream);
    }

    public void Set(TeamInfo _teamInfo)
    {
        teamInfo = _teamInfo;

        TeamNameText.text = teamInfo.Name;
        // "이름: teamInfo.player_InSquad_UnitCardDatas[0].unitStat.Name\n"
        SquadInfoText.text = string.Join(" | ", teamInfo.player_InSquad_UnitCardDatas.Select(unitCard => $"{unitCard.unitStat.Name}({unitCard.GetUnitValue()})"));

    }

    private void OnClick_Scream()
    {
        var result = PlayerManager.Instance.ScreamDataSet(teamInfo);

        if (result)
        {
            PlayerManager.Instance.SetSceneChangeType(SceneChangeType.MoveWorld);
            SceneManager.LoadScene((int)ESceneType.InGame);
        }
        else
        {
            Panel_ToastMessage.OpenToast("스쿼드에 등록된 선수가 부족하여 스크림을 실패했습니다.", false);
        }
    }
}

public class Panel_Scream : PanelAbstract
{
    public Transform Grid_ScreamTeam;
    public List<GridItem_ScreamTeam> gridItem_ScreamTeams;

    private void Awake()
    {
        Grid_ScreamTeam = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Grid_ScreamTeam");

        Init_ScreamTeam();
    }

    private void Init_ScreamTeam()
    {
        var teamInfos = PlayerManager.Instance.GetTeamInfos(EUnitTier.SurplustoRequirements);
        gridItem_ScreamTeams = new List<GridItem_ScreamTeam>();
        for (int i = 0; i < teamInfos.Count; i++)
        {
            var childItem = UIUtility.GetChildAutoCraete(Grid_ScreamTeam, i);
            childItem.SetActive(true);
            var gridItem_SquadCard = new GridItem_ScreamTeam();
            gridItem_SquadCard.Init(childItem);
            gridItem_SquadCard.Set(teamInfos[i]);

            gridItem_ScreamTeams.Add(gridItem_SquadCard);
        }
    }
}
