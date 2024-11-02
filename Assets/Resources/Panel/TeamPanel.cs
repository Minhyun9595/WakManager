using QUtility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GridItem_Unit
{
    private GameObject gameObject;

    public TextMeshProUGUI NameText;
    public TextMeshProUGUI InfoText;
    public Image UnitImageBG;
    public Image UnitImage;
    public Animator UnitImageAnimator;
    public Transform GridLayout_Trait;

    public void Init(GameObject _gameObject)
    {
        gameObject = _gameObject;
        NameText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "NameText");
        InfoText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "InfoText");
        UnitImageBG = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "UnitImageBG");
        UnitImage = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "UnitImage");
        UnitImageAnimator = UIUtility.FindComponentInChildrenByName<Animator>(gameObject, "UnitImage");
        GridLayout_Trait = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "GridLayout_Trait");
    }

    public void Set(Unit_AI unitAI)
    {
        NameText.text = unitAI.blackboard.unitData.Name;
        InfoText.text = $"Á÷¾÷: {unitAI.blackboard.unitData.Role}";

        UnitImageAnimator = unitAI.blackboard.unitAnimator.animator;

        //var clips = unitAI.blackboard.unitAnimator.GetAnimationClips();
        //if (1 <= clips.Count)
        //{
        //    var idleKey = clips.Keys.FirstOrDefault(key => key.Contains("Idle"));
        //    if (string.IsNullOrEmpty(idleKey) == false)
        //    {
        //        UnitImageAnimator.clip = clips[idleKey];
        //    }
        //    else
        //    {
        //        UnitImageAnimator.clip = clips.First().Value;
        //    }
        //    UnitImageAnimator.Play();
        //}
    }
}

public class TeamPanel : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public Transform GridLayout_Unit;
    public Transform UnitInfoItem;

    private int focusTeamIndex;
    [SerializeField] private List<Unit_AI> unitAIList;
    [SerializeField] private List<GridItem_Unit> gridItem_Units;

    void Start()
    {
        Title = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "Title");
        GridLayout_Unit = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "GridLayout_Unit");
        UnitInfoItem = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "UnitInfoItem");
    }

    public void Initialize(int _focusTeamIndex, List<Unit_AI> _unitAIList)
    {
        focusTeamIndex = _focusTeamIndex;
        Title.text = $"ÆÀ {_focusTeamIndex}\nÆÀ ÀÌ¸§³Ö´Â°÷";
        unitAIList = _unitAIList;

        gridItem_Units = new List<GridItem_Unit>();
        for (int i = 0; i < unitAIList.Count; i++)
        {
            var childItem = QUtility.UIUtility.GetChildAutoCraete(GridLayout_Unit, i);
            var gridItem_Unit = new GridItem_Unit();
            gridItem_Unit.Init(childItem);
            gridItem_Unit.Set(unitAIList[i]);

            gridItem_Units.Add(gridItem_Unit);
        }
    }
}
