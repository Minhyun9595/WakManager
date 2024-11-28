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

        var panel = PanelRenderQueueManager.Instance;
        var poolManager = PoolManager.Instance;
        var playerManager = PlayerManager.Instance;
        var customTime = CustomTime.Instance;
    }

    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.Alpha1) && SceneManager.GetActiveScene().buildIndex != (int)ESceneType.Menu)
        //{
        //    PlayerManager.Instance.SetSceneChangeType(SceneChangeType.MoveWorld);
        //    SceneManager.LoadScene(0);
        //}
        //else if (Input.GetKeyUp(KeyCode.Alpha2) && SceneManager.GetActiveScene().buildIndex != (int)ESceneType.Lobby)
        //{
        //    PlayerManager.Instance.SetSceneChangeType(SceneChangeType.MoveWorld);
        //    SceneManager.LoadScene(1);
        //}
        //else if (Input.GetKeyUp(KeyCode.Alpha3) && SceneManager.GetActiveScene().buildIndex != (int)ESceneType.InGame)
        //{
        //    PlayerManager.Instance.SetSceneChangeType(SceneChangeType.MoveWorld);
        //    SceneManager.LoadScene(2);
        //}
        if (Input.GetKeyUp(KeyCode.Alpha2) && SceneManager.GetActiveScene().buildIndex == (int)ESceneType.Lobby)
        {
            PlayerManager.Instance.gameSchedule.AdvanceDay();
        }
    }
}
