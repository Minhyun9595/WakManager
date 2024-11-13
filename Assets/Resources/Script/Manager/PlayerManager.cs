using QUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public string saveName;
    public string saveTime;
    public float playTime;

    public List<UnitData> worldUnitDatas = new List<UnitData>();
    public List<UnitData> market_UnitCardDatas = new List<UnitData>();
    public List<UnitData> player_Squad_UnitCardDatas = new List<UnitData>();
    public List<UnitData> player_InSquad_UnitCardDatas = new List<UnitData>();
}

public enum SceneChangeType
{
    None,
    NewWorld,
    LoadWorld,
    MoveWorld,
}

public class PlayerManager : CustomSingleton<PlayerManager>
{
    new void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        Load_NewWorld();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private ESceneType sceneType;
    private SceneChangeType sceneChangeType = SceneChangeType.None;
    public string saveName;
    private string saveFolderPath;
    private SaveData loadData = null;
    private float playTime;
    [SerializeField] private List<UnitData> worldUnitCardDatas = new List<UnitData>();
    [SerializeField] private List<UnitData> market_UnitCardDatas = new List<UnitData>();
    [SerializeField] private List<UnitData> player_Squad_UnitCardDatas = new List<UnitData>();
    [SerializeField] private List<UnitData> player_InSquad_UnitCardDatas = new List<UnitData>();

    void Start()
    {
        // 사용자 문서 폴더 경로 얻기
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // 프로젝트 이름과 세이브 데이터 폴더 설정
        saveFolderPath = Path.Combine(documentsPath, "WM2025", "SaveData");

        // 폴더가 없는 경우 생성
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
    }

    void Update()
    {
        playTime += Time.deltaTime;
    }

