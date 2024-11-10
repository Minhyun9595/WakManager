using QUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Panel_MainMenu : PanelAbstract
{
    public Button NewGameButton;
    public Button LoadPanelButton;

    void Start()
    {
        NewGameButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "NewGameButton");
        LoadPanelButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "LoadPanelButton");

        NewGameButton.onClick.RemoveAllListeners();
        LoadPanelButton.onClick.RemoveAllListeners();

        NewGameButton.onClick.AddListener(OnClick_NewGame);
        LoadPanelButton.onClick.AddListener(OnClick_LoadPanel);
    }

    void Update()
    {
        
    }

    void OnClick_NewGame()
    {
        PlayerManager.Instance.SetNewGame();
        SceneManager.LoadScene(1);
    }

    void OnClick_LoadPanel()
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_SaveData);
    }
}
