using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[System.Serializable]
public class TeamUnitData
{
    public string TeamIndex;
    public List<int> unitIndices = new List<int>();
    public List<UnitData> unitDatas = new List<UnitData>();
}

[System.Serializable]
public class SpawnedUnit
{
    public string TeamIndex;
    public List<Unit_AI> units = new List<Unit_AI>();
}


public class FieldManager : MonoBehaviour
{
    public static FieldManager instance;

    public static FieldManager Instance
    {
        get
        {
            return instance;
        }
    }

    public void Awake()
    {
        StageBG = GameObject.FindWithTag("StageBG");
        instance = this;
        InitMapData();
    }

    [SerializeField] private List<TeamUnitData> teamUnitIndices = new List<TeamUnitData>();
    [SerializeField] private List<SpawnedUnit> spawnedUnits = new List<SpawnedUnit>();

    [SerializeField] private TeamPanel teamPanel_1;
    [SerializeField] private TeamPanel teamPanel_2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ResetGame();
        }
    }

    public void SetInGame(List<TeamInfo> _teamInfoList)
    {
        spawnedUnits.Clear();
        teamUnitIndices.Clear();

        foreach (var teamInfo in _teamInfoList)
        {
            var teamUnitData = new TeamUnitData();
            teamUnitData.TeamIndex = teamInfo.TeamIndex;
            foreach (var unit in teamInfo.player_InSquad_UnitCardDatas)
            {
                teamUnitData.unitDatas.Add(unit);
            }

            teamUnitIndices.Add(teamUnitData);
        }

        // 게임 시작
        var teamCount = teamUnitIndices.Count;

        spawnedUnits = new List<SpawnedUnit>();
        for (int i = 0, length = teamCount; i < length; i++)
        {
            var spawnedUnit = new SpawnedUnit();
            spawnedUnit.TeamIndex = _teamInfoList[i].TeamIndex;
            spawnedUnits.Add(spawnedUnit);
            CreateTeam(_teamInfoList[i], i, teamUnitIndices[i].unitDatas);
        }

        InvokeRepeating("InvokeCheckGameOver", 0.0f, 1.0f);
    }

#if UNITY_EDITOR
    [ContextMenu("ResetGame")]
