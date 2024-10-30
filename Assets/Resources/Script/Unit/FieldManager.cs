using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : CustomSingleton<FieldManager>
{
    public void StartGame()
    {
        StageBG = GameObject.FindWithTag("StageBG");
        CreateTeam(1, new List<int> { 1, 2, 3 });
        CreateTeam(2, new List<int> { 4, 5, 6 });
    }

    private void CreateTeam(int _teamIndex, List<int> _unitIndexList)
    {
        foreach(var unitIndex in _unitIndexList)
        {
            var unit = PoolManager.Instance.GetFromPool("Unit");
            var unitAI = unit.GetComponent<Unit_AI>();
            unitAI.Initialize(_teamIndex, unitIndex);
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
}
