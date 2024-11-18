using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnGraph : MonoBehaviour
{
    public float speed = 2f; // 이동 속도
    private Vector2 currentPoint; // 현재 위치
    private Vector2 nextPoint; // 다음 목적지
    private Vector2 previousPoint; // 이전 위치
    private bool isMoving = true; // 이동 여부 플래그

    void Start()
    {
        InitializeStartPoint(); // 시작 지점 초기화
        SetNextPoint(); // 첫 번째 목적지 설정
    }

    void Update()
    {
        if (!isMoving) return; // 이동 중이 아니면 처리하지 않음

        // 목적지로 이동
        transform.position = Vector2.MoveTowards(transform.position, nextPoint, Time.deltaTime * speed);

        // 목적지에 도달했을 때 멈춤
        if (Vector2.Distance(transform.position, nextPoint) < 0.1f)
        {
            StartCoroutine(WaitBeforeNextMove());
        }
    }

    private void InitializeStartPoint()
    {
        // LineManager의 그래프에서 임의의 시작점을 선택
        if (LineManager.Instance.graph.Count > 0)
        {
            foreach (var point in LineManager.Instance.graph.Keys)
            {
                currentPoint = point; // 첫 번째 키를 시작 지점으로 설정
                transform.position = currentPoint; // 객체를 해당 위치로 이동
                previousPoint = currentPoint; // 이전 위치도 초기화
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
        isMoving = false; // 이동 중지
        yield return new WaitForSeconds(Random.Range(1f, 2f)); // 3~5초 대기
        currentPoint = nextPoint; // 현재 위치를 갱신
        SetNextPoint(); // 새로운 목적지 설정
        isMoving = true; // 이동 재개
    }

    private void SetNextPoint()
    {
        List<Vector2> connectedPoints = LineManager.Instance.graph[currentPoint]; // 현재 위치에서 연결된 점들 가져오기

        // 이동 가능한 점이 없는 경우, 이전 지점으로 돌아감
        if (connectedPoints.Count == 1 && connectedPoints[0] == previousPoint)
        {
            nextPoint = previousPoint;
            previousPoint = currentPoint; // 이전 위치를 업데이트
            return;
        }

        // 연결된 점이 1개일 경우 무조건 선택
        if (connectedPoints.Count == 1)
        {
            nextPoint = connectedPoints[0];
            return;
        }

        // 연결된 점이 2개 이상일 경우 이전 점 제외하고 선택
        do
        {
            nextPoint = connectedPoints[Random.Range(0, connectedPoints.Count)];
        } while (connectedPoints.Count > 1 && nextPoint == previousPoint);

        previousPoint = currentPoint; // 이전 위치 업데이트
    }
}
