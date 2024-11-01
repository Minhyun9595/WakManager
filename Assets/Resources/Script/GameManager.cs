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
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1) && SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2) && SceneManager.GetActiveScene().buildIndex != 1)
        {
            SceneManager.LoadScene(1);
        }
    }
}
