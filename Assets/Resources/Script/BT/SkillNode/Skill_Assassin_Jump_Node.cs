using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Skill_Assassin_Jump_Node : BehaviorNode
{
    private Blackboard blackboard;
    private float jumpDuration = 1.0f; // 점프 지속 시간 (초)
    private float maxJumpHeight = 2.0f; // 점프의 최대 높이
    private bool isJumping = false;
    private NodeStatus currentStatus = NodeStatus.Failure; // 기본 상태를 Failure로 초기화
    private Material originalMaterial; // 원래 마테리얼 저장
    private Material jumpMaterial; // 점프용 마테리얼
    public Skill_Assassin_Jump_Node(Blackboard bb)
    {
        blackboard = bb;

        jumpMaterial = Resources.Load<Material>("Materials/Assasin_Jump");
        if (jumpMaterial == null)
        {
            Debug.LogError("Assasin_Jump material not found in Resources/Materials.");
        }

        Renderer renderer = blackboard.GetBodyRenderer();
        if (renderer != null)
        {
            originalMaterial = renderer.material; // 원래 마테리얼 저장
        }
    }

    public override NodeStatus Execute()
    {
        if (isJumping)
        {
            blackboard.unitAnimator.SetAnimation(EAnimationType.Move);

            if(currentStatus == NodeStatus.Failure)
            {
                isJumping = false;
            }

            return currentStatus;
        }

        blackboard.unitFieldInfo.isNoneTargeting = true;

        // 가장 먼 적 찾기
        var enemySpawnUnit = FieldManager.Instance.GetEnemySpawnedUnit_ByTeamIndex(blackboard.teamIndex);
        if (enemySpawnUnit.units == null || enemySpawnUnit.units.Count == 0)
            return NodeStatus.Failure;

        Unit_AI farthestEnemy = null;
        if (blackboard.teamIndex % 2 == 0) // 최대 X 찾기
        {
            farthestEnemy = enemySpawnUnit.units.OrderByDescending(x => x.transform.position.x).FirstOrDefault();
        }
        else // 최소 X 찾기
        {
            farthestEnemy = enemySpawnUnit.units.OrderBy(x => x.transform.position.x).FirstOrDefault();
        }

        if (farthestEnemy == null)
            return NodeStatus.Failure;

        // 목표 위치 계산
        Vector3 startPosition = blackboard.myUnitAI.transform.position;
        Vector3 targetPosition = farthestEnemy.transform.position + new Vector3(1.0f * (blackboard.teamIndex % 2 == 0 ? -1 : 1), 0, 0); // 적 뒤쪽
        targetPosition.y = startPosition.y;

        // 마테리얼 변경
        Renderer renderer = blackboard.GetBodyRenderer();
        if (renderer != null)
        {
            if (jumpMaterial != null)
            {
                renderer.material = jumpMaterial; // 점프 마테리얼로 변경
            }
        }

        // 코루틴 시작
        isJumping = true;
        currentStatus = NodeStatus.Running; // 상태를 Running으로 설정
        blackboard.myUnitAI.StartCoroutine(JumpToTarget(startPosition, targetPosition, jumpDuration, renderer)); // 1초 동안 점프
        return currentStatus;
    }

    private IEnumerator JumpToTarget(Vector3 start, Vector3 target, float duration, Renderer renderer)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += CustomTime.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // 0에서 1까지의 비율

            // XZ 위치 선형 보간
            Vector3 currentPosition = Vector3.Lerp(start, target, t);

            // Y 위치 포물선 계산
            float height = maxJumpHeight * (1 - Mathf.Pow(2 * t - 1, 2)); // 포물선 높이 계산
            currentPosition.y += height;

            // 캐릭터 위치 업데이트
            blackboard.myUnitAI.transform.position = currentPosition;

            yield return null; // 다음 프레임까지 대기
        }

        // 마테리얼 복원
        if (renderer != null && originalMaterial != null)
        {
            renderer.material = originalMaterial;
        }

        // 점프 완료 처리
        blackboard.myUnitAI.transform.position = target;
        blackboard.unitFieldInfo.isNoneTargeting = false;
        currentStatus = NodeStatus.Failure;
    }
}
