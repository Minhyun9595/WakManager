using QUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Traning : PanelAbstract
{
    public Button SelectUnitButton;
    public Button TraningButton_Health;
    public Button TraningButton_Attack;
    public Button TraningButton_Defense;
    public Button TraningButton_Trait;
    public Button TraningButton_Mental;
    public Button TraningButton_Scream;

    public string traningUniqueUnitID;
    void Awake()
    {
        SelectUnitButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "SelectUnitButton");
        TraningButton_Health = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "TraningButton_Health");
        TraningButton_Attack = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "TraningButton_Attack");
        TraningButton_Defense = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "TraningButton_Defense");
        TraningButton_Trait = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "TraningButton_Trait");
        TraningButton_Mental = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "TraningButton_Mental");
        TraningButton_Scream = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "TraningButton_Scream");

        SelectUnitButton.onClick.AddListener(OnClick_SelectUnit);
        TraningButton_Health.onClick.AddListener(OnClick_Traning_Stat_Health);
        TraningButton_Attack.onClick.AddListener(OnClick_Traning_Stat_Damage);
        TraningButton_Defense.onClick.AddListener(OnClick_Traning_Stat_Defense);
        TraningButton_Trait.onClick.AddListener(OnClick_Traning_Trait);
        TraningButton_Mental.onClick.AddListener(OnClick_Traning_Mental);
        TraningButton_Scream.onClick.AddListener(OnClick_Traning_Scream);
    }
    
    public override void Open()
    {
        base.Open();
        FrontInfoCanvas.Instance.SetPanelName("훈련");

        SetPanelMode(true);
    }

    void SetPanelMode(bool isOpenMode)
    {
        TraningButton_Health.gameObject.SetActive(!isOpenMode);
        TraningButton_Trait.gameObject.SetActive(!isOpenMode);
        TraningButton_Mental.gameObject.SetActive(!isOpenMode);
        SelectUnitButton.gameObject.SetActive(isOpenMode);
        TraningButton_Scream.gameObject.SetActive(isOpenMode);
    }

    void OnClick_Traning_Stat_Health()
    {
        var unitData = PlayerManager.Instance.PlayerTeamInfo.GetUnitData_ByUniqueID(traningUniqueUnitID);
        if (unitData == null)
        {
            Close();
            return;
        }
        unitData.AddSchedule(1, EUnitScheduleType.Traning_Health);
        Panel_ToastMessage.OpenToast($"{unitData.unitInfo_Immutable.Name}이 체력 훈련에 들어갔습니다.", true);
        SetPanelMode(true);
    }

    void OnClick_Traning_Stat_Damage()
    {
        var unitData = PlayerManager.Instance.PlayerTeamInfo.GetUnitData_ByUniqueID(traningUniqueUnitID);
        unitData.AddSchedule(1, EUnitScheduleType.Traning_Damage);
        Panel_ToastMessage.OpenToast($"{unitData.unitInfo_Immutable.Name}이 공격력 훈련에 들어갔습니다.", true);
        SetPanelMode(true);
    }

    void OnClick_Traning_Stat_Defense()
    {
        var unitData = PlayerManager.Instance.PlayerTeamInfo.GetUnitData_ByUniqueID(traningUniqueUnitID);
        if (unitData == null)
        {
            Close();
            return;
        }
        unitData.AddSchedule(1, EUnitScheduleType.Traning_Armor);
        Panel_ToastMessage.OpenToast($"{unitData.unitInfo_Immutable.Name}이 방어 훈련에 들어갔습니다.", true);
        SetPanelMode(true);
    }

    void OnClick_Traning_Trait()
    {
        var unitData = PlayerManager.Instance.PlayerTeamInfo.GetUnitData_ByUniqueID(traningUniqueUnitID);
        if (unitData == null)
        {
            Close();
            return;
        }    
        unitData.AddSchedule(1, EUnitScheduleType.Traning_Trait);
        Panel_ToastMessage.OpenToast($"{unitData.unitInfo_Immutable.Name}이 특성 훈련에 들어갔습니다.", true);
        SetPanelMode(true);
    }

    void OnClick_Traning_Mental()
    {
        var unitData = PlayerManager.Instance.PlayerTeamInfo.GetUnitData_ByUniqueID(traningUniqueUnitID);
        if (unitData == null)
        {
            Close();
            return;
        }
        unitData.AddSchedule(1, EUnitScheduleType.Traning_Mental);
        Panel_ToastMessage.OpenToast($"{unitData.unitInfo_Immutable.Name}이 멘탈 훈련에 들어갔습니다.", true);
        SetPanelMode(true);
    }

    void OnClick_SelectUnit()
    {
        var squadUnitDatas = PlayerManager.Instance.PlayerTeamInfo.GetPlayer_SquadUnitDatas();

        if(squadUnitDatas.Count == 0)
        {
            Panel_ToastMessage.OpenToast("선수가 없습니다.", false);
            return;
        }

        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_TeamUnitList);
    }

    void OnClick_Traning_Scream()
    {
        var inSquadUnitDatas = PlayerManager.Instance.PlayerTeamInfo.GetPlayer_InSquadUnitDatas();
        if (inSquadUnitDatas.Count == 0)
        {
            Panel_ToastMessage.OpenToast("스쿼드에 들어가있는 선수가 없습니다.", false);
            return;
        }

        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Scream);
    }

    public void SelectUnit(string unitUniqueID)
    {
        Panel_ToastMessage.OpenToast(unitUniqueID, false);
        var unitData = PlayerManager.Instance.PlayerTeamInfo.GetUnitData_ByUniqueID(unitUniqueID);

        if(unitData == null)
        {
            return;
        }

        traningUniqueUnitID = unitData.unitUniqueID;

        SetPanelMode(false);
    }

}