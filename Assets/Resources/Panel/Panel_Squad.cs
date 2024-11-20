using QUtility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridItem_InSquadCard : GridAbstract, GridInterface
{
    private string unitUniqueID;
    public Image UnitImage;
    public TextMeshProUGUI PointText;
    public TextMeshProUGUI NameText;

    public Button OutSquadButton;
    public Transform Content_GridTrait;
    public List<GridItem_Trait> gridItem_Traits = new List<GridItem_Trait>();

    public new void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);

        UnitImage = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "UnitImage");
        PointText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "PointText");
        NameText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "NameText");
        Content_GridTrait = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Content_GridTrait");

        OutSquadButton = _gameObject.GetComponent<Button>();

        OutSquadButton.onClick.RemoveAllListeners();
        OutSquadButton.onClick.AddListener(OnClick_OutSquad);
    }

    public void Set(UnitData _unitData)
    {
        unitUniqueID = _unitData.unitUniqueID;
        NameText.text = _unitData.unitInfo_Immutable.Name;
        PointText.text = _unitData.GetUnitValue().ToString();
        UpdateMarket(_unitData);
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

    private void OnClick_OutSquad()
    {
        var result = PlayerManager.Instance.SquadActionUnitCard(unitUniqueID, false);

        if(result)
        {
            var panel_Squad = PanelRenderQueueManager.Instance.GetPanel<Panel_Squad>();
            panel_Squad.PanelUpdate();
        }
    }
}


public class GridItem_SquadCard : GridAbstract, GridInterface
{
    private string unitUniqueID;
    public TextMeshProUGUI JobText;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI TraitText;
    public TextMeshProUGUI PotentialPowerText;
    public TextMeshProUGUI ValueText;

    public Button InSquadButton;
    public Button SellButton;
    public Button InfoButton;

    public new void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);

        JobText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "JobText");
        NameText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "NameText");
        TraitText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TraitText");
        PotentialPowerText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "PotentialPowerText");
        ValueText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "ValueText");

        InSquadButton = _gameObject.GetComponent<Button>();
        SellButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "SellButton");
        InfoButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "InfoButton");

        InSquadButton.onClick.RemoveAllListeners();
        SellButton.onClick.RemoveAllListeners();
        InfoButton.onClick.RemoveAllListeners();

        InSquadButton.onClick.AddListener(OnClick_SquadCard);
        SellButton.onClick.AddListener(OnClick_SellButton);
        InfoButton.onClick.AddListener(OnClick_InfoButton);
    }

    public void Set(UnitData _unitData)
    {
        unitUniqueID = _unitData.unitUniqueID;
        DT_Role dT_Role = DT_Role.GetInfoByIndex(_unitData.unitInfo_Immutable.RoleIndex);
        JobText.text = dT_Role.Name;
        NameText.text = _unitData.unitInfo_Immutable.Name;
        TraitText.text = $"특성 {_unitData.traitIndexList.Count}개";
        PotentialPowerText.text = "잠재력";
        ValueText.text = _unitData.GetUnitValue().ToString();
    }

    public void OnClick_SellButton()
    {

    }

    public void OnClick_InfoButton()
    {

    }

    public void OnClick_SquadCard()
    {
        var result = PlayerManager.Instance.SquadActionUnitCard(unitUniqueID, true);

        if (result)
        {
            var panel_Squad = PanelRenderQueueManager.Instance.GetPanel<Panel_Squad>();
            panel_Squad.PanelUpdate();
        }
    }
}

public class Panel_Squad : PanelAbstract
{
    public Transform Grid_InSquad;
    public Transform Grid_Squad;

    public List<GridItem_SquadCard> gridItem_SquadCards;
    public List<GridItem_InSquadCard> gridItem_InSquadCards;

    void Awake()
    {
        Grid_InSquad = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Grid_InSquad");
        Grid_Squad = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Grid_Squad");
    }

    public override void Open()
    {
        base.Open();
        FrontInfoCanvas.Instance.SetPanelName("스쿼드");
    }

    public void PanelUpdate()
    {
        Init_Squad();
        Init_InSquad();
    }

    private void Init_Squad()
    {
        var unitDatas = PlayerManager.Instance.GetPlayer_SquadUnitDatas();

        gridItem_SquadCards = new List<GridItem_SquadCard>();
        for (int i = 0; i < unitDatas.Count; i++)
        {
            var childItem = UIUtility.GetChildAutoCraete(Grid_Squad, i);
            childItem.SetActive(true);
            var gridItem_SquadCard = new GridItem_SquadCard();
            gridItem_SquadCard.Init(childItem);
            gridItem_SquadCard.Set(unitDatas[i]);

            gridItem_SquadCards.Add(gridItem_SquadCard);
        }

        for (int i = unitDatas.Count; i < Grid_Squad.childCount; i++)
        {
            Grid_Squad.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Init_InSquad()
    {
        var unitDatas = PlayerManager.Instance.GetPlayer_InSquadUnitDatas();

        gridItem_InSquadCards = new List<GridItem_InSquadCard>();
        for (int i = 0; i < unitDatas.Count; i++)
        {
            var childItem = UIUtility.GetChildAutoCraete(Grid_InSquad, i);
            childItem.SetActive(true);
            var gridItem_InSquadCard = new GridItem_InSquadCard();
            gridItem_InSquadCard.Init(childItem);
            gridItem_InSquadCard.Set(unitDatas[i]);

            gridItem_InSquadCards.Add(gridItem_InSquadCard);
        }

        for (int i = unitDatas.Count; i < Grid_InSquad.childCount; i++)
        {
            Grid_InSquad.GetChild(i).gameObject.SetActive(false);
        }
    }

}
