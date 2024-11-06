using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public int unitIndex;
    public DT_UnitStat unitStat;
    public DT_UnitInfo_Immutable unitInfo_Immutable;

    // ĳ���� ������ �� ����
    public List<int> traitIndexList;

    // ���������� �ε��� �� �� �� ���� ���� (����� �� �����Ƿ�)
    public List<DT_Skill> skillList;
    public List<DT_Trait> traitList;

    public static UnitData CreateNewUnit(int _unitIndex)
    {
        UnitData unitData = new UnitData();
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
        for (int i = 0; i < unitData.unitInfo_Immutable.MaxTraitCount; i++)
        {
            unitData.CreateTrait(ref Types);
        }

        return unitData;
    }

    public UnitData LoadUnit(string jsonData)
    {
        var unitData = JsonUtility.FromJson<UnitData>(jsonData);

        return unitData;
    }

    public string SaveUnit()
    {
        return JsonUtility.ToJson(this);
    }

    public void CreateTrait(ref List<string> _types)
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
                var targetTrait = infoList[rankIndex].Value;
                traitIndexList.Add(targetTrait.TraitIndex);
                traitList.Add(targetTrait);
                break;
            }
        }
    }
}
