using QUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Panel_LobbyMenu : PanelAbstract
{
    private List<Button> buttonList = new List<Button>();

    public Button Button_Office;
    public Button Button_Squad;
    public Button Button_News;
    public Button Button_Market;
    public Button Button_RandomRecruit;
    public Button Button_Schedule;
    public Button Button_Traning;
    public Button Button_TeamHome;
    public Button Button_SavePanel;
    public Button Button_InternationalActivity;
    public Button Button_TeamUpgrade;
    public Button Button_Record;

    private Dictionary<string, OfficeUnitObject> dicOfficeUnit = new Dictionary<string, OfficeUnitObject>();
    void Start()
    {
        Button_Office = InitializeButton("Button_Office", OnClick_Office);
        Button_Squad = InitializeButton("Button_Squad", OnClick_Squad);
        Button_News = InitializeButton("Button_News", OnClick_News);
        Button_Market = InitializeButton("Button_Market", OnClick_Market);
        Button_Schedule = InitializeButton("Button_Schedule", OnClick_Schedule);
        Button_Traning = InitializeButton("Button_Traning", OnClick_Traning);
        Button_TeamHome = InitializeButton("Button_TeamHome", OnClick_TeamHome);
        Button_SavePanel = InitializeButton("Button_SavePanel", OnClick_SavePanel);
        Button_InternationalActivity = InitializeButton("Button_InternationalActivity", OnClick_InternationalActivity);
        Button_TeamUpgrade = InitializeButton("Button_TeamUpgrade", OnClick_TeamUpgrade);
        Button_Record = InitializeButton("Button_Record", OnClick_Record);
    }

    private Button InitializeButton(string buttonName, UnityAction onClickAction)
    {
        Button button = UIUtility.FindComponentInChildrenByName<Button>(gameObject, buttonName);
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClickAction);
            buttonList.Add(button);
        }
        return button;
    }

    private void OnClick_Squad()
    {
        var panel_Squad = PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Squad);
        panel_Squad.GetComponent<Panel_Squad>().PanelUpdate();
    }

    private void OnClick_Office()
    {
        // 로비 소환 유닛 업데이트
        var playerSquadUnitDatas = PlayerManager.Instance.GetPlayer_SquadUnitDatas();
        var playerUnitIDs = new HashSet<string>(playerSquadUnitDatas.Select(x => x.unitUniqueID)); // 현재 플레이어 유닛 ID 목록

        // 현재 사무실에 있는 유닛 중, 플레이어 스쿼드에 없는 유닛 제거
        var unitsToRemove = dicOfficeUnit.Where(pair => !playerUnitIDs.Contains(pair.Key)).ToList(); // 리스트로 변환해 안전하게 순회

        foreach (var pair in unitsToRemove)
        {
            pair.Value.ReturnToPool();
            dicOfficeUnit.Remove(pair.Key); // 딕셔너리에서 제거
        }

        // 플레이어 스쿼드에 있는 유닛 중, 사무실에 없는 유닛 소환
        foreach (var unitData in playerSquadUnitDatas)
        {
            if (!dicOfficeUnit.ContainsKey(unitData.unitUniqueID))
            {
                var office = OfficeUnitObject.Spawn(unitData);
                dicOfficeUnit.Add(unitData.unitUniqueID, office); // 새로 소환된 유닛 추가
            }
        }

        PanelRenderQueueManager.Instance.CloseAllPanel();
        FrontInfoCanvas.Instance.SetPanelName("사무실");
    }

    private void OnClick_News() { /* News 버튼 클릭 시 실행할 코드 */ }
    private void OnClick_Market()
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Market);
    }

    private void OnClick_Schedule() 
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Schedule);
    }

    private void OnClick_Traning() 
    { 
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Traning);
    }
    private void OnClick_TeamHome()
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_TeamHome);
    }

    private void OnClick_SavePanel() 
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_SaveData, PanelRenderQueueManager.ECanvasType.FrontCanvas);
    }

    private void OnClick_InternationalActivity()
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_InternationalActivity);
    }

    private void OnClick_TeamUpgrade()
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_TeamUpgrade);
    }

    private void OnClick_Record()
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Record);
    }
}
