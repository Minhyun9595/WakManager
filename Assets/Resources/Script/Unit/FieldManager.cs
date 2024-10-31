using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TeamUnitData
{
    public List<int> unitIndices = new List<int>();
}

[System.Serializable]
public class SpawnedUnit
{
    public List<Unit_AI> units = new List<Unit_AI>();
}


public class FieldManager : MonoBehaviour
{
    [SerializeField] private List<TeamUnitData> teamUnitIndices = new List<TeamUnitData>();
    [SerializeField] private List<SpawnedUnit> spawnedUnits = new List<SpawnedUnit>();


    private void Start()
    {
        StageBG = GameObject.FindWithTag("StageBG");
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

        spawnedUnits.Clear();
        if (0 < teamCount)
        {
            spawnedUnits = Enumerable.Repeat(new SpawnedUnit(), teamCount).ToList();
        }

        for (int i = 0, length = teamCount; i < length; i++)
        {
            CreateTeam(i, teamUnitIndices[i].unitIndices);
        }
    }

    private void CreateTeam(int _teamIndex, List<int> _unitIndexList)
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

            var unitObject = Unit_AI.Spawn(position, _teamIndex, unitIndex);
            var unitAI = unitObject.GetComponent<Unit_AI>();
            spawnedUnits[_teamIndex].units.Add(unitAI);
        }
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
}
