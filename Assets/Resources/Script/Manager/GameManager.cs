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

        if (SceneManager.GetActiveScene().buildIndex == (int)ESceneType.Lobby)
        {
            CreateNewCards(10);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1) && SceneManager.GetActiveScene().buildIndex != (int)ESceneType.Menu)
        {
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2) && SceneManager.GetActiveScene().buildIndex != (int)ESceneType.Lobby)
        {
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3) && SceneManager.GetActiveScene().buildIndex != (int)ESceneType.InGame)
        {
            SceneManager.LoadScene(2);
        }

    }

    [SerializeField] private List<UnitData> createUnitDataList = new List<UnitData>();
    public void CreateNewCards(int _createCount)
    {
        Debug.Log("CreateNewCards");
        List<UnitData> cards = new List<UnitData>();
        for (int i = 0; i < _createCount; i++)
        {
            var keys = DT_UnitInfo_Immutable.GetKeys();
            var randUnitIndex = Random.Range(0, keys.Count);
            var unitIndex = keys[randUnitIndex];

            var unitData = UnitData.CreateNewUnit(unitIndex);

            cards.Add(unitData);
        }
        createUnitDataList.Clear();
        createUnitDataList = cards;
    }
}
