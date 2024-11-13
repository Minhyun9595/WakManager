using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unitcondition
{
    public int CurrentContdition;
    public EUnitConditionType EConditionType;
    public EUnitTier eUnitTier;

    public int Professionalism;
    public int Ambition;
    public int Injury_Proneness;
    public int Consistency;
    public int Pressure_Handling;
    public int Teamwork;
    public int Preparation;
    public int Diligence;

    public Unitcondition(EUnitTier _eUnitTier)
    {
        eUnitTier = _eUnitTier;
        
        Professionalism = Random.Range(1, 21);
        Ambition = Random.Range(1, 21);
        Injury_Proneness = Random.Range(1, 21);
        Consistency = Random.Range(1, 21);
        Pressure_Handling = Random.Range(1, 21);
        Teamwork = Random.Range(1, 21);
        Preparation = Random.Range(1, 21);
        Diligence = Random.Range(1, 21);

        CurrentContdition = 80;
    }

    public EUnitConditionType GetCondition()
    {
        if (CurrentContdition >= 90)
        {
            EConditionType = EUnitConditionType.Superb;
        }
        else if (CurrentContdition >= 80)
        {
            EConditionType = EUnitConditionType.Good;
        }
        else if (CurrentContdition >= 65)
        {
            EConditionType = EUnitConditionType.Okay;
        }
        else if (CurrentContdition >= 40)
        {
            EConditionType = EUnitConditionType.Poor;
        }
        else
        {
            EConditionType = EUnitConditionType.VeryPoor;
        }

        return EConditionType;
    }

    public void ProgressCondition()
    {
        var addPoint = 0;
        switch (eUnitTier)
        {
            case EUnitTier.WorldClass:
                addPoint += 50;
                break;
            case EUnitTier.LeagueStar:
                addPoint += 30;
                break;
            case EUnitTier.FirstTeam:
                addPoint += 25;
                break;
            case EUnitTier.Rotation:
                addPoint += 20;
                break;
            case EUnitTier.Prospect:
                addPoint += 10;
                break;
            case EUnitTier.SurplustoRequirements:
                addPoint += 0;
                break;
        }

    }

    public int GetConditionValue()
    {
        var totalValue = (Professionalism + Ambition + Injury_Proneness + Consistency + Pressure_Handling + Teamwork + Preparation + Diligence);
        totalValue /= 20;
        return totalValue;
    }
}

[System.Serializable]
public class UnitData
{
    public string unitUniqueID;
    public int unitIndex;
    public DT_UnitStat unitStat;
    public DT_UnitInfo_Immutable unitInfo_Immutable;

    // ĳ���� ������ �� ����
    public EUnitTier eUnitTier;
    public List<int> traitIndexList;
    public Unitcondition unitCondition;

    // ���� �Ѱ�ġ

    // ���������� �ε��� �� �� �� ���� ���� (����� �� �����Ƿ�)
    public List<DT_Skill> skillList;
    public List<DT_Trait> traitList;

    public static UnitData CreateNewUnit(EUnitTier _eUnitTier, int _unitIndex)
    {
        UnitData unitData = new UnitData();
        unitData.eUnitTier = _eUnitTier;
        unitData.unitUniqueID = System.Guid.NewGuid().ToString();
        unitData.unitIndex = _unitIndex;
        unitData.unitStat = DT_UnitStat.GetInfoByIndex(_unitIndex);
        unitData.unitInfo_Immutable = DT_UnitInfo_Immutable.GetInfoByIndex(_unitIndex);

        unitData.unitCondition = new Unitcondition(_eUnitTier);
        unitData.skillList = new List<DT_Skill>();
        foreach (var skillNmae in unitData.unitInfo_Immutable.SkillNameList)
        {
            var dtSkill = DT_Skill.GetInfoByIndex(skillNmae);
            unitData.skillList.Add(dtSkill);
        }

        unitData.traitIndexList = new List<int>();
        unitData.traitList = new List<DT_Trait>();
        var Types = DT_Trait.GetTypes();

        // Ư�� ����
        var randomTraitCount = Random.Range(0, unitData.unitInfo_Immutable.MaxTraitCount + 1);
        for (int i = 0; i < randomTraitCount; i++)
        {
            unitData.CreateTrait(ref Types);
        }

        return unitData;
    }

