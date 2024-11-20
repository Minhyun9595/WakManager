using QUtility;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.U2D;

public class OfficeUnitObject : MonoBehaviour
{
    private static EPrefabType prefabType = EPrefabType.OfficeUnitObject;
    public static OfficeUnitObject Spawn(UnitData unitData)
    {
        GameObject projectileObject = PoolManager.Instance.GetFromPool(prefabType.ToString());
        OfficeUnitObject officeUnitObject = projectileObject.GetComponent<OfficeUnitObject>();
        officeUnitObject.Set(unitData);

        return officeUnitObject;
    }

    public float speed = 2f; // 이동 속도
    private Vector2 currentPoint; // 현재 위치
    private Vector2 nextPoint; // 다음 목적지
    private Vector2 previousPoint; // 이전 위치
    private bool isMoving = true; // 이동 여부 플래그
    private Animator animator;
    private SpriteRenderer sprite;
    public string unitUniqueID;

    public Transform MessageBox;
    public SpriteRenderer MessageBox_SpriteRenderer;
    public TextMeshPro MessageText;

    private void Awake()
    {
        animator = UIUtility.FindComponentInChildrenByName<Animator>(gameObject, "Sprite");
        sprite = UIUtility.FindComponentInChildrenByName<SpriteRenderer>(gameObject, "Sprite");
        MessageBox = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "MessageBox");
        MessageBox_SpriteRenderer = UIUtility.FindComponentInChildrenByName<SpriteRenderer>(gameObject, "MessageBox");
        MessageText = UIUtility.FindComponentInChildrenByName<TextMeshPro>(gameObject, "MessageText");

    }

    public void Set(UnitData unitData)
    {
        MessageBox.gameObject.SetActive(false);
        MessageText.gameObject.SetActive(false);

        unitUniqueID = unitData.unitUniqueID;

        var controller = Resources.Load<RuntimeAnimatorController>($"Animation/UnitAnimation/{unitData.unitInfo_Immutable.Animator}/{unitData.unitInfo_Immutable.Animator}");
        animator.runtimeAnimatorController = controller;
        animator.Play("Move");

        ShowMessage($"안녕하세요 고정멤버 {unitData.unitInfo_Immutable.Name}입니다.");
    }

    void Start()
    {
        InitializeStartPoint();
        SetNextPoint();
    }

    void Update()
    {
        if (!isMoving) return;

        Vector2 direction = nextPoint - (Vector2)transform.position;
        transform.position = Vector2.MoveTowards(transform.position, nextPoint, Time.deltaTime * speed);

        if (direction.x > 0)
        {
            sprite.flipX = true;
        }
        else if (direction.x < 0)
        {
            sprite.flipX = false;
        }

        if (Vector2.Distance(transform.position, nextPoint) < 0.1f)
        {
            StartCoroutine(WaitBeforeNextMove());
        }
    }

    private void InitializeStartPoint()
    {
        if (LineManager.Instance.graph.Count > 0)
        {
            var graphKeys = new List<Vector2>(LineManager.Instance.graph.Keys); // 키를 리스트로 변환
            var randomIndex = Random.Range(0, graphKeys.Count); // 무작위 인덱스 생성
            var randomKey = graphKeys[randomIndex]; // 무작위 키 선택

            currentPoint = randomKey; // 무작위 키를 시작 지점으로 설정
            transform.position = currentPoint; // 객체를 해당 위치로 이동
            previousPoint = currentPoint; // 이전 위치도 초기화
        }
    }

    private IEnumerator WaitBeforeNextMove()
    {
        isMoving = false; // 이동 중지

        var rand = Random.Range(0, 100);
        if (rand < 20)
        {
            animator.Play("Idle");
            yield return new WaitForSeconds(Random.Range(0.5f, 3f)); // 3~5초 대기
            animator.Play("Move");
        }
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

    public void ReturnToPool()
    {
        PoolManager.Instance.ReturnToPool(prefabType.ToString(), gameObject);
    }

    public void ShowMessage(string message)
    {
        StartCoroutine(DisplayMessageCoroutine(message));
    }

    private IEnumerator DisplayMessageCoroutine(string message)
    {
        // 메시지 출력 시작
        MessageBox.gameObject.SetActive(true);
        MessageText.gameObject.SetActive(true);

        MessageText.text = ""; // 텍스트 초기화
        for (int i = 0; i < message.Length; i++)
        {
            MessageText.text += message[i]; // 1글자씩 추가
            AdjustMessageBoxSize(); // MessageBox 크기 조정
            yield return new WaitForSeconds(0.05f); // 0.02초 대기
        }

        // 모든 글자 출력 후 1초 대기
        yield return new WaitForSeconds(2.5f);

        // 메시지 숨기기
        MessageBox.gameObject.SetActive(false);
        MessageText.gameObject.SetActive(false);

        // 다음 메시지 출력 대기 시간 (5~25초 랜덤)
        float waitTime = Random.Range(5f, 15f);
        yield return new WaitForSeconds(waitTime);

        // 다시 메시지 출력 (임의의 메시지를 사용하거나 매개변수를 반복적으로 사용)
        ShowMessage(message);
    }

    private Vector2 padding = new Vector2(0.2f, 0.1f); // 텍스트 외부의 여백 (x: 좌우, y: 위아래)

    private void AdjustMessageBoxSize()
    {
        if (MessageText == null || MessageBox == null) return;

        // TextMeshPro의 Preferred Values 계산
        float textWidth = MessageText.renderedWidth;  // 실제 렌더링된 가로 길이
        float textHeight = MessageText.renderedHeight; // 실제 렌더링된 세로 길이

        // MessageBox 크기 계산
        Vector2 newSize = new Vector2(
            Mathf.Max(textWidth + padding.x * 2, 0.5f), // 텍스트 너비 + 좌우 여백
            Mathf.Max(textHeight + padding.y * 2, 0.2f) // 텍스트 높이 + 위아래 여백
        );

        // SpriteRenderer 크기 조정
        SpriteRenderer messageBoxRenderer = MessageBox.GetComponent<SpriteRenderer>();
        if (messageBoxRenderer != null)
        {
            messageBoxRenderer.drawMode = SpriteDrawMode.Sliced; // Sliced 모드 설정
            messageBoxRenderer.size = newSize; // 크기 변경
        }

        // MessageBox 위치 조정 (텍스트 중심에 맞추기)
        MessageBox.localPosition = new Vector3(
            MessageText.transform.localPosition.x,
            MessageText.transform.localPosition.y,
            MessageBox.localPosition.z // 기존 Z축 값 유지
        );
    }

}
