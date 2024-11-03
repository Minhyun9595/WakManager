using QUtility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GridItem_Unit
{
    private GameObject gameObject;

    public TextMeshProUGUI NameText;
    public TextMeshProUGUI InfoText;
    public Image UnitImageBG;
    public Image UnitImage;
    public Animator UnitImageAnimator;
    public Transform GridLayout_Trait;

    private Sprite[] animationSprites;
    private float frameRate = 0.1f;
    private int currentFrame = 0;
    private float timer;

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
        InfoText.text = $"직업: {unitAI.blackboard.unitData.GetRoleName()}";

        var animationClips = unitAI.blackboard.unitAnimator.GetAnimationClips();
        // animationClips중에 이름에 Idle이 들어가는 걸 찾아서 spirtes에 넣는다.
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
        Title.text = $"팀 {_focusTeamIndex + 1}\n팀 이름넣는곳";
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
