using System;
using System.Collections.Generic;
using UnityEngine;

public enum EScheduleType
{
    None,
    Contest,
    Scream,
    Training,
    InternationalActivity,
}

public class Schedule
{
    public int Year { get; private set; }
    public int Month { get; private set; }
    public int Day { get; private set; }
    public EScheduleType Type { get; private set; }
    public string Description { get; private set; }

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
    public void AddSchedule(int year, int month, int day, EScheduleType type, string description)
    {
        string dateKey = new DateTime(year, month, day).ToString("yyyy-MM-dd");

        if (monthlyCalendar.ContainsKey(dateKey))
        {
            monthlyCalendar[dateKey] = new Schedule(year, month, day, type, description);
        }
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
    }

    public int GetPlayDay()
    {
        return (CurrentDate - StartDate).Days;
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
