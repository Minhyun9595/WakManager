using QUtility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_TeamUpgrade : PanelAbstract
{
    public Button FindUnit_Button;
    public Button UnitAnalysis_Button;
    public Button Facility_Button;
    public Button RecruitmentOfCoachingStaff_Button;

    public TextMeshProUGUI FindUnit_Title;
    public TextMeshProUGUI UnitAnalysis_Title;
    public TextMeshProUGUI Facility_Title;
    public TextMeshProUGUI RecruitmentOfCoachingStaff_Title;

    public TextMeshProUGUI FindUnit_Description;
    public TextMeshProUGUI UnitAnalysis_Description;
    public TextMeshProUGUI Facility_Description;
    public TextMeshProUGUI RecruitmentOfCoachingStaff_Description;

    void Awake()
    {
        FindUnit_Button = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "FindUnit_Button");
        UnitAnalysis_Button = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "UnitAnalysis_Button");
        Facility_Button = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "Facility_Button");
        RecruitmentOfCoachingStaff_Button = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "RecruitmentOfCoachingStaff_Button");

        FindUnit_Title = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "FindUnit_Title");
        UnitAnalysis_Title = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "UnitAnalysis_Title");
        Facility_Title = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "Facility_Title");
        RecruitmentOfCoachingStaff_Title = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "RecruitmentOfCoachingStaff_Title");

        FindUnit_Description = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "FindUnit_Description");
        UnitAnalysis_Description = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "UnitAnalysis_Description");
        Facility_Description = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "Facility_Description");
        RecruitmentOfCoachingStaff_Description = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "RecruitmentOfCoachingStaff_Description");

        FindUnit_Button.onClick.AddListener(() => OnClick_Upgrade(TeamUpgrade.UpgradeType.FindUnit));
        UnitAnalysis_Button.onClick.AddListener(() => OnClick_Upgrade(TeamUpgrade.UpgradeType.UnitAnalysis));
        Facility_Button.onClick.AddListener(() => OnClick_Upgrade(TeamUpgrade.UpgradeType.Facility));
        RecruitmentOfCoachingStaff_Button.onClick.AddListener(() => OnClick_Upgrade(TeamUpgrade.UpgradeType.RecruitmentOfCoachingStaff));
    }

    public override void Open()
    {
        base.Open();
        FrontInfoCanvas.Instance.SetPanelName("팀 업그레이드");

        PanelUpdate();
    }

    public void PanelUpdate()
    {
        UpdateUpgradeUI(TeamUpgrade.UpgradeType.FindUnit, FindUnit_Title, FindUnit_Description);
        UpdateUpgradeUI(TeamUpgrade.UpgradeType.UnitAnalysis, UnitAnalysis_Title, UnitAnalysis_Description);
        UpdateUpgradeUI(TeamUpgrade.UpgradeType.Facility, Facility_Title, Facility_Description);
        UpdateUpgradeUI(TeamUpgrade.UpgradeType.RecruitmentOfCoachingStaff, RecruitmentOfCoachingStaff_Title, RecruitmentOfCoachingStaff_Description);
    }

    private void UpdateUpgradeUI(TeamUpgrade.UpgradeType type, TextMeshProUGUI title, TextMeshProUGUI description)
    {
        var upgrade = PlayerManager.Instance.PlayerTeamUpgrade.GetCurrentUpgrade(type);
        title.text = upgrade.Name;
        description.text = string.Format(upgrade.Desc, upgrade.Value1) + $"\n비용: {UIUtility.GetUnitizeText(upgrade.Cost)}$";
    }


    private void OnClick_Upgrade(TeamUpgrade.UpgradeType type)
    {
        var before = PlayerManager.Instance.PlayerTeamUpgrade.GetCurrentUpgrade(type);
        var result = PlayerManager.Instance.PlayerTeamUpgrade.TryLevelUp(type);

        if (result)
        {
            var after = PlayerManager.Instance.PlayerTeamUpgrade.GetCurrentUpgrade(type);
            var message = $"[{after.Name}] 업그레이드 성공: {string.Format(after.Desc, after.Value1)}";
            Panel_ToastMessage.OpenToast(message, true);
            NotificationManager.Instance.ShowNotification(message);
            PanelUpdate();
        }
        else
        {
            Panel_ToastMessage.OpenToast($"[{before.Name}] 업그레이드 실패: 재화가 부족하거나 최대 레벨입니다.", false);
        }
    }
}
