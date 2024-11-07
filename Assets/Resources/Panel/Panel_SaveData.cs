using QUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridItem_SaveData : GridAbstract, GridInterface
{
    private int index = -1;
    public Button FocusButton;
    public TextMeshProUGUI IndexText;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI SaveText;

    public new void Init(GameObject _gameObject)
    {
        base.Init(_gameObject);
        FocusButton = gameObject.GetComponent<Button>();
        IndexText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "IndexText");
        NameText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "NameText");
        TimeText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TimeText");
        SaveText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "SaveText");
    }

    public void InitIndex(int _index)
    {
        index = _index;
        SaveText.text = $"{_index + 1}슬롯 저장하기";
    }

    public void Update(SaveData saveData)
    {
        IndexText.gameObject.SetActive(false);
        TimeText.gameObject.SetActive(false);
        NameText.gameObject.SetActive(false);
        SaveText.gameObject.SetActive(false);

        if (saveData == null)
        {
            SaveText.gameObject.SetActive(true);
        }
        else
        {
            NameText.text = saveData.saveName;
            TimeText.text = saveData.saveTime.ToString();
            IndexText.gameObject.SetActive(true);
            TimeText.gameObject.SetActive(true);
            NameText.gameObject.SetActive(true);
        }
    }
}


public class Panel_SaveData : PanelAbstract
{
    private const int SlotCount = 10;
    private int focusIndex = -1;
    public Transform Content;
    [SerializeField] private List<GridItem_SaveData> gridList = new List<GridItem_SaveData>();

    public Button SaveButton;
    public Button LoadButton;
    public Button ExitButton;
    void Start()
    {
        Content = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Content");
        SaveButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "SaveButton");
        LoadButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "LoadButton");
        ExitButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "ExitButton");

        ExitButton.onClick.AddListener(Close);
        SaveButton.onClick.AddListener(OnClick_SaveButton);
        LoadButton.onClick.AddListener(OnClick_LoadButton);

        for (int i = 0; i < SlotCount; i++)
        {
            var childItem = QUtility.UIUtility.GetChildAutoCraete(Content, i);
            var gridItem_SaveData = new GridItem_SaveData();
            gridItem_SaveData.Init(childItem);
            gridItem_SaveData.InitIndex(i);

            int index = i;
            gridItem_SaveData.FocusButton.onClick.AddListener(() => OnClick_GridButton(index));
            gridList.Add(gridItem_SaveData);
        }

        Reload();
    }

    void Reload()
    {
        // 데이터 없으면 저장하기 활성화 나머지 끄기
        // 0번은 자동저장 슬롯

        for (int i = 0; i < SlotCount; i++)
        {
            var saveData = PlayerManager.Instance.LoadData(i);
            var gridItem = gridList[i];

            gridItem.Update(saveData);
        }
    }

    public void OnClick_GridButton(int _index)
    {
        focusIndex = _index;
        Debug.Log(focusIndex);
    }

    void OnClick_SaveButton()
    {
        if(focusIndex < 0 || SlotCount <= focusIndex)
        {
            Debug.Log("focusIndex 범위 밖");
            return;
        }

        PlayerManager.Instance.SaveData(focusIndex);

        Reload();
    }

    void OnClick_LoadButton()
    {
        if (focusIndex < 0 || SlotCount <= focusIndex)
        {
            Debug.Log("focusIndex 범위 밖");
            return;
        }

        PlayerManager.Instance.LoadData(focusIndex);
    }
}
