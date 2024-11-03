using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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

    [SerializeField] private TeamPanel teamPanel_1;
    [SerializeField] private TeamPanel teamPanel_2;
    private void Start()
    {
        StageBG = GameObject.FindWithTag("StageBG");
        InitMapData();
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

            if(_teamIndex == 0)
            {
                teamPanel_1.Initialize(_teamIndex, spawnedUnits[_teamIndex].units);
            }
            else if (_teamIndex == 1)
            {
                teamPanel_2.Initialize(_teamIndex, spawnedUnits[_teamIndex].units);
            }
        }
    }

    public GameObject StageBG;

    private Vector3 center;               // �߾� ��ġ
    private float leftBound, rightBound;  // �¿� ��� ��
    private float topBound, bottomBound;  // ���� ��� ��

    private void InitMapData()
    {
        // ���� SpriteRenderer ������Ʈ ��������
        if (StageBG == null)
            return;

        SpriteRenderer spriteRenderer = StageBG.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer�� �����ϴ�!");
            return;
        }

        // Bounds ���
        Bounds bounds = spriteRenderer.bounds;
        center = bounds.center;
        leftBound = bounds.min.x;
        rightBound = bounds.max.x;
        bottomBound = bounds.min.y;
        topBound = bounds.max.y;
    }

    // �� ���� �� ���ʿ��� ���� ��ǥ ��ȯ
    public Vector3 GetRandomLeftPosition()
    {
        float randomX = Random.Range(leftBound, center.x);
        float randomY = Random.Range(bottomBound, topBound);
        return new Vector3(randomX, randomY, 0);
    }

    // �� ���� �� �����ʿ��� ���� ��ǥ ��ȯ
    public Vector3 GetRandomRightPosition()
    {
        float randomX = Random.Range(center.x, rightBound);
        float randomY = Random.Range(bottomBound, topBound);
        return new Vector3(randomX, randomY, 0);
    }

    // �� ������ ���� ��ǥ ��ȯ
    public Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(leftBound, rightBound);
        float randomY = Random.Range(bottomBound, topBound);
        return new Vector3(randomX, randomY, 0);
    }

    // �� ������ ������ �ʵ��� �����ϴ� �Լ�
    public Vector3 ClampPosition(Vector3 position)
    {
        float clampedX = Mathf.Clamp(position.x, leftBound, rightBound);
        float clampedY = Mathf.Clamp(position.y, bottomBound, topBound);
        return new Vector3(clampedX, clampedY, position.z);
    }

    private void OnDrawGizmos()
    {
        // SpriteRenderer ������Ʈ ��������
        if (StageBG == null)
            return;

        SpriteRenderer spriteRenderer = StageBG.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            return;

        // Sprite�� Bounds (���) ��������
        Bounds bounds = spriteRenderer.bounds;

        // Gizmo ���� ����
        Gizmos.color = Color.green;

        // �����¿� ��ǥ ���
        Vector3 topLeft = new Vector3(bounds.min.x, bounds.max.y, 0);
        Vector3 topRight = new Vector3(bounds.max.x, bounds.max.y, 0);
        Vector3 bottomLeft = new Vector3(bounds.min.x, bounds.min.y, 0);
        Vector3 bottomRight = new Vector3(bounds.max.x, bounds.min.y, 0);

        // �簢�� �׸���
        Gizmos.DrawLine(topLeft, topRight);     // ���� ��
        Gizmos.DrawLine(topRight, bottomRight); // ������ ��
        Gizmos.DrawLine(bottomRight, bottomLeft); // �Ʒ��� ��
        Gizmos.DrawLine(bottomLeft, topLeft);    // ���� ��
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ResetGame();
        }
    }
}
