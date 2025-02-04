using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        DataTable.Instance.Initialize();

        AwakeSingleton();
    }

    private void AwakeSingleton()
    {
        var panel = PanelRenderQueueManager.Instance;
        var poolManager = PoolManager.Instance;
        var playerManager = PlayerManager.Instance;
        var customTime = CustomTime.Instance;
    }

    void Update()
    {
        // Cheat
        if (Input.GetKeyUp(KeyCode.Alpha2) && SceneManager.GetActiveScene().buildIndex == (int)ESceneType.Lobby)
        {
            PlayerManager.Instance.gameSchedule.AdvanceDay();
        }
    }
}
