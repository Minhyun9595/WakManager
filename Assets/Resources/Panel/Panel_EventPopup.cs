using QUtility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ENoticeEventType
{
    OkayType,
    SelectType_YesNo,
}

public class Panel_EventPopup : PanelAbstract
{
    public Transform MonthPayBG;
    public TextMeshProUGUI MonthPayText;
    public Button PayButton;

    public Transform NoticeEventBG;
    public Button OkayButton;
    public Button YesButton;
    public Button NoButton;

    public static void EventOpen_MonthPay()
    {
        var obj = PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_EventPopup, PanelRenderQueueManager.ECanvasType.FrontCanvas);
        var eventPopup = obj.GetComponent<Panel_EventPopup>();
        eventPopup.Event_Pay();
    }

    public static void EventOpen_NoticeEvent(ENoticeEventType eNoticeEventType)
    {
        var obj = PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_EventPopup, PanelRenderQueueManager.ECanvasType.FrontCanvas);
        var eventPopup = obj.GetComponent<Panel_EventPopup>();
        eventPopup.Event_Notice(eNoticeEventType);
    }

    void Awake()
    {
        isCanClose = false;

        MonthPayBG = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "MonthPayBG");
        MonthPayText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "MonthPayText");
        PayButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "PayButton");
        PayButton.onClick.AddListener(OnClick_PayButton);

        NoticeEventBG = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "NoticeEventBG");
        OkayButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "OkayButton");
        YesButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "YesButton");
        NoButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "NoButton");
        OkayButton.onClick.AddListener(OnClick_NoticeOkay);
        YesButton.onClick.AddListener(OnClick_NoticeYes);
        NoButton.onClick.AddListener(OnClick_NoticeNo);

    }

    public override void Open()
    {
        base.Open();
        CloseBG();
    }

    public override void Close()
    {
        base.Close();
    }

    public void CloseBG()
    {
        MonthPayBG.gameObject.SetActive(false);
        NoticeEventBG.gameObject.SetActive(false);
    }

    public void Event_Pay()
    {
        MonthPayBG.gameObject.SetActive(true);
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
        MonthPayText.text = stringBuilder.ToString();
    }

    public void Event_Notice(ENoticeEventType eNoticeEventType)
    {
        NoticeEventBG.gameObject.SetActive(true);
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

        // -가 되어도 신경쓰지 않고 뺀다.
        playerTeamInfo.Money -= totalOutput;
        FrontInfoCanvas.Instance?.SetMoneyText(playerTeamInfo.Money);
        Panel_ToastMessage.OpenToast($"{UIUtility.GetUnitizeText(totalOutput)}$ 지출되었습니다.", true);

        Close();
    }

    public void OnClick_NoticeOkay()
    {

    }

    private void OnClick_NoticeYes()
    {

    }

    private void OnClick_NoticeNo()
    {

    }
}