    public void SaveData(int index)
    {
        SaveData saveData = new SaveData();
        saveData.saveName = $"슬롯_{index + 1}";
        saveData.saveTime = DateTime.Now.ToString("yyyy년-MM월-dd일 HH:mm");
        saveData.playTime = playTime;
        saveData.worldUnitDatas = worldUnitCardDatas;
        saveData.market_UnitCardDatas = market_UnitCardDatas;
        saveData.player_Squad_UnitCardDatas = player_Squad_UnitCardDatas;
        saveData.player_InSquad_UnitCardDatas = player_InSquad_UnitCardDatas;

        var filePath = Path.Combine(saveFolderPath, $"saveData_{index}.json");
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, json);
    }

    public bool Load(int _index)
    {
        SaveData saveData = LoadData(_index);

        if(saveData == null)
        {
            return false;
        }

        loadData = saveData;
        return true;
    }

    public SaveData LoadData(int _index)
    {
        var filePath = Path.Combine(saveFolderPath, $"saveData_{_index}.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data;
        }
        else
        {
            return null;
        }
    }

    public void SetNewGame()
    {
        SetSceneChangeType(SceneChangeType.NewWorld);
        loadData = null;
    }

    public void SetSceneChangeType(SceneChangeType _sceneChangeType)
    {
        sceneChangeType = _sceneChangeType;
    }

    public SceneChangeType GetSceneChangeType()
    {
        return sceneChangeType;
    }



    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"OnSceneLoaded {Instance.GetSceneChangeType().ToString()}");

        switch (Instance.GetSceneChangeType())
        {
            case SceneChangeType.NewWorld:
                Load_NewWorld();
                break;
            case SceneChangeType.LoadWorld:
                Load_OldWorld();
                break;
            case SceneChangeType.MoveWorld:
                // 이동만 했을 경우 아무것도 하지 않는다.
                break;
            default:
                break;
        }

        sceneType = (ESceneType)scene.buildIndex;
    }

    // 새로운 게임
    public void Load_NewWorld()
    {
        Debug.Log("Load_NewWorld");
        playTime = 0.0f;
        worldUnitCardDatas = CreateNewCards(20);
        market_UnitCardDatas.Clear();
        player_Squad_UnitCardDatas.Clear();
        player_InSquad_UnitCardDatas.Clear();
    }

    // 로딩한 게임
    public void Load_OldWorld()
    {
        Debug.Log("Load_OldWorld");
        playTime = loadData.playTime;
        saveName = loadData.saveName;

        worldUnitCardDatas = loadData.worldUnitDatas;
        market_UnitCardDatas = loadData.market_UnitCardDatas;
        player_Squad_UnitCardDatas = loadData.player_Squad_UnitCardDatas;
        player_InSquad_UnitCardDatas = loadData.player_InSquad_UnitCardDatas;

        Load_UnitInit(worldUnitCardDatas);
        Load_UnitInit(market_UnitCardDatas);
        Load_UnitInit(player_Squad_UnitCardDatas);
        Load_UnitInit(player_InSquad_UnitCardDatas);
    }

    private void Load_UnitInit(List<UnitData> unitDatas)
    {
        foreach(UnitData unitData in unitDatas) 
        {
            unitData.LoadUnit();
        }
    }
    public ESceneType GetSceneType() { return sceneType; }

    public List<UnitData> GetRandomWorldCard(int cardCount)
    {
        var list = new List<UnitData>();
        var count = Math.Min(worldUnitCardDatas.Count, cardCount);
        var randIndexList = MathUtility.GetRandomIndices(worldUnitCardDatas.Count, count);

        for (int i = 0; i < randIndexList.Count; i++)
        {
            var randomIndex = randIndexList[i];
            var randomUnit = worldUnitCardDatas[randomIndex];
            list.Add(randomUnit);
        }

        market_UnitCardDatas.Clear();
        market_UnitCardDatas.AddRange(list);

        return list;
    }


    private List<UnitData> CreateNewCards(int _createCount)
    {
        Debug.Log($"CreateNewCards - {_createCount}");

        List<UnitData> cards = new List<UnitData>();
        // EUnitTier를 순회
        var keys = DT_UnitInfo_Immutable.GetKeys();

        foreach (var item in Enum.GetValues(typeof(EUnitTier)))
        {
            var unitTier = (EUnitTier)item;
            for (int i = 0; i < keys.Count; i++)
            {
                var unitIndex = keys[i];
                var unitData = UnitData.CreateNewUnit(unitTier, unitIndex);

                cards.Add(unitData);
            }
        }

        return cards;
    }

    public void BuyUnit(string unitUniqueID)
    {
        var buyCard = worldUnitCardDatas.Find(x => x.unitUniqueID == unitUniqueID);
        if (buyCard != null)
        {
            player_Squad_UnitCardDatas.Add(buyCard);
            market_UnitCardDatas.RemoveAll(x => x.unitUniqueID == unitUniqueID);
            worldUnitCardDatas.RemoveAll(x => x.unitUniqueID == unitUniqueID); // ID와 일치하는 모든 항목 제거
        }
        else
        {
            Debug.LogWarning($"Card with ID {unitUniqueID} not found in worldUnitCardDatas.");
        }
    }

    public bool SquadActionUnitCard(string unitUniqueID, bool isAdd)
    {
        if(isAdd)
        {
            var inSquadCard = player_InSquad_UnitCardDatas.Find(x => x.unitUniqueID == unitUniqueID);

            if (inSquadCard != null)
            {
                Debug.Log("이미 스쿼드에 들어가있는 카드");
                return true;
            }

            var squadCard = player_Squad_UnitCardDatas.Find(x => x.unitUniqueID == unitUniqueID);
            player_InSquad_UnitCardDatas.Add(squadCard);
        }
        else
        {
            player_InSquad_UnitCardDatas.RemoveAll(x => x.unitUniqueID == unitUniqueID);
            return true;
        }

        return false;
    }


    public List<UnitData> GetMarketDatas()
    {
        return market_UnitCardDatas;
    }

    public List<UnitData> GetPlayer_SquadUnitDatas()
    {
        return player_Squad_UnitCardDatas;
    }

    public List<UnitData> GetPlayer_InSquadUnitDatas()
    {
        return player_InSquad_UnitCardDatas;
    }
}
