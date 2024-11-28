using QUtility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_EventPopup : PanelAbstract
{
    public Transform MonthPayBG;
    public TextMeshProUGUI MonthPayText;
    public Button PayButton;

    public static void EventOpen_MonthPay()
    {
        var obj = PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_EventPopup, PanelRenderQueueManager.ECanvasType.FrontCanvas);
        var eventPopup = obj.GetComponent<Panel_EventPopup>();
        eventPopup.Event_Pay();
    }

    void Awake()
    {
        MonthPayBG = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "MonthPayBG");
        MonthPayText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "MonthPayText");
        PayButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "PayButton");
        isCanClose = false;

        PayButton.onClick.AddListener(OnClick_PayButton);
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public void Event_Pay()
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
        MonthPayText.text = stringBuilder.ToString();
    }

    public void OnClick_PayButton()
    {
        var playerTeamInfo = PlayerManager.Instance.PlayerTeamInfo;
        var totalIncome = playerTeamInfo.MonthIncomeList.Sum(x => x.Value);
        var tax = playerTeamInfo.GetTax();

        var totalPay = playerTeamInfo.GetUnitPay();
        var facilityInfo = PlayerManager.Instance.PlayerTeamUpgrade.GetCurrentUpgrade(TeamUpgrade.UpgradeType.Facility);
        var officePay = facilityInfo.Value2;

        var totalOutput = totalPay + officePay + tax;

        // -�� �Ǿ �Ű澲�� �ʰ� ����.
        playerTeamInfo.Money -= totalOutput;
        FrontInfoCanvas.Instance?.SetMoneyText(playerTeamInfo.Money);
        Panel_ToastMessage.OpenToast($"{UIUtility.GetUnitizeText(totalOutput)}$ ����Ǿ����ϴ�.", true);

        Close();
    }
}
