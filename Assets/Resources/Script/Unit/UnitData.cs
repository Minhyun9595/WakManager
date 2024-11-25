using System;
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
    public int AddStatPoint;
    public int PotentialPoints;
    public int Population;

    // 일정
    public ScheduleDate schedule;

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

        var tierInfo = DT_TierInfo.GetInfoByIndex(eUnitTier);
        AddStatPoint = tierInfo.AddStatPoint;
        PotentialPoints = UnityEngine.Random.Range(tierInfo.PotentialPointMin, tierInfo.PotentialPointMax); 

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
        randomTraitCount = UnityEngine.Random.Range(0, unitInfo_Immutable.MaxTraitCount + 1);
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
        var rand = UnityEngine.Random.Range(0, _types.Count);
        var TraitIndex = _types[rand];

        // 한번 뽑은 특성은 빼기
        _types.RemoveAt(rand);

        // 등급 종류 확인
        var infoList = DT_Trait.GetInfoByIndex_Type(TraitIndex).OrderBy(x => x.Value.Rank).ToList();
        // 앞에서부터 45, 35, 10, 7%, 3% 확률로 지급
        var percentTable = new List<int>() { 45, 80, 90, 97, 100 };
        var rateRand = UnityEngine.Random.Range(0, 100); // 0 ~ 99

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

    public bool AddSchedule(int day, EUnitScheduleType eUnitScheduleType)
    {
        // 현재 날짜 가져오기
        DateTime currentDate = PlayerManager.Instance.gameSchedule.CurrentDate;
        // 오늘부터 day일을 더한 날짜 계산
        DateTime targetDate = currentDate.AddDays(day);

        // 스케줄이 존재하고, 오늘 이후라면 실패 반환
        if (schedule != null)
        {
            DateTime existingScheduleDate = new DateTime(schedule.Year, schedule.Month, schedule.Day);

            if (existingScheduleDate > currentDate)
            {
                return false; // 실패
            }
        }

        // 스케줄이 null이거나 오늘이라면 새 스케줄을 생성
        schedule = new ScheduleDate(targetDate.Year, targetDate.Month, targetDate.Day);

        return true;
    }

    public int GetScheduleLeftDay()
    {
        // 스케줄이 null이면 -1 반환 (스케줄이 없음을 나타냄)
        if (schedule == null)
            return 0;

        // 현재 날짜 가져오기
        DateTime currentDate = PlayerManager.Instance.gameSchedule.CurrentDate;

        // 스케줄 날짜 생성
        DateTime scheduleDate = new DateTime(schedule.Year, schedule.Month, schedule.Day);

        // 남은 일수를 계산
        int daysLeft = (scheduleDate - currentDate).Days;

        // 남은 일수가 음수라면 0 반환 (스케줄이 이미 지난 경우)
        return daysLeft < 0 ? 0 : daysLeft;
    }

    public void SoloStream(TeamInfo teamInfo)
    {
        var rand = UnityEngine.Random.Range(0, 100);
        var addPopulation = 0;
        var addMoney = Population * 300.0f;
        var state = "";
        if(rand <= 10) // 대성공
        {
            state = "대성공";
            addPopulation = 100;
            addMoney *= 3.0f;
        }
        else if (rand <= 20) // 성공
        {
            state = "큰 성공";
            addPopulation = 60;
            addMoney = 2.0f;
        }
        else if (rand <= 40) // 성공
        {
            state = "작은 성공";
            addPopulation = 50;
            addMoney = 1.5f;
        }
        else if (rand <= 70) // 성공
        {
            state = "무난한 방송";
            addPopulation = 40;
            addMoney = 1.0f;
        }
        else if (rand <= 90) // 작은 실패
        {
            state = "작은 실패";
            addPopulation = 20;
            addMoney = 0.5f;
        }
        else // 대실패
        {
            state = "대실패";
            addPopulation = 10;
            addMoney = 0.2f;
        }

        NotificationManager.Instance.ShowNotification($"[{state}]{unitInfo_Immutable.Name} 개인 방송으로 {addMoney} 수익과 인기 {addPopulation}를  얻었습니다.");
        teamInfo.AddMoney(EMoneyType.Activity_SoloStream, (int)addMoney);
        Population += addPopulation;
    }
}


public class Unitcondition
{
    public int CurrentContdition;
    public DT_Condition dt_Condition;
    public EUnitTier eUnitTier;

    public int Professionalism; // 프로페셔널리즘
    public int Ambition; // 야망
    public int Injury_Proneness; // 부상 성향
    public int Consistency; // 일관성
    public int Pressure_Handling; // 압박 처리 능력
    public int Teamwork; // 팀워크
    public int Preparation; // 준비성
    public int Diligence; // 근면성
    public int Royalty; // 충성심

    public Unitcondition(EUnitTier _eUnitTier)
    {
        eUnitTier = _eUnitTier;

        var tierInfo = DT_TierInfo.GetInfoByIndex(eUnitTier);
        int min = tierInfo.ConditionStatMin;
        int max = tierInfo.ConditionStatMax;

        Professionalism = UnityEngine.Random.Range(min, max);
        Ambition = UnityEngine.Random.Range(min, max);
        Injury_Proneness = UnityEngine.Random.Range(min, max);
        Consistency = UnityEngine.Random.Range(min, max);
        Pressure_Handling = UnityEngine.Random.Range(min, max);
        Teamwork = UnityEngine.Random.Range(min, max);
        Preparation = UnityEngine.Random.Range(min, max);
        Diligence = UnityEngine.Random.Range(min, max);
        Royalty = UnityEngine.Random.Range(min, max);

        CurrentContdition = DT_Const.GetInfoByIndex("CONDITION_START");
        UpdateCondition();
    }

    private void UpdateCondition()
    {
        dt_Condition = DT_Condition.GetCondition(CurrentContdition);
    }

    public EUnitConditionType GetCondition()
    {
        UpdateCondition();
        return dt_Condition.eUnitConditionType;
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
        var totalValue = (Professionalism + Ambition + Injury_Proneness + Consistency + Pressure_Handling + Teamwork + Preparation + Diligence + Royalty);
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

    public void SetSchedule(int day)
    {

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
