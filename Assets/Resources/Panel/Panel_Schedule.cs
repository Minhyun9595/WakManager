using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QUtility;
using System;
using System.Reflection;

public class GridItem_Day : GridAbstract, GridInterface
{
    public Image image;
    public TextMeshProUGUI DayText;
    public TextMeshProUGUI ScheduleText;

    public new void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);
        image = gameObject.GetComponent<Image>();
        DayText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "DayText");
        ScheduleText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "ScheduleText");
        DayText.text = "";
        ScheduleText.text = "";
    }

    public void SetColorAlpha(bool state)
    {
        image.enabled = state;
        DayText.enabled = state;
        ScheduleText.enabled = state;
    }

    public void Set(Schedule _schedule)
    {
        SetColorAlpha(true);
        DayText.text = _schedule.Day.ToString();

        switch (_schedule.Type)
        {
            case EScheduleType.Contest:
                image.color = Color.cyan;
                ScheduleText.text = "대회";
                break;
            case EScheduleType.Scream:
                image.color = Color.cyan;
                ScheduleText.text = "스크림";
                break;
            case EScheduleType.Training:
                image.color = Color.cyan;
                ScheduleText.text = "훈련";
                break;
            case EScheduleType.InternationalActivity:
                image.color = Color.cyan;
                ScheduleText.text = "대외 활동";
                break;
            default:
                image.color = UIUtility.HexToColor("00AB40");
                break;
        }
    }
}

public class Panel_Schedule : PanelAbstract
{
    public TextMeshProUGUI CurrentMonthText;
    public Button BeforeButton;
    public Button AfterButton;
    public Transform CalanderGrid;
    public List<GridItem_Day> gridList;

    public int currentShowYear = -1;
    public int currentShowMonth = -1;
    void Start()
    {
        currentShowYear = PlayerManager.Instance.gameSchedule.CurrentYear;
        currentShowMonth = PlayerManager.Instance.gameSchedule.CurrentMonth;

        CurrentMonthText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "CurrentMonthText");
        BeforeButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "BeforeButton");
        AfterButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "AfterButton");
        CalanderGrid = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "CalanderGrid");
        var GridItem_Day = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "GridItem_Day");

        gridList = new List<GridItem_Day>();
        for (int i = 0; i < 35 - 1; i++)
        {
            var childItem = UIUtility.GetChildAutoCraete(CalanderGrid, i);
            var gridItem_Day = new GridItem_Day();
            gridItem_Day.Init(childItem);

            gridList.Add(gridItem_Day);
        }

        BeforeButton.onClick.AddListener(() => OnClick_MoveMonthButton(-1));
        AfterButton.onClick.AddListener(() => OnClick_MoveMonthButton(1));
        PanelUpdate();
    }

    void PanelUpdate()
    {
        CurrentMonthText.text = $"{currentShowYear}년 {currentShowMonth}월";
        var monthlyScheduleList = PlayerManager.Instance.gameSchedule.GetMonthlySchedules(currentShowYear, currentShowMonth);

        DateTime firstDay = new DateTime(currentShowYear, currentShowMonth, 1);
        int startOffset = (int)firstDay.DayOfWeek;

        for (int i = 0; i < gridList.Count; i++)
        {
            gridList[i].SetColorAlpha(false);
        }

        for (int day = 1; day <= DateTime.DaysInMonth(currentShowYear, currentShowMonth); day++)
        {
            int gridIndex = startOffset + day - 1;

            if (gridIndex >= 0 && gridIndex < gridList.Count)
            {
                Schedule schedule = monthlyScheduleList[day - 1];
                gridList[gridIndex].Set(schedule);
            }
        }
    }

    void OnClick_MoveMonthButton(int direction)
    {
        currentShowMonth += direction;

        if (13 <= currentShowMonth)
        {
            currentShowYear += 1;
            currentShowMonth = 1;
        }
        else if(currentShowMonth <= 0)
        {
            currentShowYear -= 1;
            currentShowMonth = 12;
        }

        PanelUpdate();
    }
}
