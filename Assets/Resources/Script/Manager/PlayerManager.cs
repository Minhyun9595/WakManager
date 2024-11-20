using QUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
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
    public TeamInfo playerTeamInfo = new TeamInfo();
    public TeamUpgrade playerTeamUpgrade = new TeamUpgrade();
    public List<TeamInfo> worldTeamList = new List<TeamInfo>();
    public string gameScheduleData;
}

public enum SceneChangeType
{
    None,
    NewWorld,
    LoadWorld,
    MoveWorld,
    FieldBattle_Scream_End,
}

public class PlayerManager : CustomSingleton<PlayerManager>
{
    new void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SceneManager.GetActiveScene().buildIndex != (int)ESceneType.Menu)
        {
            Load_NewWorld();
        }
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
    private string inputTeamName = string.Empty;
    private const int storyStartYear = 250;
    [SerializeField] private List<UnitData> worldUnitCardDatas = new List<UnitData>();
    [SerializeField] private List<UnitData> market_UnitCardDatas = new List<UnitData>();
    [SerializeField] private TeamInfo playerTeamInfo = new TeamInfo();
    [SerializeField] private TeamUpgrade playerTeamUpgrade = new TeamUpgrade();
    [SerializeField] private List<TeamInfo> worldTeamList = new List<TeamInfo>();
    [SerializeField] public GameSchedule gameSchedule = new GameSchedule(storyStartYear, 1);


    // Getter
    public TeamInfo PlayerTeamInfo { get { return playerTeamInfo; } }
    public TeamUpgrade PlayerTeamUpgrade { get { return playerTeamUpgrade; } }

    // ��ũ���� ������
    private List<TeamInfo> screamTeamInfo = new List<TeamInfo>();

    void Start()
    {
        // ����� ���� ���� ��� ���
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // ������Ʈ �̸��� ���̺� ������ ���� ����
        saveFolderPath = Path.Combine(documentsPath, "WM2025", "SaveData");

        // ������ ���� ��� ����
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
        saveData.saveName = $"����_{index + 1}";
        saveData.saveTime = DateTime.Now.ToString("yyyy��-MM��-dd�� HH:mm");
        saveData.playTime = playTime;
        saveData.worldUnitDatas = worldUnitCardDatas;
        saveData.market_UnitCardDatas = market_UnitCardDatas;
        saveData.playerTeamInfo = playerTeamInfo;
        saveData.worldTeamList = worldTeamList;
        saveData.gameScheduleData = gameSchedule.ToJson();

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

    public void SetNewGame(string _inputTeamName)
    {
        inputTeamName = _inputTeamName;
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
                // �̵��� ���� ��� �ƹ��͵� ���� �ʴ´�.
                break;
            case SceneChangeType.FieldBattle_Scream_End:
                // ���� ������ ���� ����, ����� ���� ��

                // ��ũ�� ��� UI ����ֱ�
                break;
            default:
                break;
        }

        sceneType = (ESceneType)scene.buildIndex;

        if(gameSchedule != null && FrontInfoCanvas.Instance != null)
        {
            FrontInfoCanvas.Instance.SetDateText(gameSchedule.CurrentDate);
        }

        if(sceneType == ESceneType.InGame)
        {
            FieldManager.Instance.SetInGame(screamTeamInfo);
        }
    }

    // ���ο� ����
    public void Load_NewWorld()
    {
        Debug.Log("Load_NewWorld");
        playTime = 0.0f;
        worldUnitCardDatas = CreateWorldCard(5);
        market_UnitCardDatas.Clear();
        playerTeamInfo.Clear();
        playerTeamInfo.Initialize(EUnitTier.SurplustoRequirements, "��α�");

        int multiple = 1;
#if UNITY_EDITOR
        multiple = 10;
#endif
        playerTeamInfo.AddMoney(DT_Const.GetInfoByIndex("START_GOLD") * multiple);

        playerTeamUpgrade = new TeamUpgrade();

        // �� 32�� �����
        TeamNameGenerator teamNameGenerator = new TeamNameGenerator();
        worldTeamList.Clear();

        foreach(var unitTierType in Enum.GetValues(typeof(EUnitTier)))
        {
            List<TeamInfo> newTeamInfos = new List<TeamInfo>();
            for (int i = 0; i < 32; i++)
            {
                var team = new TeamInfo();
                team.Initialize((EUnitTier)unitTierType, teamNameGenerator.GetRandomName());
                newTeamInfos.Add(team);
            }

            foreach (var team in newTeamInfos)
            {
                var teamCardList = CreateWorldTeamCard((EUnitTier)unitTierType, 5);
                foreach (var unit in teamCardList)
                {
                    team.AddInSquadUnit(unit);
                }
            }

            worldTeamList.AddRange(newTeamInfos);
        }


        // ������ ����
        gameSchedule.GenerateMonthlyCalendar(storyStartYear, 1);

        var dlg = PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_StoryDialogue, PanelRenderQueueManager.ECanvasType.FrontCanvas);
        if(dlg != null)
            dlg.GetComponent<Panel_StoryDialogue>().PlayDialogue(1);
    }

    // �ε��� ����
    public void Load_OldWorld()
    {
        Debug.Log("Load_OldWorld");
        playTime = loadData.playTime;
        saveName = loadData.saveName;

        worldUnitCardDatas = loadData.worldUnitDatas;
        market_UnitCardDatas = loadData.market_UnitCardDatas;
        playerTeamInfo = loadData.playerTeamInfo;
        playerTeamUpgrade = loadData.playerTeamUpgrade;
        worldTeamList = loadData.worldTeamList;
        gameSchedule.FromJson(loadData.gameScheduleData);

        Load_UnitInit(worldUnitCardDatas);
        Load_UnitInit(market_UnitCardDatas);
        playerTeamInfo.Load_UnitInit();
        foreach (var team in worldTeamList)
        {
            team.Load_UnitInit();
        }
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

    private List<UnitData> CreateWorldTeamCard(EUnitTier _tier, int _createCount)
    {
        List<UnitData> cards = new List<UnitData>();
        // EUnitTier�� ��ȸ
        var keys = DT_UnitInfo_Immutable.GetKeys();

        for (int i = 0; i < _createCount; i++)
        {
            var rand = UnityEngine.Random.Range(0, keys.Count);
            var unitIndex = keys[rand];
            keys.RemoveAt(rand);
            var unitData = UnitData.CreateNewUnit(_tier, unitIndex);
            cards.Add(unitData);
        }

        return cards;
    }

    private List<UnitData> CreateWorldCard(int _createCount)
    {
        List<UnitData> cards = new List<UnitData>();
        // EUnitTier�� ��ȸ
        var keys = DT_UnitInfo_Immutable.GetKeys();

        foreach (var item in Enum.GetValues(typeof(EUnitTier)))
        {
            var unitTier = (EUnitTier)item;
            for (int i = 0; i < keys.Count; i++)
            {
                for (int j = 0; j < _createCount; j++)
                {
                    var unitIndex = keys[i];
                    var unitData = UnitData.CreateNewUnit(unitTier, unitIndex);
                    cards.Add(unitData);
                }
            }
        }

        return cards;
    }

    public bool BuyUnit(string unitUniqueID)
    {
        var buyCard = worldUnitCardDatas.Find(x => x.unitUniqueID == unitUniqueID);
        if (buyCard != null)
        {
            var dt_TierInfo = DT_TierInfo.GetInfoByIndex(buyCard.eUnitTier);
            if (playerTeamInfo.ReduceMoney(dt_TierInfo.RecurtCost) == false)
            {
                return false;
            }

            playerTeamInfo.AddSquadUnit(buyCard);
            market_UnitCardDatas.RemoveAll(x => x.unitUniqueID == unitUniqueID);
            worldUnitCardDatas.RemoveAll(x => x.unitUniqueID == unitUniqueID); // ID�� ��ġ�ϴ� ��� �׸� ����

            gameSchedule.AdvanceDay();

            return true;
        }
        else
        {
            Debug.LogWarning($"Card with ID {unitUniqueID} not found in worldUnitCardDatas.");
        }

        return false;
    }

    public List<UnitData> GetMarketDatas()
    {
        return market_UnitCardDatas;
    }

    public bool SquadActionUnitCard(string unitUniqueID, bool isAdd)
    {
        return playerTeamInfo.SquadActionUnitCard(unitUniqueID, isAdd);
    }

    public List<UnitData> GetPlayer_SquadUnitDatas()
    {
        return playerTeamInfo.GetPlayer_SquadUnitDatas();
    }

    public List<UnitData> GetPlayer_InSquadUnitDatas()
    {
        return playerTeamInfo.GetPlayer_InSquadUnitDatas();
    }

    public List<TeamInfo> GetTeamInfos(EUnitTier eUnitTier)
    {
        return worldTeamList;
    }

    public bool ScreamDataSet(TeamInfo enemyTeamInfo)
    {
        if (playerTeamInfo.player_InSquad_UnitCardDatas.Count < 1)
        {
            return false;
        }
        if (gameSchedule.AddScheduleToday(EScheduleType.Scream, "��ũ��") == false)
        {
            return false;
        }

        gameSchedule.AdvanceDay();
        screamTeamInfo = new List<TeamInfo>();
        screamTeamInfo.Add(playerTeamInfo);
        screamTeamInfo.Add(enemyTeamInfo);

        return true;
    }

    public TeamInfo GetTeamInfo(string TeamIndex)
    {
        if(playerTeamInfo.TeamIndex.Equals(TeamIndex))
        {
            return playerTeamInfo;
        }
        else
        {
            return worldTeamList.Find(x => x.TeamIndex == TeamIndex);
        }
    }
}
