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
    public Outline outline;
    public Image image;
    public TextMeshProUGUI DayText;
    public TextMeshProUGUI ScheduleText;

    public new void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);
        image = gameObject.GetComponent<Image>();
        outline = gameObject.GetComponent<Outline>();
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
        outline.enabled = false;
        DayText.text = _schedule.Day.ToString();

        switch (_schedule.Type)
        {
            case EScheduleType.Contest:
                image.color = Color.cyan;
                ScheduleText.text = "��ȸ";
                break;
            case EScheduleType.Scream:
                image.color = Color.cyan;
                ScheduleText.text = "��ũ��";
                break;
            case EScheduleType.Training:
                image.color = Color.cyan;
                ScheduleText.text = "�Ʒ�";
                break;
            case EScheduleType.InternationalActivity:
                image.color = Color.cyan;
                ScheduleText.text = "��� Ȱ��";
                break;
            default:
                image.color = UIUtility.HexToColor("00AB40");
                break;
        }
    }

    public void SetToday()
    {
        outline.enabled = true;
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
    public int currentDay = -1;

    private void Awake()
    {
        currentShowYear = PlayerManager.Instance.gameSchedule.CurrentDate.Year;
        currentShowMonth = PlayerManager.Instance.gameSchedule.CurrentDate.Month;
        currentDay = PlayerManager.Instance.gameSchedule.CurrentDate.Day;
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
    }

    void Start()
    {
        PanelUpdate();
    }

    public override void Open()
    {
        base.Open();
        FrontInfoCanvas.Instance.SetPanelName("������");
        currentShowYear = PlayerManager.Instance.gameSchedule.CurrentDate.Year;
        currentShowMonth = PlayerManager.Instance.gameSchedule.CurrentDate.Month;
        currentDay = PlayerManager.Instance.gameSchedule.CurrentDate.Day;
        PanelUpdate();
    }

    void PanelUpdate()
    {
        CurrentMonthText.text = $"{currentShowYear}�� {currentShowMonth}��";
        var monthlyScheduleList = PlayerManager.Instance.gameSchedule.GetMonthlySchedules(currentShowYear, currentShowMonth);

        DateTime firstDay = new DateTime(currentShowYear, currentShowMonth, 1);
        int startOffset = (int)firstDay.DayOfWeek;

        DateTime today = PlayerManager.Instance.gameSchedule.CurrentDate;

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
                gridList[gridIndex].gameObject.GetComponent<Outline>().enabled = false;

                // ���� ��¥�� ���� ���̸� �÷� ����
                if (currentShowYear == today.Year && currentShowMonth == today.Month && day == today.Day)
                {
                    gridList[gridIndex].SetToday();
                }
            }
        }
    }

    void OnClick_MoveMonthButton(int direction)
    {
            var beforeYear = currentShowYear;
        var beforeMonth = currentShowMonth;

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

        if(currentShowYear < PlayerManager.Instance.gameSchedule.StartDate.Year)
        {
            // ���ŷ� ����.
            currentShowYear = beforeYear;
            currentShowMonth = beforeMonth;
            return;
        }

        PanelUpdate();
    }
}
