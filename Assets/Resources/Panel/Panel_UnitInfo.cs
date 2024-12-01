using QUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_UnitInfo : PanelAbstract
{
    // Unit
    public Animator UnitImageAnimator;
    public Image TeamLogoImage;
    public TextMeshProUGUI UnitNameText;
    public TextMeshProUGUI UnitInfoText;

    // Condition
    public List<Image> ConditionLines;
    public Image ConditionLine_1;
    public Image ConditionLine_2;
    public Image ConditionLine_3;
    public Image ConditionLine_4;
    public Image ConditionLine_5;
    public Image ConditionLine_6;
    public Image ConditionLine_7;
    public Image ConditionLine_8;
    public TextMeshProUGUI ProfessionalismText;
    public TextMeshProUGUI Injury_PronenessText;
    public TextMeshProUGUI AmbitionText;
    public TextMeshProUGUI Consistency;
    public TextMeshProUGUI TeamworkText;
    public TextMeshProUGUI PreparationText;
    public TextMeshProUGUI DiligenceText;
    public TextMeshProUGUI RoyaltyText;

    // Stat
    public TextMeshProUGUI StatText;
    public Image SkillImage;
    public TextMeshProUGUI SkillName;
    public TextMeshProUGUI SkillDesc;

    public Transform Grid_UnitTraitScroll;
    public List<GridItem_UnitInfoTrait> gridList;

    public Button ExitButton;

    private const float MaxRadius = 200.0f; // �׷��� �ִ� ������

    void Awake()
    {
        // �ʱ�ȭ
        UnitImageAnimator = UIUtility.FindComponentInChildrenByName<Animator>(gameObject, "UnitImage");
        TeamLogoImage = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "TeamLogoImage");
        UnitNameText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "UnitNameText");
        UnitInfoText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "UnitInfoText");

        // Condition Lines
        ConditionLine_1 = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "ConditionLine_1");
        ConditionLine_2 = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "ConditionLine_2");
        ConditionLine_3 = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "ConditionLine_3");
        ConditionLine_4 = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "ConditionLine_4");
        ConditionLine_5 = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "ConditionLine_5");
        ConditionLine_6 = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "ConditionLine_6");
        ConditionLine_7 = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "ConditionLine_7");
        ConditionLine_8 = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "ConditionLine_8");
        ConditionLines.Add(ConditionLine_1);
        ConditionLines.Add(ConditionLine_2);
        ConditionLines.Add(ConditionLine_3);
        ConditionLines.Add(ConditionLine_4);
        ConditionLines.Add(ConditionLine_5);
        ConditionLines.Add(ConditionLine_6);
        ConditionLines.Add(ConditionLine_7);
        ConditionLines.Add(ConditionLine_8);

        ProfessionalismText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "ProfessionalismText");
        Injury_PronenessText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "Injury_PronenessText");
        AmbitionText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "AmbitionText");
        Consistency = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "Consistency");
        TeamworkText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TeamworkText");
        PreparationText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "PreparationText");
        DiligenceText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "DiligenceText");
        RoyaltyText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "RoyaltyText");


        // Stats
        StatText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "StatText");
        SkillImage = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "SkillImage");
        SkillName = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "SkillName");
        SkillDesc = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "SkillDesc");

        // Grid and Exit
        Grid_UnitTraitScroll = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Grid_UnitTraitScroll");
        ExitButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "ExitButton");

        // Exit Button Listener
        ExitButton.onClick.AddListener(Close);
    }

    public override void Open()
    {
        base.Open();
        FrontInfoCanvas.Instance.SetPanelName("���� ����");
    }

    public void SetUnit(UnitData unitData)
    {
        var controller = unitData.unitInfo_Immutable.GetRuntimeAnimatorController();
        UnitImageAnimator.runtimeAnimatorController = controller;
        UnitImageAnimator.Play("Idle_Image");
        //TeamLogoImage.sprite = UIUtility.GetSprite(unitData.unitInfo_Immutable.TeamLogoSprite);
        UnitNameText.text = unitData.unitInfo_Immutable.Name;
        StringBuilder stringBuilder_UnitInfo = new StringBuilder();
        stringBuilder_UnitInfo.AppendLine($"����: {unitData.GetRoleName()}");
        stringBuilder_UnitInfo.AppendLine($"�Ҽ�: {unitData.unitInfo_Immutable.Affiliation}");
        stringBuilder_UnitInfo.AppendLine($"�޿�: {UIUtility.GetUnitizeText(unitData.Pay)}$");
        stringBuilder_UnitInfo.AppendLine($"������: {unitData.AddStatPoint}");
        UnitInfoText.text = stringBuilder_UnitInfo.ToString();

        var unitCondition = unitData.unitCondition;

        ProfessionalismText.text = $"������\n{unitCondition.Professionalism}";
        Injury_PronenessText.text = $"�λ� ����\n{unitCondition.Injury_Proneness}";
        AmbitionText.text = $"�߸�\n{unitCondition.Ambition}";
        Consistency.text = $"�ϰ���\n{unitCondition.Consistency}";
        TeamworkText.text = $"����ũ\n{unitCondition.Teamwork}";
        PreparationText.text = $"�غ�\n{unitCondition.Preparation}";
        DiligenceText.text = $"�ٸ���\n{unitCondition.Diligence}";
        RoyaltyText.text = $"�漺��\n{unitCondition.Royalty}";

        //�⺻����
        //���� Ÿ��: �ٰŸ�
        //������ Ÿ��: ����
        //��Ÿ�: 
        //ü��:
        //���ݷ�:
        //����:
        //�������׷�:
        //�̵��ӵ�:
        //ġ��Ÿ Ȯ��:
        //ġ��Ÿ ����:

        StringBuilder stringBuilder_Stat = new StringBuilder();
        stringBuilder_Stat.AppendLine($"�⺻����");
        stringBuilder_Stat.AppendLine($"���� Ÿ��: {unitData.unitInfo_Immutable.GetAttackTypeText()}");
        stringBuilder_Stat.AppendLine($"������ Ÿ��: {unitData.unitStat.GetDamageTypeText()}");
        stringBuilder_Stat.AppendLine($"��Ÿ�: {unitData.unitStat.Range}");
        stringBuilder_Stat.AppendLine($"ü��: {unitData.unitStat.Health}");
        stringBuilder_Stat.AppendLine($"���ݷ�: {unitData.unitStat.Damage}");
        stringBuilder_Stat.AppendLine($"����: {unitData.unitStat.Armor}");
        stringBuilder_Stat.AppendLine($"�������׷�: {unitData.unitStat.MagicArmor}");
        stringBuilder_Stat.AppendLine($"�̵��ӵ�: {unitData.unitStat.GetSpeedText()}");
        stringBuilder_Stat.AppendLine($"ġ��Ÿ Ȯ��: {unitData.unitStat.CriticalChance / 100}%");
        stringBuilder_Stat.AppendLine($"ġ��Ÿ ����: {unitData.unitStat.CriticalRatio / 100}%");
        StatText.text = stringBuilder_Stat.ToString();

        if (unitData?.skillList.Count != 0)
        {
            SkillImage.gameObject.SetActive(true);
            var skillInfo = unitData.skillList[0];
            SkillImage.sprite = UIUtility.GetSprite(skillInfo.IconName);
            SkillName.text = skillInfo.Name;
            SkillDesc.text = skillInfo.Description;
        }
        else
        {
            SkillImage.gameObject.SetActive(false);
            SkillName.text = "��ų����";
            SkillDesc.text = "";
        }

        List<int> conditions = new List<int>();
        conditions.Add(unitData.unitCondition.Professionalism);
        conditions.Add(unitData.unitCondition.Injury_Proneness);
        conditions.Add(unitData.unitCondition.Ambition);
        conditions.Add(unitData.unitCondition.Consistency);
        conditions.Add(unitData.unitCondition.Teamwork);
        conditions.Add(unitData.unitCondition.Preparation);
        conditions.Add(unitData.unitCondition.Diligence);
        conditions.Add(unitData.unitCondition.Royalty);

        DrawConditionPolygon(conditions);
        UpdateTraitGrid(unitData.traitList);
    }

    public void DrawConditionPolygon(List<int> conditions)
    {
        if (conditions.Count != 8)
        {
            Debug.LogError("Conditions �迭 ũ��� �ݵ�� 8�̾�� �մϴ�.");
            return;
        }

        // 8���� ������ ��ǥ ���
        Vector2[] points = new Vector2[8];
        float angleStep = 360f / 8;

        for (int i = 0; i < 8; i++)
        {
            float radius = (conditions[i] / 20f) * MaxRadius; // ���� ���� ���������� ��ȯ
            float angle = Mathf.Deg2Rad * (i * angleStep);    // ������ �������� ��ȯ

            // ��ǥ ���
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);

            points[i] = new Vector2(x, y);
        }

        // ConditionLine ��ġ ������Ʈ (7�� ���)
        for (int i = 0; i < ConditionLines.Count; i++)
        {
            Vector2 start = points[i];
            Vector2 end = points[(i + 1) % 8]; // ���� ������ (8��° ���� 0���� ����)

            UpdateLinePosition(ConditionLines[i].rectTransform, start, end);
        }
    }

    private void UpdateLinePosition(RectTransform line, Vector2 start, Vector2 end)
    {
        // ���� �߽��� ���
        Vector2 midpoint = (start + end) / 2;

        // ���� ���� ���
        Vector2 direction = end - start;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ���� ���� ���
        float length = direction.magnitude;

        // RectTransform ������Ʈ
        line.anchoredPosition = midpoint;               // �߽��� ��ġ
        line.sizeDelta = new Vector2(length, line.sizeDelta.y); // ���� ����
        line.localRotation = Quaternion.Euler(0, 0, angle);     // ȸ�� ����
    }

    private void UpdateTraitGrid(List<DT_Trait> traits)
    {
        gridList = new List<GridItem_UnitInfoTrait>();

        for (int i = 0; i < traits.Count; i++)
        {
            var childItem = UIUtility.GetChildAutoCraete(Grid_UnitTraitScroll, i);
            var gridItem = new GridItem_UnitInfoTrait();
            gridItem.Init(childItem);
            gridItem.SetTrait(traits[i]);

            gridList.Add(gridItem);
            childItem.gameObject.SetActive(true);
        }

        for (int i = traits.Count; i < Grid_UnitTraitScroll.childCount; i++)
        {
            Grid_UnitTraitScroll.GetChild(i).gameObject.SetActive(false);
        }
    }
}

public class GridItem_UnitInfoTrait : GridAbstract, GridInterface
{
    public int traitIndex = -1;
    public Image TraitImage;
    public TextMeshProUGUI TraitRateText;
    public TextMeshProUGUI TraitName;
    public TextMeshProUGUI TraitDesc;

    public new void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);
        TraitImage = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "TraitImage");
        TraitRateText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TraitRateText");
        TraitName = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TraitName");
        TraitDesc = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TraitDesc");
    }

    public void SetTrait(DT_Trait dT_Trait)
    {
        traitIndex = dT_Trait.TraitIndex;
        TraitImage.sprite = UIUtility.GetSprite(dT_Trait.IconSprite);
        TraitRateText.text = dT_Trait.GetRankString();
        TraitName.text = dT_Trait.Name;
        TraitDesc.text = dT_Trait.GetDesc();
    }
}