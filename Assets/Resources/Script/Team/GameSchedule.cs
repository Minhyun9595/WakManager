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

    // ������ ���� �Է¹޾� �޷��� �����մϴ�.
    public void GenerateMonthlyCalendar(int year, int month)
    {
        int daysInMonth = DateTime.DaysInMonth(year, month);

        // ���� ù������ ������ ������ �� ��¥�� ���� �ʱ�ȭ
        for (int day = 1; day <= daysInMonth; day++)
        {
            string dateKey = new DateTime(year, month, day).ToString("yyyy-MM-dd");
            monthlyCalendar[dateKey] = new List<Schedule>(); // �������� ������ ����Ʈ �ʱ�ȭ
        }
    }

    // Ư�� ��¥�� �������� �߰��ϴ� �޼���
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

    // JSON���� �����ϱ� ���� �޼���
    public string ToJson()
    {
        return JsonUtility.ToJson(new SerializableDictionaryWrapper(monthlyCalendar), true);
    }

    // JSON���� �ҷ����� ���� �޼���
    public void FromJson(string json)
    {
        var wrapper = JsonUtility.FromJson<SerializableDictionaryWrapper>(json);
        monthlyCalendar = wrapper.ToDictionary();
    }
}


// JSON ����ȭ�� ���� ���� Ŭ����
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