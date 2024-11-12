using QUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEnemyInRangeCondition : BehaviorNode
{
    private Blackboard blackboard;
    private float nextTargetUpdateTime;
    private const float targetUpdateInterval = 0.3f; // Ÿ�� ���� �ֱ�

    public IsEnemyInRangeCondition(Blackboard bb)
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
        if (blackboard.targetUnitAI != null && Time.time < nextTargetUpdateTime)
        {
            return NodeStatus.Success; // �ֱⰡ ������ �ʾ����� ���� Ÿ���� �״�� ����
        }

        nextTargetUpdateTime = Time.time + targetUpdateInterval;

        // ���� Ÿ���� ���ų�, ����߰ų�, �� ���� Ÿ���� ���� ��� ���ο� Ÿ���� ã��
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

        foreach (var enemy in MathUtility.GetAllEnemiesInRange(blackboard.myUnitAI, blackboard.teamIndex, blackboard.myTransform.position, blackboard.realUnitData.GetRange()))
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
            // ����� ������ ��Ͽ� �� �߰� (�׾��� �� �˸�)
            blackboard.myUnitAI.SetTarget(closestEnemy);
        }
    }

    private bool HasHigherPriorityTarget()
    {
        var enemyList = MathUtility.GetAllEnemiesInRange(blackboard.myUnitAI, blackboard.teamIndex, blackboard.myTransform.position, blackboard.realUnitData.GetRange());
        foreach (var enemy in enemyList)
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
        // �켱���� �Ǵ� ����: �Ÿ��� �� �����ų� ü���� ���� ���
        return Vector3.Distance(blackboard.myTransform.position, newTarget.transform.position) <
               Vector3.Distance(blackboard.myTransform.position, currentTarget.transform.position);
    }
}
