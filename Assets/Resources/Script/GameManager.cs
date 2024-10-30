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

        var panel = PanelManager.Instance;
        var poolManager = PoolManager.Instance;
        var fieldManager = FieldManager.Instance;

        InitMapData();
        fieldManager.StartGame();
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
