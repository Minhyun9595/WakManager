using QUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridItem_SaveData : GridAbstract, GridInterface
{
    private int index = -1;
    public Button FocusButton;
    public TextMeshProUGUI IndexText;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI SaveText;

    public override void Init(GameObject _gameObject)
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
            GameSchedule gameSchedule = new GameSchedule(1, 1);
            gameSchedule.FromJson(saveData.gameScheduleData);

            NameText.text = $"{saveData.saveName} - {gameSchedule.GetPlayDay()}일차";
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
    public Button ExitToMenuButton;
    void Start()
    {
        Content = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "Content");
        SaveButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "SaveButton");
        LoadButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "LoadButton");
        ExitButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "ExitButton");
        ExitToMenuButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "ExitToMenuButton");

        ExitButton.onClick.AddListener(OnClick_Close);
        SaveButton.onClick.AddListener(OnClick_SaveButton);
        LoadButton.onClick.AddListener(OnClick_LoadButton);
        ExitToMenuButton.onClick.AddListener(OnClick_ToMenuButton);

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

        bool isNotMenuScene = PlayerManager.Instance.GetSceneType() != ESceneType.Menu;
        bool isLobbyScene = PlayerManager.Instance.GetSceneType() == ESceneType.Lobby;
        ExitToMenuButton.gameObject.SetActive(isNotMenuScene);
        SaveButton.gameObject.SetActive(isLobbyScene);
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

        var result = PlayerManager.Instance.Load(focusIndex);

        if(result)
        {
            PlayerManager.Instance.SetSceneChangeType(SceneChangeType.LoadWorld);
            SceneManager.LoadScene((int)ESceneType.Lobby);
        }
    }

    void OnClick_ToMenuButton()
    {
        PlayerManager.Instance.SetSceneChangeType(SceneChangeType.MoveWorld);
        SceneManager.LoadScene((int)ESceneType.Menu);
    }

    void OnClick_Close()
    {
        PanelRenderQueueManager.Instance.ClosePanel(this);
    }
}
