using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public string unitUniqueID;
    public int unitIndex;
    public DT_UnitStat unitStat;
    public DT_UnitInfo_Immutable unitInfo_Immutable;

    // ĳ���� ������ �� ����
    public List<int> traitIndexList;

    // ���� �Ѱ�ġ

    // ���������� �ε��� �� �� �� ���� ���� (����� �� �����Ƿ�)
    public List<DT_Skill> skillList;
    public List<DT_Trait> traitList;

    public static UnitData CreateNewUnit(int _unitIndex)
    {
        UnitData unitData = new UnitData();
        unitData.unitUniqueID = System.Guid.NewGuid().ToString();
        unitData.unitIndex = _unitIndex;
        unitData.unitStat = DT_UnitStat.GetInfoByIndex(_unitIndex);
        unitData.unitInfo_Immutable = DT_UnitInfo_Immutable.GetInfoByIndex(_unitIndex);

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
        UnitData newUnitData = CreateNewUnit(origin_UnitIndex);
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
