using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit_FieldData
{
    private Blackboard blackboard;
    private UnitData unitData;
    public float FullHp;
    public float Hp;
    public float NormalAction_LeftCoolTime;
    public bool isDead;
    public Dictionary<int, DT_Trait> traits;
    public bool isNoneTargeting = false;

    private const float AddingFontHeightCoefficient = 0.3f;
    private const float AddingDelayTimeCoefficient = 0.18f;

    public Unit_FieldData(Blackboard _blackboard, UnitData _unitData)
    {
        blackboard = _blackboard;
        unitData = _unitData;

        FullHp = unitData.unitStat.Health;
        Hp = FullHp;
        NormalAction_LeftCoolTime = 0.0f;

        isDead = false;
    }

    public void Update(float _deltaTime)
    {
        if (blackboard.isAnimationPlaying == false)
        {
            NormalAction_LeftCoolTime -= _deltaTime;
        }
    }

    public void AttackActionResetCoolTime()
    {
        NormalAction_LeftCoolTime = 1 / unitData.unitStat.AttackSpeed;
    }

    public List<float> Hit(EDamageType _damageType, List<DamageInfo> _damageList, Vector3 _position)
    {
        List<float> convertDamageInfoList = new List<float>();

        if (IsCanNotTarget())
            return convertDamageInfoList;

        var beforeDeadState = IsCanNotTarget();
        var myArmor = unitData.unitStat.Armor;
        var myMagicArmor = unitData.unitStat.MagicArmor;

        for (int i = 0; i < _damageList.Count; i++)
        {
            var damageInfo = _damageList[i];
            var convertDamage = damageInfo.damage;

            //#Ư�� ���䰡��: ���� ������ {0}�� ���ҵ� ���ظ� �Խ��ϴ�. (��Ʈ �� ����)
            convertDamage = Convert_Leather(convertDamage);

            if (_damageType == EDamageType.Magical)
            {
                // ���� ���� ȿ��
                convertDamage = damageInfo.damage - myMagicArmor;
            }
            else if (_damageType == EDamageType.Physical)
            {
                // ���� ���� ȿ��
                convertDamage = damageInfo.damage - myArmor;
            }
            else if (_damageType == EDamageType.True)
            {
                convertDamage = damageInfo.damage;
            }

            //#Ư�� �γ���: ���� ���ظ� {0}% ���ҽ�ŵ�ϴ�.
            convertDamage = Convert_Patience(convertDamage);

            // 1 ���� ���� ���ش� �����Ѵ�.
            if (convertDamage <= 1)
            {
                convertDamage = 0;
            }

            Hp -= convertDamage;
            convertDamageInfoList.Add(convertDamage);

            blackboard.unitReport.AddReceiveDamage(_damageType, convertDamage);

            var addingFontHeight = i * AddingFontHeightCoefficient;
            var addingDelayTime = i * AddingDelayTimeCoefficient;
            DamageFont.Spawn(_position + new Vector3(0, 0.5f + (addingFontHeight), 0), convertDamage, QUtility.UIUtility.GetDamageColor(damageInfo.isCritical), addingDelayTime);
        }

        // �׾����� üũ
        if (IsCanNotTarget())
        {
            Debug.Log($"{unitData.unitStat.Name} ���");
        }

        // UI ������Ʈ

        var teamPanel = FieldManager.Instance.GetTeamPanel(blackboard.teamIndex);
        teamPanel.UpdateUnit();

        return convertDamageInfoList;
    }

    public bool IsCanNotTarget()
    {
        return Hp <= 0 || isNoneTargeting;
    }

    public bool IsDead()
    {
        return Hp <= 0;
    }

    public float Convert_Leather(float originalDamage)
    {
        float convertDamage = originalDamage;
        var trait = blackboard.realUnitData.GetTrait(TraitType.����_����);

        if(trait != null)
        {
            convertDamage -= trait.Value1;
            convertDamage = Mathf.Max(0, convertDamage);
        }

        return convertDamage; 
    }

    public float Convert_Patience(float originalDamage)
    {
        float convertDamage = originalDamage;
        var trait = blackboard.realUnitData.GetTrait(TraitType.�γ���);

        if (trait != null)
        {
            convertDamage *= (1.0f - trait.Value1 * 0.01f);
            convertDamage = Mathf.Max(0, convertDamage);
        }

        return convertDamage;
    }
}