#endif
    public void ResetGame()
    {
        foreach (var team in spawnedUnits)
        {
            foreach (var unit in team.units)
            {
                PoolManager.Instance.ReturnToPool(EPrefabType.Unit.ToString(), unit.gameObject);
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("StartGame")]
#endif
    public void StartGame()
    {
        ResetGame();

        var teamCount = teamUnitIndices.Count;

        spawnedUnits = new List<SpawnedUnit>();
        for (int i = 0, length = teamCount; i < length; i++)
        {
            var spawnedUnit = new SpawnedUnit();
            spawnedUnits.Add(spawnedUnit);
            CreateTestTeam(i, teamUnitIndices[i].unitIndices);
        }
    }

    private void CreateTestTeam(int _teamIndex, List<int> _unitIndexList)
    {
        foreach(var unitIndex in _unitIndexList)
        {
            Vector3 position = Vector3.zero;
            if(_teamIndex % 2 == 0)
            {
                position = GetRandomRightPosition();
            }
            else
            {
                position = GetRandomLeftPosition();
            }

            var unitObject = Unit_AI.TestSpawn(position, _teamIndex, unitIndex);
            var unitAI = unitObject.GetComponent<Unit_AI>();
            spawnedUnits[_teamIndex].units.Add(unitAI);

            if(_teamIndex == 0)
            {
                teamPanel_1.InitializeTest(_teamIndex, spawnedUnits[_teamIndex].units);
            }
            else if (_teamIndex == 1)
            {
                teamPanel_2.InitializeTest(_teamIndex, spawnedUnits[_teamIndex].units);
            }
        }
    }

    private void CreateTeam(TeamInfo _teamInfo, int _teamIndex, List<UnitData> _unitDataList)
    {
        foreach (var unitData in _unitDataList)
        {
            Vector3 position = Vector3.zero;
            if (_teamIndex % 2 == 0)
            {
                position = GetRandomLeftPosition();
            }
            else
            {
                position = GetRandomRightPosition();
            }

            var unitObject = Unit_AI.Spawn(position, _teamIndex, unitData);
            var unitAI = unitObject.GetComponent<Unit_AI>();
            spawnedUnits[_teamIndex].units.Add(unitAI);

            if (_teamIndex == 0)
            {
                teamPanel_1.Initialize(_teamInfo, _teamIndex, spawnedUnits[_teamIndex].units);
            }
            else if (_teamIndex == 1)
            {
                teamPanel_2.Initialize(_teamInfo, _teamIndex, spawnedUnits[_teamIndex].units);
            }
        }
    }


    public TeamPanel GetTeamPanel(int _teamIndex)
    {
        if(_teamIndex == 0)
        {
            return teamPanel_1;
        }
        else
        {
            return teamPanel_2;
        }
    }

    public SpawnedUnit GetEnemySpawnedUnit_ByTeamIndex(int _teamIndex)
    {
        if(_teamIndex == 0)
        {
            _teamIndex = 1;
        }
        else
        {
            _teamIndex = 0;
        }

        if(_teamIndex < spawnedUnits.Count)
        {
            return spawnedUnits[_teamIndex];
        }

        Debug.LogError($"GetSpawnedUnit_ByTeamIndex {_teamIndex}");
        return null;
    }

    private void InvokeCheckGameOver()
    {
        foreach(var teamSpawnedUnit in spawnedUnits)
        {
            bool allUnitDead = true;
            foreach(var unit in teamSpawnedUnit.units)
            {
                if(unit.blackboard.unitFieldInfo.IsDead() == false)
                {
                    allUnitDead = false;
                    break;
                }
            }

            if(allUnitDead)
            {
                Debug.Log("전투 종료");
                CancelInvoke("InvokeCheckGameOver");
                Invoke("InGameOver", 1.0f);
            }
        }
    }

    private void InGameOver()
    {
        // 팀 1 전투보고서 추가
        var team0Info = PlayerManager.Instance.GetTeamInfo(spawnedUnits[0].TeamIndex);
        BattleReport team0battleReport = new BattleReport(spawnedUnits[0].units, spawnedUnits[1].units);
        team0Info.teamBattleReports.Add(team0battleReport);

        // 팀 2 전투보고서 추가
        var team1Info = PlayerManager.Instance.GetTeamInfo(spawnedUnits[1].TeamIndex);
        BattleReport team1battleReport = new BattleReport(spawnedUnits[1].units, spawnedUnits[0].units);
        team1Info.teamBattleReports.Add(team0battleReport);

        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_FieldBattleEnd, PanelRenderQueueManager.ECanvasType.FrontCanvas);
    }

    public GameObject StageBG;

    private Vector3 center;               // 중앙 위치
    private float leftBound, rightBound;  // 좌우 경계 값
    private float topBound, bottomBound;  // 상하 경계 값

    private void InitMapData()
    {
        // 맵의 SpriteRenderer 컴포넌트 가져오기
        if (StageBG == null)
            return;

        SpriteRenderer spriteRenderer = StageBG.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer가 없습니다!");
            return;
        }

        // Bounds 계산
        Bounds bounds = spriteRenderer.bounds;
        center = bounds.center;
        leftBound = bounds.min.x;
        rightBound = bounds.max.x;
        bottomBound = bounds.min.y;
        topBound = bounds.max.y;
    }

    // 맵 범위 내 왼쪽에서 랜덤 좌표 반환
    public Vector3 GetRandomLeftPosition()
    {
        float randomX = Random.Range(leftBound, center.x);
        float randomY = Random.Range(bottomBound, topBound);
        return new Vector3(randomX, randomY, 0);
    }

    // 맵 범위 내 오른쪽에서 랜덤 좌표 반환
    public Vector3 GetRandomRightPosition()
    {
        float randomX = Random.Range(center.x, rightBound);
        float randomY = Random.Range(bottomBound, topBound);
        return new Vector3(randomX, randomY, 0);
    }

    // 맵 내부의 임의 좌표 반환
    public Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(leftBound, rightBound);
        float randomY = Random.Range(bottomBound, topBound);
        return new Vector3(randomX, randomY, 0);
    }

    // 맵 밖으로 나가지 않도록 제한하는 함수
    public Vector3 ClampPosition(Vector3 position)
    {
        float clampedX = Mathf.Clamp(position.x, leftBound, rightBound);
        float clampedY = Mathf.Clamp(position.y, bottomBound, topBound);
        return new Vector3(clampedX, clampedY, position.z);
    }

    private void OnDrawGizmos()
    {
        // SpriteRenderer 컴포넌트 가져오기
        if (StageBG == null)
            return;

        SpriteRenderer spriteRenderer = StageBG.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            return;

        // Sprite의 Bounds (경계) 가져오기
        Bounds bounds = spriteRenderer.bounds;

        // Gizmo 색상 설정
        Gizmos.color = Color.green;

        // 상하좌우 좌표 계산
        Vector3 topLeft = new Vector3(bounds.min.x, bounds.max.y, 0);
        Vector3 topRight = new Vector3(bounds.max.x, bounds.max.y, 0);
        Vector3 bottomLeft = new Vector3(bounds.min.x, bounds.min.y, 0);
        Vector3 bottomRight = new Vector3(bounds.max.x, bounds.min.y, 0);

        // 사각형 그리기
        Gizmos.DrawLine(topLeft, topRight);     // 위쪽 선
        Gizmos.DrawLine(topRight, bottomRight); // 오른쪽 선
        Gizmos.DrawLine(bottomRight, bottomLeft); // 아래쪽 선
        Gizmos.DrawLine(bottomLeft, topLeft);    // 왼쪽 선
    }
}
