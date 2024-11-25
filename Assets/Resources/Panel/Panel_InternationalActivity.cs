using QUtility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class Panel_InternationalActivity : PanelAbstract
{
    public Button SoloStream_Button;
    public Button TeamStream_Button;
    public Button Goods_Button;
    public Button FindAds_Button;
    public Button FindSponsor_Button;

    public TextMeshProUGUI SoloStream_Text;
    public TextMeshProUGUI TeamStream_Text;
    public TextMeshProUGUI Goods_Text;
    public TextMeshProUGUI FindAds_Text;
    public TextMeshProUGUI FindSponsor_Text;

    void Awake()
    {
        SoloStream_Button = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "SoloStream_Button");
        TeamStream_Button = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "TeamStream_Button");
        Goods_Button = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "Goods_Button");
        FindAds_Button = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "FindAds_Button");
        FindSponsor_Button = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "FindSponsor_Button");

        SoloStream_Text = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "SoloStream_Text");
        TeamStream_Text = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TeamStream_Text");
        Goods_Text = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "Goods_Text");
        FindAds_Text = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "FindAds_Text");
        FindSponsor_Text = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "FindSponsor_Text");

        SoloStream_Button.onClick.AddListener(OnClick_SoloStream);
        TeamStream_Button.onClick.AddListener(OnClick_TeamStream);
        Goods_Button.onClick.AddListener(OnClick_Goods);
        FindAds_Button.onClick.AddListener(OnClick_FindAds);
        FindSponsor_Button.onClick.AddListener(OnClick_FindSponsor);

    }

    public override void Open()
    {
        base.Open();
        FrontInfoCanvas.Instance.SetPanelName("외부 활동");
    }

    void OnClick_SoloStream()
    {
        if(PlayerManager.Instance.PlayerTeamInfo.player_Squad_UnitCardDatas.Count < 1)
        {
            Panel_ToastMessage.OpenToast("선수가 없습니다.", false);
            return;
        }

        PlayerManager.Instance.PlayerTeamInfo.DoActivity_SoloStream();
        PlayerManager.Instance.gameSchedule.AdvanceDay();
    }

    void OnClick_TeamStream()
    {
        if (PlayerManager.Instance.PlayerTeamInfo.player_Squad_UnitCardDatas.Count < 1)
        {
            Panel_ToastMessage.OpenToast("선수가 없습니다.", false);
            return;
        }

        PlayerManager.Instance.PlayerTeamInfo.DoActivity_TeamStream();
        PlayerManager.Instance.gameSchedule.AdvanceDay();
    }

    void OnClick_Goods()
    {
        PlayerManager.Instance.PlayerTeamInfo.DoActivity_SellGoods();
        PlayerManager.Instance.gameSchedule.AdvanceDay();
    }

    void OnClick_FindAds()
    {
        PlayerManager.Instance.PlayerTeamInfo.DoActivity_Ads();
        PlayerManager.Instance.gameSchedule.AdvanceDay();
    }

    void OnClick_FindSponsor()
    {
        PlayerManager.Instance.PlayerTeamInfo.DoActivity_FindSponsor();
        PlayerManager.Instance.gameSchedule.AdvanceDay();
    }
}
