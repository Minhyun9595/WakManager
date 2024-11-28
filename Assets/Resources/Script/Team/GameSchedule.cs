using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum EScheduleType
{
    None,
    Contest,
    Scream,
    Training,
    InternationalActivity,
    Search_Market,
    ContractUnit_Market,
    Activity_SoloStream,
    Activity_TeamStream,
}

public enum EUnitScheduleType
{
    Traning_Health,
    Traning_Damage,
    Traning_Armor,
    Traning_Mental,
    Traning_Trait,
}

[System.Serializable]
public class ScheduleDate
{
    public EUnitScheduleType EUnitScheduleType;
    public int Year;
    public int Month;
    public int Day;

    public ScheduleDate(EUnitScheduleType eUnitScheduleType,  int year, int month, int day)
    {
        EUnitScheduleType = eUnitScheduleType;
        Year = year;
        Month = month;
        Day = day;
    }
}

[System.Serializable]
public class Schedule
{
    public int Year;
    public int Month;
    public int Day;
    public EScheduleType Type;
    public string Description;

    public Schedule(int year, int month, int day, EScheduleType type, string description)
    {
        Year = year;
        Month = month;
        Day = day;
        Type = type;
        Description = description;
    }
}

public class GameSchedule
{
    public DateTime StartDate { get; private set; }
    public DateTime CurrentDate { get; private set; }

    public Dictionary<string, Schedule> monthlyCalendar;

    public GameSchedule(int year, int month)
    {
        StartDate = new DateTime(year, month, 1);
        CurrentDate = new DateTime(year, month, 1);
        monthlyCalendar = new Dictionary<string, Schedule>();
        GenerateMonthlyCalendar(CurrentDate.Year, CurrentDate.Month);
    }

    // ������ ���� �Է¹޾� �޷��� �����մϴ�.
    public void GenerateMonthlyCalendar(int year, int month)
    {
        int daysInMonth = DateTime.DaysInMonth(year, month);

        // ���� ù������ ������ ������ �� ��¥�� ���� �ʱ�ȭ
        for (int day = 1; day <= daysInMonth; day++)
        {
            string dateKey = new DateTime(year, month, day).ToString("yyyy-MM-dd");
            if (!monthlyCalendar.ContainsKey(dateKey))
            {
                // Default schedule with None type for empty days
                monthlyCalendar[dateKey] = new Schedule(year, month, day, EScheduleType.None, "No scheduled activity");
            }
        }
    }

    // Ư�� ��¥�� �������� �߰��ϴ� �޼���
    public bool AddSchedule(int year, int month, int day, EScheduleType type, string description)
    {
        string dateKey = new DateTime(year, month, day).ToString("yyyy-MM-dd");

        if (monthlyCalendar.ContainsKey(dateKey))
        {
            monthlyCalendar[dateKey] = new Schedule(year, month, day, type, description);
            return true;
        }
        else
        {
            Panel_ToastMessage.OpenToast("�̹� ������ �ֽ��ϴ�.", false);
        }

        return false;
    }

    public bool AddScheduleToday(EScheduleType type, string description)
    {
        string dateKey = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day).ToString("yyyy-MM-dd");

        if (monthlyCalendar.ContainsKey(dateKey))
        {
            monthlyCalendar[dateKey] = new Schedule(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, type, description);
            return true;
        }
        else
        {
            Panel_ToastMessage.OpenToast("������ ������ ��ȭ�߽��ϴ�.", false);
        }
        return false;
    }

    public List<Schedule> GetMonthlySchedules(int year, int month)
    {
        GenerateMonthlyCalendar(year, month);

        List<Schedule> monthlySchedules = new List<Schedule>();
        int daysInMonth = DateTime.DaysInMonth(year, month);

        for (int day = 1; day <= daysInMonth; day++)
        {
            string dateKey = new DateTime(year, month, day).ToString("yyyy-MM-dd");

            if (monthlyCalendar.ContainsKey(dateKey))
            {
                monthlySchedules.Add(monthlyCalendar[dateKey]);
            }
        }

        return monthlySchedules;
    }

    // Ư�� ��¥�� �̵�
    public void AdvanceDay()
    {
        CurrentDate = CurrentDate.AddDays(1); // �������� �̵�
        FrontInfoCanvas.Instance.SetDateText(CurrentDate);

        // ���� ������ üũ
        var playerUnits = PlayerManager.Instance.PlayerTeamInfo.GetPlayer_SquadUnitDatas();
        foreach(var unit in playerUnits)
        {
            unit.ScheduleCheck();
        }
    }

    public int GetPlayDay()
    {
        return (CurrentDate - StartDate).Days;
    }

    public string GetToday()
    {
        return CurrentDate.ToString("yyyy-MM-dd");
    }

    public bool IsToday(ScheduleDate scheduleDate)
    {
        if (scheduleDate == null)
            return false;

        if(scheduleDate.Year == CurrentDate.Year && scheduleDate.Month == CurrentDate.Month && scheduleDate.Day == CurrentDate.Day)
        {
            return true;
        }

        return false;
    }

    // JSON���� �����ϱ� ���� �޼���
    public string ToJson()
    {
        return JsonUtility.ToJson(new SerializableGameScheduleWrapper(this), true);
    }

    // JSON���� �ҷ����� ���� �޼���
    public void FromJson(string json)
    {
        var wrapper = JsonUtility.FromJson<SerializableGameScheduleWrapper>(json);
        CurrentDate = DateTime.Parse(wrapper.CurrentDate); // DateTime ����
        StartDate = DateTime.Parse(wrapper.StartDate); // DateTime ����
        monthlyCalendar = wrapper.ToDictionary();
    }
}

// JSON ����ȭ�� ���� ���� Ŭ����
[Serializable]
public class SerializableGameScheduleWrapper
{
    public string CurrentDate; // DateTime�� string���� ����
    public string StartDate; // DateTime�� string���� ����
    public List<CalendarDayEntry> Entries;

    public SerializableGameScheduleWrapper(GameSchedule gameSchedule)
    {
        CurrentDate = gameSchedule.CurrentDate.ToString("yyyy-MM-dd");
        StartDate = gameSchedule.StartDate.ToString("yyyy-MM-dd");
        Entries = new List<CalendarDayEntry>();

        foreach (var kvp in gameSchedule.monthlyCalendar)
        {
            Entries.Add(new CalendarDayEntry(kvp.Key, kvp.Value));
        }
    }

    public Dictionary<string, Schedule> ToDictionary()
    {
        var dictionary = new Dictionary<string, Schedule>();
        foreach (var entry in Entries)
        {
            dictionary[entry.Date] = entry.Schedule;
        }
        return dictionary;
    }
}

[Serializable]
public class CalendarDayEntry
{
    public string Date;
    public Schedule Schedule;

    public CalendarDayEntry(string date, Schedule schedule)
    {
        Date = date;
        Schedule = schedule;
    }
}
