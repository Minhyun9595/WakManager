using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


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
        AttackActionResetCoolTime();

        isDead = false;
    }

    public void Update(float _deltaTime)
    {
        NormalAction_LeftCoolTime -= _deltaTime;
    }

    public void AttackActionResetCoolTime()
    {
        NormalAction_LeftCoolTime = 1 / unitData.unitStat.AttackSpeed;
    }

    public bool Hit(EDamageType _damageType, List<DamageInfo> _damageList, Vector3 _position)
    {
        if (IsCanNotTarget())
            return false;

        var beforeDeadState = IsCanNotTarget();
        var myArmor = unitData.unitStat.Armor;
        var myMagicArmor = unitData.unitStat.MagicArmor;

        for (int i = 0; i < _damageList.Count; i++)
        {
            var damageInfo = _damageList[i];
            var convertDamage = damageInfo.damage;

            //#특성 질긴가죽: 받은 피해의 {0}이 감소된 피해를 입습니다. (도트 당 적용)
            if (_damageType == EDamageType.Magical)
            {
                // 마법 공격 효과
                convertDamage = damageInfo.damage - myMagicArmor;
            }
            else if (_damageType == EDamageType.Physical)
            {
                // 물리 공격 효과
                convertDamage = damageInfo.damage - myArmor;
            }
            else if (_damageType == EDamageType.True)
            {
                convertDamage = damageInfo.damage;
            }

            //#특성 인내심: 받은 피해를 {0}% 감소시킵니다.

            // 1 보다 작은 피해는 무시한다.
            if (convertDamage <= 1)
            {
                convertDamage = 0;
            }

            Hp -= convertDamage;

            switch(_damageType)
            {
                case EDamageType.Physical:
                    blackboard.unitReport.DamageReceive_Physical += convertDamage;
                    break;
                case EDamageType.Magical:
                    blackboard.unitReport.DamageReceive_Magical += convertDamage;
                    break;
                case EDamageType.True:
                    blackboard.unitReport.DamageReceive_True += convertDamage;
                    break;
            }
            blackboard.unitReport.DamageReceive_Total += convertDamage;

            var addingFontHeight = i * AddingFontHeightCoefficient;
            var addingDelayTime = i * AddingDelayTimeCoefficient;
            DamageFont.Spawn(_position + new Vector3(0, 0.5f + (addingFontHeight), 0), convertDamage, QUtility.UIUtility.GetDamageColor(damageInfo.isCritical), addingDelayTime);
        }

        // 죽었는지 체크
        if (IsCanNotTarget())
        {
            Debug.Log($"{unitData.unitStat.Name} 사망");
        }

        // UI 업데이트

        var teamPanel = FieldManager.Instance.GetTeamPanel(blackboard.teamIndex);
        teamPanel.UpdateUnit();

        return true;
    }

    public bool IsCanNotTarget()
    {
        return Hp <= 0 || isNoneTargeting;
    }

    public bool IsDead()
    {
        return Hp <= 0;
    }
}