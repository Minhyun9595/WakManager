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

    // 캐릭터 생성할 때 고정
    public EUnitTier eUnitTier;
    public List<int> traitIndexList;
    public Unitcondition unitCondition;
    public int PotentialPoints;

    // 성장 한계치
    public AddStat addStat;

    // 고정이지만 로딩할 때 매 번 새로 생성 (변경될 수 있으므로)
    public List<DT_Skill> skillList;
    public List<DT_Trait> traitList;

    public static UnitData CreateNewUnit(EUnitTier _eUnitTier, int _unitIndex)
    {
        UnitData unitData = new UnitData();
        unitData.eUnitTier = _eUnitTier;
        unitData.unitIndex = _unitIndex;
        unitData.InitializeTier(_eUnitTier);

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
    
    public void InitializeTier(EUnitTier _eUnitTier)
    {
        eUnitTier = _eUnitTier;

        unitUniqueID = System.Guid.NewGuid().ToString();
        unitStat = DT_UnitStat.GetInfoByIndex(unitIndex);
        unitInfo_Immutable = DT_UnitInfo_Immutable.GetInfoByIndex(unitIndex);

        int AddStatPoint = 0;
        switch (eUnitTier)
        {
            case EUnitTier.WorldClass:
                AddStatPoint = 150;
                PotentialPoints = Random.Range(0, 100);
                break;
            case EUnitTier.LeagueStar:
                AddStatPoint = 100;
                PotentialPoints = Random.Range(0, 100);
                break;
            case EUnitTier.FirstTeam:
                AddStatPoint = 70;
                PotentialPoints = Random.Range(0, 100);
                break;
            case EUnitTier.Rotation:
                AddStatPoint = 40;
                PotentialPoints = Random.Range(0, 100);
                break;
            case EUnitTier.Prospect:
                AddStatPoint = 20;
                PotentialPoints = Random.Range(0, 100);
                break;
            case EUnitTier.SurplustoRequirements:
                AddStatPoint = 10;
                PotentialPoints = Random.Range(0, 100);
                break;
            default:
                break;
        }

        unitCondition = new Unitcondition(_eUnitTier);
        skillList = new List<DT_Skill>();
        foreach (var skillNmae in unitInfo_Immutable.SkillNameList)
        {
            var dtSkill = DT_Skill.GetInfoByIndex(skillNmae);
            skillList.Add(dtSkill);
        }

        traitIndexList = new List<int>();
        traitList = new List<DT_Trait>();
        var Types = DT_Trait.GetTypes();

        // 특성 생성
        var randomTraitCount = 0;
        randomTraitCount = Random.Range(0, unitInfo_Immutable.MaxTraitCount + 1);
        for (int i = 0; i < randomTraitCount; i++)
        {
            CreateTrait(ref Types);
        }

    }

    public void LoadUnit()
    {
        // 이미 로드된 상태이므로 캐릭터 고정 스탯만 다시 로드
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
        
        // 컨디션 점수
        
        // 티어 점수

        return point;
    }

    private void CreateTrait(ref List<string> _types)
    {
        // 특성 정하기
        var rand = Random.Range(0, _types.Count);
        var TraitIndex = _types[rand];

        // 한번 뽑은 특성은 빼기
        _types.RemoveAt(rand);

        // 등급 종류 확인
        var infoList = DT_Trait.GetInfoByIndex_Type(TraitIndex).OrderBy(x => x.Value.Rank).ToList();
        // 앞에서부터 45, 35, 10, 7%, 3% 확률로 지급
        var percentTable = new List<int>() { 45, 80, 90, 97, 100 };
        var rateRand = Random.Range(0, 100); // 0 ~ 99

        // 등급 정하기 (개수가 부족한 경우는? 일단 없다고 가정)
        foreach (var info in infoList)
        {
            // 개수 채우면 나가기
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

        int min = 1;
        int max = 21;
        switch (_eUnitTier)
        {
            case EUnitTier.WorldClass:
                min = 15;
                max = 21;
                break;
            case EUnitTier.LeagueStar:
                min = 11;
                max = 19;
                break;
            case EUnitTier.FirstTeam:
                min = 10;
                max = 17;
                break;
            case EUnitTier.Rotation:
                min = 3;
                max = 13;
                break;
            case EUnitTier.Prospect:
                min = 2;
                max = 10;
                break;
            case EUnitTier.SurplustoRequirements:
                min = 1;
                max = 7;
                break;
        }

        Professionalism = Random.Range(min, max);
        Ambition = Random.Range(min, max);
        Injury_Proneness = Random.Range(min, max);
        Consistency = Random.Range(min, max);
        Pressure_Handling = Random.Range(min, max);
        Teamwork = Random.Range(min, max);
        Preparation = Random.Range(min, max);
        Diligence = Random.Range(min, max);

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
        totalValue /= 5;
        return totalValue;
    }

    public void AddStat(EAddStatType eAddStatType)
    {
        switch(eAddStatType)
        {
            case EAddStatType.Health:
                break;
            case EAddStatType.Damage:
                break;
            case EAddStatType.Armor:
                break;
            case EAddStatType.MagicArmor:
                break;
            case EAddStatType.CriticalChance:
                break;
            case EAddStatType.CriticalDamage:
                break;
        }
    }

}

public enum EAddStatType
{
    Health,
    Damage,
    Armor,
    MagicArmor,
    CriticalChance,
    CriticalDamage,
}
public class AddStat
{
    public int Health;
    public float Damage;
    public float AttackSpeed;
    public int Range;
    public int Armor;
    public int MagicArmor;
    public int CriticalChance;
    public int CriticalRatio;
}