    public static UnitData CopyNewUnit(UnitData originUnitData)
    {
        var origin_UnitIndex = originUnitData.unitIndex;
        UnitData newUnitData = CreateNewUnit(originUnitData.eUnitTier, origin_UnitIndex);
        newUnitData.unitUniqueID = originUnitData.unitUniqueID;
        newUnitData.traitIndexList = originUnitData.traitIndexList;

        newUnitData.InitializeSkill();
        newUnitData.InitializeTrait();

        return newUnitData;
    }

    public void LoadUnit()
    {
        // �̹� �ε�� �����̹Ƿ� ĳ���� ���� ���ȸ� �ٽ� �ε�
        unitStat = DT_UnitStat.GetInfoByIndex(unitIndex);
        unitInfo_Immutable = DT_UnitInfo_Immutable.GetInfoByIndex(unitIndex);

        InitializeSkill();
        InitializeTrait();
    }

    private void InitializeSkill()
    {
        skillList.Clear();
        skillList = new List<DT_Skill>();
        foreach (var skillNmae in unitInfo_Immutable.SkillNameList)
        {
            var dtSkill = DT_Skill.GetInfoByIndex(skillNmae);
            skillList.Add(dtSkill);
        }
    }

    private void InitializeTrait()
    {
        traitList.Clear();
        for (int i = 0; i < traitIndexList.Count; i++)
        {
            var dt_trait = DT_Trait.GetInfoByIndex(traitIndexList[i]);
            traitList.Add(dt_trait);
        }
    }

    public int GetUnitValue()
    {
        var point = 20;

        for (int i = 0; i < traitList.Count; i++)
        {
            var trait = traitList[i];
            point += DT_TraitValue.GetInfoByIndex(trait.Rank).Value;
        }
        
        // ����� ����
        
        // Ƽ�� ����

        return point;
    }

    private void CreateTrait(ref List<string> _types)
    {
        // Ư�� ���ϱ�
        var rand = Random.Range(0, _types.Count);
        var TraitIndex = _types[rand];

        // �ѹ� ���� Ư���� ����
        _types.RemoveAt(rand);

        // ��� ���� Ȯ��
        var infoList = DT_Trait.GetInfoByIndex_Type(TraitIndex).OrderBy(x => x.Value.Rank).ToList();
        // �տ������� 45, 35, 10, 7%, 3% Ȯ���� ����
        var percentTable = new List<int>() { 45, 80, 90, 97, 100 };
        var rateRand = Random.Range(0, 100); // 0 ~ 99

        // ��� ���ϱ� (������ ������ ����? �ϴ� ���ٰ� ����)
        foreach (var info in infoList)
        {
            // ���� ä��� ������
            if(unitInfo_Immutable.MaxTraitCount < traitList.Count)
            {
                break;
            }

            var rank = info.Value.Rank;
            var rankIndex = rank - 1;
            if (rateRand < percentTable[rankIndex])
            {
                var dt_trait = infoList[rankIndex].Value;
                traitIndexList.Add(dt_trait.TraitIndex);
                traitList.Add(dt_trait);
                break;
            }
        }
    }

    public EDamageType GetDamageType()
    {
        return unitStat.GetDamageType();
    }

    public float GetMoveSpeed(EAxsType eAxsType)
    {
        switch (eAxsType)
        {
            case EAxsType.X:
                return unitStat.MoveSpeed_X;
            case EAxsType.Y:
                return unitStat.MoveSpeed_Y;
        }
        return 0;
    }

    public float GetRange()
    {
        return unitStat.Range * ConstValue.RangeCoefficient;
    }

    public string GetRoleName()
    {
        return DT_Role.GetInfoByIndex(unitInfo_Immutable.RoleIndex).Name;
    }
}
