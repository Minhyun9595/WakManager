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

    // ĳ���� ������ �� ����
    public EUnitTier eUnitTier;
    public List<int> traitIndexList;
    public Unitcondition unitCondition;
    public int AddStatPoint;
    public int PotentialPoints;
    public int Population;
    public int Pay;

    // ����
    public ScheduleDate schedule;

    // ���� �Ѱ�ġ
    public AddStat addStat;

    // ���������� �ε��� �� �� �� ���� ���� (����� �� �����Ƿ�)
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

        var tierInfo = DT_UnitTierInfo.GetInfoByIndex(eUnitTier);

        var addStatRand = UnityEngine.Random.Range(tierInfo.AddStatPointMin, tierInfo.AddStatPointMax + 1);
        var potentialStatRand = UnityEngine.Random.Range(tierInfo.PotentialPointMin, tierInfo.PotentialPointMax + 1);

        AddStatPoint = DT_Potential.GetRandomPotentialValue(addStatRand);
        PotentialPoints = DT_Potential.GetRandomPotentialValue(potentialStatRand);
        Pay = (int)UnityEngine.Random.Range(tierInfo.MonthCost * 0.9f, tierInfo.MonthCost * 1.1f);
        Pay = (Pay / 100) * 100; // 100 ���� ������

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

        // Ư�� ����
        var dt_unitTier = DT_UnitTierInfo.GetInfoByIndex(eUnitTier);
        var randomTraitCount = dt_unitTier.GetRandomTraitCount();
        for (int i = 0; i < randomTraitCount; i++)
        {
            CreateTrait(ref Types);
        }
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
        
        // ����� ���� (8)
        point += unitCondition.GetConditionValue();
        
        // Ƽ�� ����
        var dt_unitTier = DT_UnitTierInfo.GetInfoByIndex(eUnitTier);
        point += dt_unitTier.Value;

        return point;
    }

    private void CreateTrait(ref List<string> _types)
    {
        // Ư�� ���ϱ�
        var rand = UnityEngine.Random.Range(0, _types.Count);
        var TraitIndex = _types[rand];

        // �ѹ� ���� Ư���� ����
        _types.RemoveAt(rand);

        // ��� ���� Ȯ��
        var infoList = DT_Trait.GetInfoByIndex_Type(TraitIndex).OrderBy(x => x.Value.Rank).ToList();
        // �տ������� 45, 35, 10, 7%, 3% Ȯ���� ����
        var percentTable = new List<int>() { 45, 80, 90, 97, 100 };
        var rateRand = UnityEngine.Random.Range(0, 100); // 0 ~ 99

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

    private DT_Trait eagleEyeTrait;
    public float GetRange()
    {
        var range = unitStat.Range * ConstValue.RangeCoefficient;

        var IsRangeUnit = unitInfo_Immutable.CheckRangeUnit();
        if (IsRangeUnit)
        {
            var trait = GetTrait(TraitType.EagleEye);

            if (trait != null)
            {
                range += trait.Value1;
            }
        }

        return range;
    }

    public string GetRoleName()
    {
        return DT_Role.GetInfoByIndex(unitInfo_Immutable.RoleIndex).Name;
    }

    public bool AddSchedule(int day, EUnitScheduleType eUnitScheduleType)
    {
        // ���� ��¥ ��������
        DateTime currentDate = PlayerManager.Instance.gameSchedule.CurrentDate;
        // ���ú��� day���� ���� ��¥ ���
        DateTime targetDate = currentDate.AddDays(day);

        // �������� �����ϰ�, ���� ���Ķ�� ���� ��ȯ
        if (schedule != null)
        {
            DateTime existingScheduleDate = new DateTime(schedule.Year, schedule.Month, schedule.Day);

            if (existingScheduleDate > currentDate)
            {
                return false; // ����
            }
        }

        // �������� null�̰ų� �����̶�� �� �������� ����
        schedule = new ScheduleDate(eUnitScheduleType, targetDate.Year, targetDate.Month, targetDate.Day);

        return true;
    }

    public int GetScheduleLeftDay()
    {
        // �������� null�̸� -1 ��ȯ (�������� ������ ��Ÿ��)
        if (schedule == null)
            return 0;

        // ���� ��¥ ��������
        DateTime currentDate = PlayerManager.Instance.gameSchedule.CurrentDate;

        // ������ ��¥ ����
        DateTime scheduleDate = new DateTime(schedule.Year, schedule.Month, schedule.Day);

        // ���� �ϼ��� ���
        int daysLeft = (scheduleDate - currentDate).Days;

        // ���� �ϼ��� ������� 0 ��ȯ (�������� �̹� ���� ���)
        return daysLeft < 0 ? 0 : daysLeft;
    }

    public void SoloStream(TeamInfo teamInfo)
    {
        var rand = UnityEngine.Random.Range(0, 100);
        var addPopulation = 0;
        var addMoney = Population * 300.0f;
        var state = "";
        if(rand <= 10) // �뼺��
        {
            state = "�뼺��";
            addPopulation = 100;
            addMoney *= 3.0f;
        }
        else if (rand <= 20) // ����
        {
            state = "ū ����";
            addPopulation = 60;
            addMoney = 2.0f;
        }
        else if (rand <= 40) // ����
        {
            state = "���� ����";
            addPopulation = 50;
            addMoney = 1.5f;
        }
        else if (rand <= 70) // ����
        {
            state = "������ ���";
            addPopulation = 40;
            addMoney = 1.0f;
        }
        else if (rand <= 90) // ���� ����
        {
            state = "���� ����";
            addPopulation = 20;
            addMoney = 0.5f;
        }
        else // �����
        {
            state = "�����";
            addPopulation = 10;
            addMoney = 0.2f;
        }

        NotificationManager.Instance.ShowNotification($"[{state}]{unitInfo_Immutable.Name} ���� ������� {addMoney} ���Ͱ� �α� {addPopulation}��  ������ϴ�.");
        teamInfo.AddMoney(EMoneyType.Activity_SoloStream, (int)addMoney);
        AddPopulation(addPopulation);
    }

    public void ScheduleCheck()
    {
        // ���� �������� ������ �� ȿ�� �ߵ�
        if(PlayerManager.Instance.gameSchedule.IsToday(schedule))
        {
            // �Ʒ� ���� ���� üũ
            var resultRand = UnityEngine.Random.Range(0, 100);

            switch (schedule.EUnitScheduleType)
            {
                case EUnitScheduleType.Traning_Health:
                    NotificationManager.Instance.ShowNotification($"{unitInfo_Immutable.Name}�� ü�� �Ʒ��� ���������� �������ϴ�.");
                    break;
                case EUnitScheduleType.Traning_Damage:
                    NotificationManager.Instance.ShowNotification($"{unitInfo_Immutable.Name}�� ���ݷ� �Ʒ��� ���������� �������ϴ�.");
                    break;
                case EUnitScheduleType.Traning_Armor:
                    NotificationManager.Instance.ShowNotification($"{unitInfo_Immutable.Name}�� ���� �Ʒ��� ���������� �������ϴ�.");
                    break;
                case EUnitScheduleType.Traning_Mental:
                    NotificationManager.Instance.ShowNotification($"{unitInfo_Immutable.Name}�� ���ŷ� �Ʒ��� ���������� �������ϴ�.");
                    break;
                case EUnitScheduleType.Traning_Trait:
                    NotificationManager.Instance.ShowNotification($"{unitInfo_Immutable.Name}�� Ư�� �Ʒ��� ���������� �������ϴ�.");
                    break;
            }
        }

        if(PlayerManager.Instance.gameSchedule.HaveSchedule(schedule) == false)
        {
            unitCondition.RecoverCondition();
        }   
    }

    public void AddPopulation(int addPopulation)
    {
        var trait = GetTrait(TraitType.Showmaker);

        if(trait != null)
        {
            addPopulation = (int)(addPopulation * (trait.Value1 * 0.01f));
        }

        Population += addPopulation;
    }

    public DT_Trait GetTrait(string traitType)
    {
        return traitList.Find(x => x.Type == traitType);
    }
}


