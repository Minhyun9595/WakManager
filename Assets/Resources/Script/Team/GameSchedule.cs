using System;
using System.Collections;
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
    public EScheduleType Type { get; private set; }
    public string Description { get; private set; }

    public Schedule(EScheduleType type, string description)
    {
        Type = type;
        Description = description;
    }
}

public class GameSchedule
{
    private Dictionary<string, List<Schedule>> monthlyCalendar;

    public GameSchedule()
    {
        monthlyCalendar = new Dictionary<string, List<Schedule>>();
    }

    // 연도와 월을 입력받아 달력을 생성합니다.
    public void GenerateMonthlyCalendar(int year, int month)
    {
        int daysInMonth = DateTime.DaysInMonth(year, month);

        // 월의 첫날부터 마지막 날까지 각 날짜에 대해 초기화
        for (int day = 1; day <= daysInMonth; day++)
        {
            string dateKey = new DateTime(year, month, day).ToString("yyyy-MM-dd");
            monthlyCalendar[dateKey] = new List<Schedule>(); // 스케줄을 저장할 리스트 초기화
        }
    }

    // 특정 날짜에 스케줄을 추가하는 메서드
    public void AddSchedule(int year, int month, int day, EScheduleType type, string description)
    {
        string dateKey = new DateTime(year, month, day).ToString("yyyy-MM-dd");

        if (monthlyCalendar.ContainsKey(dateKey))
        {
            monthlyCalendar[dateKey].Add(new Schedule(type, description));
        }
        else
        {
            Debug.LogWarning("The date is not in the current month's calendar. Generate the calendar first.");
        }
    }

    // JSON으로 저장하기 위한 메서드
    public string ToJson()
    {
        return JsonUtility.ToJson(new SerializableDictionaryWrapper(monthlyCalendar), true);
    }

    // JSON에서 불러오기 위한 메서드
    public void FromJson(string json)
    {
        var wrapper = JsonUtility.FromJson<SerializableDictionaryWrapper>(json);
        monthlyCalendar = wrapper.ToDictionary();
    }
}


// JSON 직렬화를 위한 래퍼 클래스
[Serializable]
public class SerializableDictionaryWrapper
{
    public List<CalendarDayEntry> Entries;

    public SerializableDictionaryWrapper(Dictionary<string, List<Schedule>> dictionary)
    {
        Entries = new List<CalendarDayEntry>();
        foreach (var kvp in dictionary)
        {
            Entries.Add(new CalendarDayEntry(kvp.Key, kvp.Value));
        }
    }

    public Dictionary<string, List<Schedule>> ToDictionary()
    {
        var dictionary = new Dictionary<string, List<Schedule>>();
        foreach (var entry in Entries)
        {
            dictionary[entry.Date] = entry.Schedules;
        }
        return dictionary;
    }
}

[Serializable]
public class CalendarDayEntry
{
    public string Date;
    public List<Schedule> Schedules;

    public CalendarDayEntry(string date, List<Schedule> schedules)
    {
        Date = date;
        Schedules = schedules;
    }
}