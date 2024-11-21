using QUtility;
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
    public Button Button_TeamInfo;
    public Button Button_TeamRecord;
    public Button Button_SavePanel;
    public Button Button_InternationalActivity;
    public Button Button_TeamUpgrade;

    private Dictionary<string, OfficeUnitObject> dicOfficeUnit = new Dictionary<string, OfficeUnitObject>();
    void Start()
    {
        Button_Office = InitializeButton("Button_Office", OnClick_Office);
        Button_Squad = InitializeButton("Button_Squad", OnClick_Squad);
        Button_News = InitializeButton("Button_News", OnClick_News);
        Button_Market = InitializeButton("Button_Market", OnClick_Market);
        Button_RandomRecruit = InitializeButton("Button_RandomRecruit", OnClick_RandomRecruit);
        Button_Schedule = InitializeButton("Button_Schedule", OnClick_Schedule);
        Button_Traning = InitializeButton("Button_Traning", OnClick_Traning);
        Button_TeamInfo = InitializeButton("Button_TeamInfo", OnClick_TeamInfo);
        Button_TeamRecord = InitializeButton("Button_TeamRecord", OnClick_TeamRecord);
        Button_SavePanel = InitializeButton("Button_SavePanel", OnClick_SavePanel);
        Button_InternationalActivity = InitializeButton("Button_InternationalActivity", OnClick_InternationalActivity);
        Button_TeamUpgrade = InitializeButton("Button_TeamUpgrade", OnClick_TeamUpgrade);
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
        FrontInfoCanvas.Instance.SetPanelName("�繫��");
        var panel_Squad = PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Squad);
        panel_Squad.GetComponent<Panel_Squad>().PanelUpdate();
    }

    private void OnClick_Office()
    {
        // �κ� ��ȯ ���� ������Ʈ
        var playerSquadUnitDatas = PlayerManager.Instance.GetPlayer_SquadUnitDatas();
        var playerUnitIDs = new HashSet<string>(playerSquadUnitDatas.Select(x => x.unitUniqueID)); // ���� �÷��̾� ���� ID ���

        // ���� �繫�ǿ� �ִ� ���� ��, �÷��̾� �����忡 ���� ���� ����
        var unitsToRemove = dicOfficeUnit.Where(pair => !playerUnitIDs.Contains(pair.Key)).ToList(); // ����Ʈ�� ��ȯ�� �����ϰ� ��ȸ

        foreach (var pair in unitsToRemove)
        {
            pair.Value.ReturnToPool();
            dicOfficeUnit.Remove(pair.Key); // ��ųʸ����� ����
        }

        // �÷��̾� �����忡 �ִ� ���� ��, �繫�ǿ� ���� ���� ��ȯ
        foreach (var unitData in playerSquadUnitDatas)
        {
            if (!dicOfficeUnit.ContainsKey(unitData.unitUniqueID))
            {
                var office = OfficeUnitObject.Spawn(unitData);
                dicOfficeUnit.Add(unitData.unitUniqueID, office); // ���� ��ȯ�� ���� �߰�
            }
        }

        PanelRenderQueueManager.Instance.CloseAllPanel();
    }

    private void OnClick_News() { /* News ��ư Ŭ�� �� ������ �ڵ� */ }
    private void OnClick_Market()
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Market);
    }

    private void OnClick_RandomRecruit() { /* CardGacha ��ư Ŭ�� �� ������ �ڵ� */ }

    private void OnClick_Schedule() 
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Schedule);
    }

    private void OnClick_Traning() 
    { 
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Traning);
    }
    private void OnClick_TeamInfo() { /* TeamInfo ��ư Ŭ�� �� ������ �ڵ� */ }
    private void OnClick_TeamRecord() { /* TeamRecord ��ư Ŭ�� �� ������ �ڵ� */ }

    private void OnClick_SavePanel() 
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_SaveData);
    }

    private void OnClick_InternationalActivity()
    {

    }

    private void OnClick_TeamUpgrade()
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_TeamUpgrade);
    }
}
