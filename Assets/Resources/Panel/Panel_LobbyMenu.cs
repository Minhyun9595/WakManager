using QUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Panel_LobbyMenu : PanelAbstract
{
    private List<Button> buttonList = new List<Button>();

    public Button Button_Squad;
    public Button Button_News;
    public Button Button_Market;
    public Button Button_CardGacha;
    public Button Button_Schedule;
    public Button Button_Traning;
    public Button Button_TeamInfo;
    public Button Button_TeamRecord;
    public Button Button_SavePanel;

    void Start()
    {
        Button_Squad = InitializeButton("Button_Squad", OnClick_Squad);
        Button_News = InitializeButton("Button_News", OnClick_News);
        Button_Market = InitializeButton("Button_Market", OnClick_Market);
        Button_CardGacha = InitializeButton("Button_CardGacha", OnClick_CardGacha);
        Button_Schedule = InitializeButton("Button_Schedule", OnClick_Schedule);
        Button_Traning = InitializeButton("Button_Traning", OnClick_Traning);
        Button_TeamInfo = InitializeButton("Button_TeamInfo", OnClick_TeamInfo);
        Button_TeamRecord = InitializeButton("Button_TeamRecord", OnClick_TeamRecord);
        Button_SavePanel = InitializeButton("Button_SavePanel", OnClick_SavePanel);
    }

    private Button InitializeButton(string buttonName, UnityAction onClickAction)
    {
        Button button = UIUtility.FindComponentInChildrenByName<Button>(gameObject, buttonName);
        if (button != null)
        {
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

    private void OnClick_News() { /* News 버튼 클릭 시 실행할 코드 */ }
    private void OnClick_Market()
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Market);
    }

    private void OnClick_CardGacha() { /* CardGacha 버튼 클릭 시 실행할 코드 */ }
    private void OnClick_Schedule() { /* Schedule 버튼 클릭 시 실행할 코드 */ }
    private void OnClick_Traning() { /* Traning 버튼 클릭 시 실행할 코드 */ }
    private void OnClick_TeamInfo() { /* TeamInfo 버튼 클릭 시 실행할 코드 */ }
    private void OnClick_TeamRecord() { /* TeamRecord 버튼 클릭 시 실행할 코드 */ }

    private void OnClick_SavePanel() 
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_SaveData);
    }

}
