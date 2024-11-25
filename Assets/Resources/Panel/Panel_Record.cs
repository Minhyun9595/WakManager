using QUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridItem_Record : GridAbstract, GridInterface
{
    public TextMeshProUGUI DateText;
    public TextMeshProUGUI Text;

    public override void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);
        DateText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "DateText");
        Text = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "Text");
    }

    public void SetRecord(Notification notification)
    {
        DateText.text = notification.date;
        Text.text = notification.desc;
    }
}

public class Panel_Record : PanelAbstract
{
    public Transform Grid_Record;

    public List<GridItem_Record> gridItem_Records;

    void Awake()
    {
        Grid_Record = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Grid_Record");

    }

    public override void Open()
    {
        base.Open();
        FrontInfoCanvas.Instance.SetPanelName("기록");
        PanelUpdate();
    }

    public void PanelUpdate()
    {
        var notifications = PlayerManager.Instance.notifications;

        gridItem_Records = new List<GridItem_Record>();
        for (int i = 0; i < notifications.Count; i++)
        {
            var childItem = UIUtility.GetChildAutoCraete(Grid_Record, i);
            childItem.SetActive(true);

            var gridItem_Record = new GridItem_Record();
            gridItem_Record.Init(childItem);
            gridItem_Record.SetRecord(notifications[i]);

            gridItem_Records.Add(gridItem_Record);
        }

        // 남은 슬롯 비활성화 처리
        for (int i = notifications.Count; i < Grid_Record.childCount; i++)
        {
            Grid_Record.GetChild(i).gameObject.SetActive(false);
        }
    }
}
