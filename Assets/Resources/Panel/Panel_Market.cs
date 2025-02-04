using QUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridItem_Trait : GridAbstract
{
    public int traitIndex = -1;
    public Image TraitImage;
    public TextMeshProUGUI RankText;


    public override void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);
        TraitImage = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "TraitImage");
        RankText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "RankText");

        var button = _gameObject.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick_Trait);
    }

    public void SetTrait(DT_Trait dT_Trait)
    {
        traitIndex = dT_Trait.TraitIndex;
        TraitImage.sprite = UIUtility.GetSprite(dT_Trait.IconSprite);
        RankText.text = dT_Trait.GetRankString();
    }

    private void OnClick_Trait()
    {
        var Panel_TraitPopup = PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_TraitPopup.ToString());
        Panel_TraitPopup.GetComponent<Panel_TraitPopup>().Open(traitIndex);
    }
}

public class GridItem_MarketCard : GridAbstract
{
    public Panel_Market panel_Market;
    public string unitUniqueID;
    public Button BuyButton;
    public Animator UnitImage_Animator;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI PointText;
    public Transform Content_GridTrait;
    public Transform SellBG;

    public List<GridItem_Trait> gridItem_Traits = new List<GridItem_Trait>();

    public override void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);
        UnitImage_Animator = UIUtility.FindComponentInChildrenByName<Animator>(gameObject, "UnitImage");
        BuyButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "BuyButton");
        NameText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "NameText");
        PointText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "PointText");
        Content_GridTrait = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Content_GridTrait");
        SellBG = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "SellBG");
    }

    public void InitUnit(UnitData _unitData)
    {
        NameText.text = _unitData.unitInfo_Immutable.Name;
        PointText.text = _unitData.GetUnitValue().ToString();
        unitUniqueID = _unitData.unitUniqueID;
        //_unitData.unitInfo_Immutable.Animation

        var controller = _unitData.unitInfo_Immutable.GetRuntimeAnimatorController();
        UnitImage_Animator.runtimeAnimatorController = controller;
        UnitImage_Animator.keepAnimatorStateOnDisable = true;
        UnitImage_Animator.Play("Idle_Image");
        UpdateMarket(_unitData);

        SellBG.gameObject.SetActive(false);
    }

    private void UpdateMarket(UnitData _unitData)
    {
        gridItem_Traits = new List<GridItem_Trait>();
        for (int i = 0; i < _unitData.traitList.Count; i++)
        {
            var childItem = UIUtility.GetChildAutoCraete(Content_GridTrait, i);
            var gridItem_MarketTrait = new GridItem_Trait();
            var trait = _unitData.traitList[i];
            gridItem_MarketTrait.Init(childItem);
            gridItem_MarketTrait.SetTrait(trait);

            gridItem_Traits.Add(gridItem_MarketTrait);
            childItem.SetActive(true);
        }

        for (int i = _unitData.traitList.Count; i < Content_GridTrait.childCount; i++)
        {
            Content_GridTrait.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void PlayAnimation()
    {
        UnitImage_Animator.Play("Idle_Image");
    }
}

public class Panel_Market : PanelAbstract
{
    public Button Market_Button_0;
    public TextMeshProUGUI Market_Text_0;
    public Transform Market_Button_FG_0;
    public Button Market_Button_1;
    public TextMeshProUGUI Market_Text_1;
    public Transform Market_Button_FG_1;
    public Button Market_Button_2;
    public TextMeshProUGUI Market_Text_2;
    public Transform Market_Button_FG_2;

    public Transform BG;
    public Transform Grid_MarketCard;
    public List<GridItem_MarketCard> gridList;

    void Awake()
    {
        Market_Button_0 = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "Market_Button_0");
        Market_Text_0 = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "Market_Text_0");
        Market_Button_FG_0 = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Market_Button_FG_0");
        Market_Button_1 = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "Market_Button_1");
        Market_Text_1 = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "Market_Text_1");
        Market_Button_FG_1 = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Market_Button_FG_1");
        Market_Button_2 = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "Market_Button_2");
        Market_Text_2 = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "Market_Text_2");
        Market_Button_FG_2 = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Market_Button_FG_2");

        BG = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "BG");
        Grid_MarketCard = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Grid_MarketCard");

        var market_0 = DT_Market.GetInfoByIndex(0);
        var market_1 = DT_Market.GetInfoByIndex(1);
        var market_2 = DT_Market.GetInfoByIndex(2);

        Market_Text_0.text = $"{market_0.Name}\n{UIUtility.GetUnitizeText(market_0.SearchPrice)}$";
        Market_Text_1.text = $"{market_1.Name}\n{UIUtility.GetUnitizeText(market_1.SearchPrice)}$";
        Market_Text_2.text = $"{market_2.Name}\n{UIUtility.GetUnitizeText(market_2.SearchPrice)}$";

        Market_Button_0.onClick.AddListener(() => OnClick_Make(0));
        Market_Button_1.onClick.AddListener(() => OnClick_Make(1));
        Market_Button_2.onClick.AddListener(() => OnClick_Make(2));

        gridList = new List<GridItem_MarketCard>();
        var childItem = UIUtility.GetChildAutoCraete(Grid_MarketCard, 0);
        var gridItem_MarketCard = new GridItem_MarketCard();
        var index = 0;

        gridItem_MarketCard.Init(childItem);
        gridItem_MarketCard.BuyButton.onClick.AddListener(() => OnClick_Buy(index));
        gridList.Add(gridItem_MarketCard);
        gridItem_MarketCard.gameObject.SetActive(false);

        for (int i = 0; i < Grid_MarketCard.childCount; i++)
        {
            Grid_MarketCard.GetChild(i).gameObject.SetActive(false);
        }

        var marketDatas = PlayerManager.Instance.GetMarketDatas();
        if(marketDatas != null)
        {
            for (int i = 0; i < marketDatas.Count; i++)
            {
                SetCard(i, marketDatas[i]);
            }
        }
    }

    public override void Open()
    {
        base.Open();
        FrontInfoCanvas.Instance.SetPanelName("이적시장");

        var upgradeInfo = PlayerManager.Instance.PlayerTeamUpgrade.GetCurrentUpgrade(TeamUpgrade.UpgradeType.FindUnit);
        Market_Button_FG_0.gameObject.SetActive(false);
        Market_Button_FG_1.gameObject.SetActive(upgradeInfo.Level < 2);
        Market_Button_FG_2.gameObject.SetActive(upgradeInfo.Level < 4);
    }

    void OnClick_Make(int marketIndex)
    {
        // marketIndex가 1인 경우 레벨2가 넘어야하고 2인 경우 레벨 4가 넘어야함.
        //var upgradeInfo = PlayerManager.Instance.PlayerTeamUpgrade.GetCurrentUpgrade(TeamUpgrade.UpgradeType.FindUnit);
        //if (marketIndex == 1 && upgradeInfo.Level < 2)
        //{
        //    return;
        //}
        //else if (marketIndex == 2 && upgradeInfo.Level < 4)
        //{
        //    return;
        //}

        FindNewCard(marketIndex);
    }

    void FindNewCard(int marketIndex)
    {
        var dt_Market = DT_Market.GetInfoByIndex(marketIndex);
        var searchPrice = dt_Market.SearchPrice;

        var result = PlayerManager.Instance.PlayerTeamInfo.ReduceMoney(searchPrice);

        if (result == false)
        {
            return;
        }

        var upgradeInfo = PlayerManager.Instance.PlayerTeamUpgrade.GetCurrentUpgrade(TeamUpgrade.UpgradeType.FindUnit);
        var findCount = upgradeInfo.Value1;
        //var randomWorldCardList = PlayerManager.Instance.GetRandomWorldCard(upgradeInfo.Value1);
        List<UnitData> unitDatas = new List<UnitData>();
        for (int i = 0; i < findCount; i++)
        {
            // dt_Market에 따라서 랜덤한 티어로
            var randomTier = dt_Market.GetRandomCardTier();
            var randomUnitData = PlayerManager.Instance.GetWorldCard_ByTier(randomTier);
            unitDatas.Add(randomUnitData);
        }

        BG.gameObject.SetActive(true);
        for (int i = 0; i < unitDatas.Count; i++)
        {
            SetCard(i, unitDatas[i]);
        }

        for (int i = unitDatas.Count; i < Grid_MarketCard.childCount; i++)
        {
            gridList[i].gameObject.SetActive(false);
        }
    }

    public void SetCard(int _index, UnitData _unitData)
    {
        if(Grid_MarketCard.childCount <= _index)
        {
            var childItem = UIUtility.GetChildAutoCraete(Grid_MarketCard, _index);
            var gridItem_MarketCard = new GridItem_MarketCard();
            var index = _index;

            gridItem_MarketCard.Init(childItem);
            gridItem_MarketCard.BuyButton.onClick.AddListener(() => OnClick_Buy(index));
            gridList.Add(gridItem_MarketCard);
            gridItem_MarketCard.gameObject.SetActive(false);
        }

        gridList[_index].InitUnit(_unitData);
        gridList[_index].gameObject.SetActive(true);
        gridList[_index].PlayAnimation();
    }

    void OnClick_Buy(int _index)
    {
        var buyUnitData = PlayerManager.Instance.BuyUnit(gridList[_index].unitUniqueID);
        if(buyUnitData != null)
        {
            gridList[_index].SellBG.gameObject.SetActive(true);
            NotificationManager.Instance.ShowNotification($"선수 계약 [{buyUnitData.unitInfo_Immutable.Name}]");
        }
    }
}
