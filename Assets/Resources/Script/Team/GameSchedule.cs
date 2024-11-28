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

    // 연도와 월을 입력받아 달력을 생성합니다.
    public void GenerateMonthlyCalendar(int year, int month)
    {
        int daysInMonth = DateTime.DaysInMonth(year, month);

        // 월의 첫날부터 마지막 날까지 각 날짜에 대해 초기화
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

    // 특정 날짜에 스케줄을 추가하는 메서드
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
            Panel_ToastMessage.OpenToast("이미 일정이 있습니다.", false);
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
            Panel_ToastMessage.OpenToast("오늘은 일정을 소화했습니다.", false);
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

    // 특정 날짜로 이동
    public void AdvanceDay()
    {
        CurrentDate = CurrentDate.AddDays(1); // 다음날로 이동
        FrontInfoCanvas.Instance.SetDateText(CurrentDate);

        // 유닛 스케줄 체크
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

    // JSON으로 저장하기 위한 메서드
    public string ToJson()
    {
        return JsonUtility.ToJson(new SerializableGameScheduleWrapper(this), true);
    }

    // JSON에서 불러오기 위한 메서드
    public void FromJson(string json)
    {
        var wrapper = JsonUtility.FromJson<SerializableGameScheduleWrapper>(json);
        CurrentDate = DateTime.Parse(wrapper.CurrentDate); // DateTime 복원
        StartDate = DateTime.Parse(wrapper.StartDate); // DateTime 복원
        monthlyCalendar = wrapper.ToDictionary();
    }
}

// JSON 직렬화를 위한 래퍼 클래스
[Serializable]
public class SerializableGameScheduleWrapper
{
    public string CurrentDate; // DateTime을 string으로 저장
    public string StartDate; // DateTime을 string으로 저장
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
