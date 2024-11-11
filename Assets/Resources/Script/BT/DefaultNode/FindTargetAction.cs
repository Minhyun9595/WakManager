using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargetAction : ActionNode
{
    private Blackboard blackboard;
    private float nextTargetUpdateTime;
    private const float targetUpdateInterval = 0.3f; // 타겟 갱신 주기

    public FindTargetAction(Blackboard bb)
    {
        blackboard = bb;
    }

    public override NodeStatus Execute()
    {
        if (blackboard.isAnimationPlaying)
        {
            return NodeStatus.Success;
        }
        if (0 < blackboard.unitFieldInfo.NormalAction_LeftCoolTime)
        {
            return NodeStatus.Failure;
        }
        if (Time.time < nextTargetUpdateTime)
        {
            return NodeStatus.Success; // 주기가 지나지 않았으면 이전 타겟을 그대로 유지
        }

        nextTargetUpdateTime = Time.time + targetUpdateInterval;

        // 기존 타겟이 없거나, 사망했거나, 더 나은 타겟이 있을 경우 새로운 타겟을 찾음
        if (blackboard.targetUnitAI == null || HasHigherPriorityTarget())
        {
            FindNewTarget();
        }

        return blackboard.targetUnitAI != null ? NodeStatus.Success : NodeStatus.Failure;
    }

    private void FindNewTarget()
    {
        Unit_AI closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (var enemy in GetAllEnemiesInRange())
        {
            float distance = Vector3.Distance(blackboard.myTransform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            // 상대의 구독자 목록에 날 추간 (죽었을 때 알림)
            blackboard.myUnitAI.SetTarget(closestEnemy);
        }
    }

    private bool HasHigherPriorityTarget()
    {
        foreach (var enemy in GetAllEnemiesInRange())
        {
            if (IsHigherPriority(enemy, blackboard.targetUnitAI))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsHigherPriority(Unit_AI newTarget, Unit_AI currentTarget)
    {
        // 우선순위 판단 로직: 거리가 더 가깝거나 체력이 낮은 경우
        return Vector3.Distance(blackboard.myTransform.position, newTarget.transform.position) <
               Vector3.Distance(blackboard.myTransform.position, currentTarget.transform.position);
    }

    private IEnumerable<Unit_AI> GetAllEnemiesInRange()
    {
        List<Unit_AI> enemiesInRange = new List<Unit_AI>();

        // 탐색 반경 내의 모든 Collider 가져오기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(blackboard.myTransform.position, 1000);

        QUtility.UIUtility.DrawDebugCircle(blackboard.myTransform.position, blackboard.realUnitData.GetRange(), Color.yellow);
        foreach (Collider2D collider in colliders)
        {
            Unit_AI enemyUnit = collider.GetComponent<Unit_AI>();
            if (enemyUnit != null && 
                enemyUnit != blackboard.myUnitAI && 
                enemyUnit.blackboard.unitFieldInfo.IsDead() == false &&
                enemyUnit.GetBlackboard().teamIndex != blackboard.teamIndex) // 자신은 제외 // 같은 팀은 제외 // 죽은 유닛 제외
            {
                enemiesInRange.Add(enemyUnit);
            }
        }

        return enemiesInRange;
    }
}
