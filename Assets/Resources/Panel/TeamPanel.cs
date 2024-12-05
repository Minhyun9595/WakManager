using QUtility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Globalization;

public class GridItem_Unit : GridAbstract, GridInterface
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI InfoText;
    public Image UnitImageBG;
    public Image UnitImage;
    public Slider HpSlider;
    public Animator UnitImageAnimator;
    public Transform GridLayout_Trait;

    private Sprite[] animationSprites;
    private float frameRate = 0.1f;
    private int currentFrame = 0;
    private float timer;

    public new void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);
        NameText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "NameText");
        InfoText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "InfoText");
        UnitImageBG = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "UnitImageBG");
        UnitImage = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "UnitImage");
        HpSlider = UIUtility.FindComponentInChildrenByName<Slider>(gameObject, "HpSlider");
        UnitImageAnimator = UIUtility.FindComponentInChildrenByName<Animator>(gameObject, "UnitImage");
        GridLayout_Trait = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "GridLayout_Trait");
    }

    public void Set(Unit_AI unitAI)
    {
        var blackboard = unitAI.blackboard;
        var unitFieldInfo = blackboard.unitFieldInfo;
        var teamIndex = blackboard.teamIndex;
        NameText.text = blackboard.realUnitData.unitInfo_Immutable.Name;
        NameText.faceColor = UIUtility.GetTeamColor(teamIndex);

        Update(unitAI);

        var animationClips = unitAI.blackboard.unitAnimator.GetAnimationClips();
        // animationClips�߿� �̸��� Idle�� ���� �� ã�Ƽ� spirtes�� �ִ´�.
        UnitImageAnimator.runtimeAnimatorController = unitAI.blackboard.unitAnimator.animator.runtimeAnimatorController;
        UnitImageAnimator.Play("Idle_Image");
    }

    public void UpdateAnimation()
    {
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % animationSprites.Length;
            UnitImage.sprite = animationSprites[currentFrame];
        }
    }

    public void Update(Unit_AI unitAI)
    {
        var blackboard = unitAI.blackboard;
        var unitFieldInfo = blackboard.unitFieldInfo;
        InfoText.text = $"����: {blackboard.realUnitData.GetRoleName()}";
        HpSlider.value = unitFieldInfo.Hp / unitFieldInfo.FullHp;
    }
}

public class TeamPanel : MonoBehaviour
{
    public TeamInfo teamInfo;
    public TextMeshProUGUI Title;
    public Transform GridLayout_Unit;
    public Transform UnitInfoItem;

    private int focusTeamIndex;
    [SerializeField] private List<Tuple<Unit_AI, GridItem_Unit>> tupleList;

    void Awake()
    {
        Title = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "Title");
        GridLayout_Unit = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "GridLayout_Unit");
        UnitInfoItem = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "UnitInfoItem");
    }

    public void Initialize(TeamInfo _teamInfo, int _focusTeamIndex, List<Unit_AI> _unitAIList)
    {
        teamInfo = _teamInfo;
        focusTeamIndex = _focusTeamIndex;
        Title.text = $"�� [{teamInfo.Name}]";

        tupleList = new List<Tuple<Unit_AI, GridItem_Unit>>();
        for (int i = 0; i < _unitAIList.Count; i++)
        {
            var childItem = QUtility.UIUtility.GetChildAutoCraete(GridLayout_Unit, i);
            var gridItem_Unit = new GridItem_Unit();
            gridItem_Unit.Init(childItem);
            gridItem_Unit.Set(_unitAIList[i]);

            tupleList.Add(new Tuple<Unit_AI, GridItem_Unit>(_unitAIList[i], gridItem_Unit));
        }
    }

    public void InitializeTest(int _focusTeamIndex, List<Unit_AI> _unitAIList)
    {
        focusTeamIndex = _focusTeamIndex;
        Title.text = $"�� �׽�Ʈ {_focusTeamIndex + 1}";

        tupleList = new List<Tuple<Unit_AI, GridItem_Unit>>();
        for (int i = 0; i < _unitAIList.Count; i++)
        {
            var childItem = QUtility.UIUtility.GetChildAutoCraete(GridLayout_Unit, i);
            var gridItem_Unit = new GridItem_Unit();
            gridItem_Unit.Init(childItem);
            gridItem_Unit.Set(_unitAIList[i]);

            tupleList.Add(new Tuple<Unit_AI, GridItem_Unit>(_unitAIList[i], gridItem_Unit));
        }
    }

    public void UpdateUnit()
    {
        foreach (var _tuple in tupleList)
        {
            var _unitAI = _tuple.Item1;
            var _gridItem = _tuple.Item2;
            _gridItem.Update(_unitAI);
        }
    }
}
