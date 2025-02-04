using QUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridItem_Record : GridAbstract
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

        gridItem_Records = new List<GridItem_Record>();
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

        // 부족한 경우에만 생성
        for (int i = gridItem_Records.Count; i < notifications.Count; i++)
        {
            var childItem = UIUtility.GetChildAutoCraete(Grid_Record, i);
            childItem.SetActive(true);

            var gridItem_Record = new GridItem_Record();
            gridItem_Record.Init(childItem);

            gridItem_Records.Add(gridItem_Record);
        }

        // 데이터를 세팅
        for (int i = 0; i < notifications.Count; i++)
        {
            int reverseIndex = notifications.Count - 1 - i; // 뒤에서부터 인덱스 계산
            gridItem_Records[i].SetRecord(notifications[reverseIndex]);
            gridItem_Records[i].gameObject.SetActive(true);
        }

        // 초과된 아이템 비활성화
        for (int i = notifications.Count; i < Grid_Record.childCount; i++)
        {
            Grid_Record.GetChild(i).gameObject.SetActive(false);
        }
    }
}