public class Unitcondition
{
    public int CurrentContdition;
    public DT_Condition dt_Condition;
    public EUnitTier eUnitTier;

    public int Professionalism; // ������
    public int Ambition; // �߸�
    public int Injury_Proneness; // �λ� ����
    public int Consistency; // �ϰ���
    public int Teamwork; // ����ũ
    public int Preparation; // �غ�
    public int Diligence; // �ٸ鼺
    public int Royalty; // �漺��

    public Unitcondition(EUnitTier _eUnitTier)
    {
        eUnitTier = _eUnitTier;

        var tierInfo = DT_UnitTierInfo.GetInfoByIndex(eUnitTier);
        int min = tierInfo.ConditionStatMin;
        int max = tierInfo.ConditionStatMax + 1;

        Professionalism = UnityEngine.Random.Range(min, max);
        Ambition = UnityEngine.Random.Range(min, max);
        Injury_Proneness = UnityEngine.Random.Range(min, max);
        Consistency = UnityEngine.Random.Range(min, max);
        Teamwork = UnityEngine.Random.Range(min, max);
        Preparation = UnityEngine.Random.Range(min, max);
        Diligence = UnityEngine.Random.Range(min, max);
        Royalty = UnityEngine.Random.Range(min, max);

        CurrentContdition = UnityEngine.Random.Range(DT_Const.GetInfoByIndex("CONDITION_START_MIN"), DT_Const.GetInfoByIndex("CONDITION_START_MAX") + 1);
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
            case EUnitTier.Challenger:
                addPoint += 50;
                break;
            case EUnitTier.Master:
                addPoint += 30;
                break;
            case EUnitTier.Gold:
                addPoint += 25;
                break;
            case EUnitTier.Silver:
                addPoint += 20;
                break;
            case EUnitTier.Bronze:
                addPoint += 10;
                break;
            case EUnitTier.Iron:
                addPoint += 0;
                break;
        }
    }

    public int GetConditionValue()
    {
        var totalValue = (Professionalism + Ambition + Injury_Proneness + Consistency + Teamwork + Preparation + Diligence + Royalty);
        totalValue /= 3;
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

    public void RecoverCondition()
    {
        CurrentContdition += DT_Const.GetInfoByIndex("RECOVER_CONDITION");
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
