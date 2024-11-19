using QUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridItem_Trait : GridAbstract, GridInterface
{
    public int traitIndex = -1;
    public Image TraitImage;
    public TextMeshProUGUI RankText;


    public new void Init(GameObject _gameObject)
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
        RankText.text = dT_Trait.Rank.ToString();
    }

    private void OnClick_Trait()
    {
        var Panel_TraitPopup = PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_TraitPopup.ToString());
        Panel_TraitPopup.GetComponent<Panel_TraitPopup>().Open(traitIndex);
    }
}

public class GridItem_MarketCard : GridAbstract, GridInterface
{
    public Panel_Market panel_Market;
    public string unitUniqueID;
    public Button BuyButton;
    public Animator UnitImage;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI PointText;
    public Transform Content_GridTrait;
    public Transform SellBG;

    public List<GridItem_Trait> gridItem_Traits = new List<GridItem_Trait>();

    public new void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);
        UnitImage = UIUtility.FindComponentInChildrenByName<Animator>(gameObject, "UnitImage");
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

        var controller = Resources.Load<RuntimeAnimatorController>($"Animation/UnitAnimation/{_unitData.unitInfo_Immutable.Animator}/{_unitData.unitInfo_Immutable.Animator}");
        UnitImage.runtimeAnimatorController = controller;

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
        UnitImage.Play("Idle_Image");
    }
}

public class Panel_Market : PanelAbstract
{
    public Button MakeButton;
    public Transform BG;
    public Transform Grid_MarketCard;
    public List<GridItem_MarketCard> gridList;

    void Start()
    {
        MakeButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "MakeButton");
        BG = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "BG");
        Grid_MarketCard = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Grid_MarketCard");

        MakeButton.onClick.AddListener(OnClick_Make);

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
    }

    void OnClick_Make()
    {
        FindNewCard();
    }

    void FindNewCard()
    {
        var result = PlayerManager.Instance.PlayerTeamInfo.ReduceMoney(DT_Const.GetInfoByIndex("TRANSFER_MARKET_SEARCH_COST"));

        if (result == false)
        {
            return;
        }

        var dt_TeamUpgrade = DT_TeamUpgrade.GetInfoByIndex("인재 발굴", PlayerManager.Instance.PlayerTeamUpgrade.FindUnitLevel);
        PlayerManager.Instance.PlayerTeamUpgrade.FindUnitLevel += 1;
        var randomWorldCardList = PlayerManager.Instance.GetRandomWorldCard(dt_TeamUpgrade.Value1);

        BG.gameObject.SetActive(true);
        for (int i = 0; i < randomWorldCardList.Count; i++)
        {
            SetCard(i, randomWorldCardList[i]);
        }

        for (int i = randomWorldCardList.Count; i < Grid_MarketCard.childCount; i++)
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
        var result = PlayerManager.Instance.BuyUnit(gridList[_index].unitUniqueID);
        if(result)
        {
            gridList[_index].SellBG.gameObject.SetActive(true);
        }
    }
}
