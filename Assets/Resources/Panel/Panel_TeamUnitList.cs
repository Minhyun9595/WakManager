using QUtility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridItem_SelectTeamUnit : GridAbstract, GridInterface
{
    private string unitUniqueID;
    public TextMeshProUGUI JobText;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI TraitText;
    public TextMeshProUGUI PotentialPowerText;
    public TextMeshProUGUI ValueText;

    public Button TraningSelectButton;
    public Transform BlockImage;
    public TextMeshProUGUI BlockText;

    public new void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);

        JobText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "JobText");
        NameText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "NameText");
        TraitText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TraitText");
        PotentialPowerText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "PotentialPowerText");
        ValueText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "ValueText");
        BlockImage = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "BlockImage");
        BlockText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "BlockText");

        TraningSelectButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "TraningSelectButton");

        TraningSelectButton.onClick.RemoveAllListeners();
        TraningSelectButton.onClick.AddListener(OnClick_TraningSelectButton);
    }

    public void Set(UnitData _unitData)
    {
        unitUniqueID = _unitData.unitUniqueID;
        DT_Role dT_Role = DT_Role.GetInfoByIndex(_unitData.unitInfo_Immutable.RoleIndex);
        JobText.text = dT_Role.Name;
        NameText.text = _unitData.unitInfo_Immutable.Name;
        TraitText.text = $"특성 {_unitData.traitIndexList.Count}개";
        PotentialPowerText.text = "잠재력";
        ValueText.text = _unitData.GetUnitValue().ToString();

        TraningSelectButton.onClick.RemoveAllListeners();

        var leftDay = _unitData.GetScheduleLeftDay();
        BlockImage.gameObject.SetActive(0 < leftDay);
        if (0 < leftDay)
        {
            BlockText.text = $"{leftDay}일 뒤 훈련 종료";
        }
        else
        {
            TraningSelectButton.onClick.AddListener(OnClick_TraningSelectButton);
        }
    }

    public void OnClick_TraningSelectButton()
    {
        var panel_Traning = PanelRenderQueueManager.Instance.GetPanel<Panel_Traning>();
        panel_Traning.SelectUnit(unitUniqueID);

        var panel_TeamUnitList = PanelRenderQueueManager.Instance.GetPanel<Panel_TeamUnitList>();
        PanelRenderQueueManager.Instance.ClosePanel(panel_TeamUnitList);
    }
}


public class Panel_TeamUnitList : PanelAbstract
{
    public Transform Grid_SelectTeamUnit;
    public List<GridItem_SelectTeamUnit> gridItem_SelectTeamUnit;

    void Awake()
    {
        Grid_SelectTeamUnit = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Grid_SelectTeamUnit");
        gridItem_SelectTeamUnit = new List<GridItem_SelectTeamUnit>();
    }

    public override void Open()
    {
        base.Open();
        FrontInfoCanvas.Instance.SetPanelName("선수 선택");

        Update_TeamUnitList();
    }

    public override void Close()
    {
        base.Close();
    }

    private void Update_TeamUnitList()
    {
        var unitDatas = PlayerManager.Instance.GetPlayer_SquadUnitDatas();

        // 부족한 경우에만 생성
        for (int i = gridItem_SelectTeamUnit.Count; i < unitDatas.Count; i++)
        {
            var childItem = UIUtility.GetChildAutoCraete(Grid_SelectTeamUnit, i);
            childItem.SetActive(true);

            var gridItem = new GridItem_SelectTeamUnit();
            gridItem.Init(childItem);

            gridItem_SelectTeamUnit.Add(gridItem);
        }

        // 데이터를 세팅
        for (int i = 0; i < unitDatas.Count; i++)
        {
            gridItem_SelectTeamUnit[i].Set(unitDatas[i]);
            gridItem_SelectTeamUnit[i].gameObject.SetActive(true);
        }

        // 초과된 아이템 비활성화
        for (int i = unitDatas.Count; i < Grid_SelectTeamUnit.childCount; i++)
        {
            Grid_SelectTeamUnit.GetChild(i).gameObject.SetActive(false);
        }
    }
}
