using QUtility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridItem_SquadCard : GridAbstract, GridInterface
{
    public TextMeshProUGUI JobText;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI TraitText;
    public TextMeshProUGUI PotentialPowerText;
    public TextMeshProUGUI ValueText;

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

        SellButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "SellButton");
        InfoButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "InfoButton");

        SellButton.onClick.AddListener(OnClick_SellButton);
        InfoButton.onClick.AddListener(OnClick_InfoButton);
    }

    public void Set(UnitData _unitData)
    {
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
}

public class Panel_Squad : PanelAbstract
{
    public Transform Grid_InSquad;
    public Transform Grid_Squad;

    public List<GridItem_SquadCard> gridItem_SquadCards;

    void Awake()
    {
        Grid_InSquad = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Grid_InSquad");
        Grid_Squad = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Grid_Squad");
    }

    public void PanelUpdate()
    {
        Init_Squad();
    }

    private void Init_Squad()
    {
        var player_SquadUnitDatas = PlayerManager.Instance.GetPlayer_SquadUnitDatas();

        gridItem_SquadCards = new List<GridItem_SquadCard>();
        for (int i = 0; i < player_SquadUnitDatas.Count; i++)
        {
            var childItem = UIUtility.GetChildAutoCraete(Grid_Squad, i);
            childItem.SetActive(true);
            var gridItem_SquadCard = new GridItem_SquadCard();
            gridItem_SquadCard.Init(childItem);
            gridItem_SquadCard.Set(player_SquadUnitDatas[i]);

            gridItem_SquadCards.Add(gridItem_SquadCard);
        }

        for (int i = player_SquadUnitDatas.Count; i < Grid_Squad.childCount; i++)
        {
            Grid_Squad.GetChild(i).gameObject.SetActive(false);
        }
    }

}
