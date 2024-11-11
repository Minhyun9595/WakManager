using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargetAction : ActionNode
{
    private Blackboard blackboard;
    private float nextTargetUpdateTime;
    private const float targetUpdateInterval = 0.3f; // Ÿ�� ���� �ֱ�

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
            // ����� ������ ��Ͽ� �� �߰� (�׾��� �� �˸�)
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
        // �켱���� �Ǵ� ����: �Ÿ��� �� �����ų� ü���� ���� ���
        return Vector3.Distance(blackboard.myTransform.position, newTarget.transform.position) <
               Vector3.Distance(blackboard.myTransform.position, currentTarget.transform.position);
    }

    private IEnumerable<Unit_AI> GetAllEnemiesInRange()
    {
        List<Unit_AI> enemiesInRange = new List<Unit_AI>();

        // Ž�� �ݰ� ���� ��� Collider ��������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(blackboard.myTransform.position, 1000);

        QUtility.UIUtility.DrawDebugCircle(blackboard.myTransform.position, blackboard.realUnitData.GetRange(), Color.yellow);
        foreach (Collider2D collider in colliders)
        {
            Unit_AI enemyUnit = collider.GetComponent<Unit_AI>();
            if (enemyUnit != null && 
                enemyUnit != blackboard.myUnitAI && 
                enemyUnit.blackboard.unitFieldInfo.IsDead() == false &&
                enemyUnit.GetBlackboard().teamIndex != blackboard.teamIndex) // �ڽ��� ���� // ���� ���� ���� // ���� ���� ����
            {
                enemiesInRange.Add(enemyUnit);
            }
        }

        return enemiesInRange;
    }
}
