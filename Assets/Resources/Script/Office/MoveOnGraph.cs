using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnGraph : MonoBehaviour
{
    public float speed = 2f; // �̵� �ӵ�
    private Vector2 currentPoint; // ���� ��ġ
    private Vector2 nextPoint; // ���� ������
    private Vector2 previousPoint; // ���� ��ġ
    private bool isMoving = true; // �̵� ���� �÷���

    void Start()
    {
        InitializeStartPoint(); // ���� ���� �ʱ�ȭ
        SetNextPoint(); // ù ��° ������ ����
    }

    void Update()
    {
        if (!isMoving) return; // �̵� ���� �ƴϸ� ó������ ����

        // �������� �̵�
        transform.position = Vector2.MoveTowards(transform.position, nextPoint, Time.deltaTime * speed);

        // �������� �������� �� ����
        if (Vector2.Distance(transform.position, nextPoint) < 0.1f)
        {
            StartCoroutine(WaitBeforeNextMove());
        }
    }

    private void InitializeStartPoint()
    {
        // LineManager�� �׷������� ������ �������� ����
        if (LineManager.Instance.graph.Count > 0)
        {
            foreach (var point in LineManager.Instance.graph.Keys)
            {
                currentPoint = point; // ù ��° Ű�� ���� �������� ����
                transform.position = currentPoint; // ��ü�� �ش� ��ġ�� �̵�
                previousPoint = currentPoint; // ���� ��ġ�� �ʱ�ȭ
                Debug.Log($"Start point initialized to: {currentPoint}");
                return;
            }
        }
        else
        {
            Debug.LogError("Graph is empty! Cannot initialize start point.");
        }
    }

    private IEnumerator WaitBeforeNextMove()
    {
        isMoving = false; // �̵� ����
        yield return new WaitForSeconds(Random.Range(1f, 2f)); // 3~5�� ���
        currentPoint = nextPoint; // ���� ��ġ�� ����
        SetNextPoint(); // ���ο� ������ ����
        isMoving = true; // �̵� �簳
    }

    private void SetNextPoint()
    {
        List<Vector2> connectedPoints = LineManager.Instance.graph[currentPoint]; // ���� ��ġ���� ����� ���� ��������

        // �̵� ������ ���� ���� ���, ���� �������� ���ư�
        if (connectedPoints.Count == 1 && connectedPoints[0] == previousPoint)
        {
            nextPoint = previousPoint;
            previousPoint = currentPoint; // ���� ��ġ�� ������Ʈ
            return;
        }

        // ����� ���� 1���� ��� ������ ����
        if (connectedPoints.Count == 1)
        {
            nextPoint = connectedPoints[0];
            return;
        }

        // ����� ���� 2�� �̻��� ��� ���� �� �����ϰ� ����
        do
        {
            nextPoint = connectedPoints[Random.Range(0, connectedPoints.Count)];
        } while (connectedPoints.Count > 1 && nextPoint == previousPoint);

        previousPoint = currentPoint; // ���� ��ġ ������Ʈ
    }
}
