using QUtility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Panel_MainMenu : PanelAbstract
{
    public Button NewGameButton;
    public Button LoadPanelButton;
    public Button NewGameStartButton;
    public Transform TeamNameInputField;
    public TextMeshProUGUI TeamNameInputText;

    void Start()
    {
        NewGameButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "NewGameButton");
        LoadPanelButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "LoadPanelButton");
        NewGameStartButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "NewGameStartButton");
        TeamNameInputField = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "TeamNameInputField");
        TeamNameInputText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TeamNameInputText");

        NewGameButton.onClick.RemoveAllListeners();
        LoadPanelButton.onClick.RemoveAllListeners();

        NewGameButton.onClick.AddListener(OnClick_NewGame);
        LoadPanelButton.onClick.AddListener(OnClick_LoadPanel);
        NewGameStartButton.onClick.AddListener(OnClick_NewGameStart);

        NewGameStartButton.gameObject.SetActive(false);
        TeamNameInputField.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    void OnClick_NewGame()
    {
        var before = NewGameStartButton.gameObject.activeSelf;

        NewGameStartButton.gameObject.SetActive(!before);
        TeamNameInputField.gameObject.SetActive(!before);
    }

    void OnClick_LoadPanel()
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_SaveData);
    }

    void OnClick_NewGameStart()
    {
        if(TeamNameInputText.text != string.Empty)
        {
            PlayerManager.Instance.SetNewGame(TeamNameInputText.text);
            SceneManager.LoadScene((int)ESceneType.Lobby);
        }
    }
}
