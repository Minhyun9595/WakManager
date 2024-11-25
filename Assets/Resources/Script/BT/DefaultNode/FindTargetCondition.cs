using QUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargetCondition : BehaviorNode
{
    private Blackboard blackboard;
    private float nextTargetUpdateTime;
    private const float targetUpdateInterval = 0.3f; // Ÿ�� ���� �ֱ�


    public FindTargetCondition(Blackboard bb)
    {
        blackboard = bb;
    }

    public override NodeStatus Execute()
    {
        if(blackboard.unitFieldInfo.IsCanNotTarget())
        {
            return NodeStatus.Failure;
        }    

        if(blackboard.isAnimationPlaying)
        {
            return NodeStatus.Failure;
        }

        // �̹� Ÿ���� ��ȿ�ϸ� Success
        if (blackboard.targetUnitAI != null &&
            blackboard.targetUnitAI.blackboard.unitFieldInfo.IsCanNotTarget() == false)
        {
            return NodeStatus.Success;
        }

        // 0.3�ʰ� ������ �ʾҴٸ� ���� Ÿ�� ����
        if (CustomTime.time < nextTargetUpdateTime)
        {
            return blackboard.targetUnitAI != null ? NodeStatus.Success : NodeStatus.Failure;
        }

        nextTargetUpdateTime = CustomTime.time + targetUpdateInterval;

        // ���ο� Ÿ�� Ž��
        FindNewTarget();

        return blackboard.targetUnitAI != null ? NodeStatus.Success : NodeStatus.Failure;
    }

    private void FindNewTarget()
    {
        Unit_AI highestPriorityEnemy = null;
        float highestPriorityScore = float.MinValue;

        foreach (var enemy in MathUtility.GetAllEnemiesInRange(
            blackboard.myUnitAI,
            blackboard.teamIndex,
            blackboard.myTransform.position,
            1000))
        {
            float priorityScore = CalculatePriorityScore(enemy);

            if (priorityScore > highestPriorityScore)
            {
                highestPriorityScore = priorityScore;
                highestPriorityEnemy = enemy;
            }
        }

        if (highestPriorityEnemy != null)
        {
            blackboard.myUnitAI.SetTarget(highestPriorityEnemy);
        }
    }

    private bool HasHigherPriorityTarget()
    {
        foreach (var enemy in MathUtility.GetAllEnemiesInRange(
            blackboard.myUnitAI,
            blackboard.teamIndex,
            blackboard.myTransform.position,
            blackboard.realUnitData.GetRange()))
        {
            if (IsHigherPriority(enemy, blackboard.targetUnitAI))
            {
                return true;
            }
        }
        return false;
    }

    private float CalculatePriorityScore(Unit_AI enemy)
    {
        // �켱���� ��� ����: �Ÿ��� �������� ������ ����
        float distance = Vector3.Distance(blackboard.myTransform.position, enemy.transform.position);

        return (1.0f / distance);
    }

    private bool IsHigherPriority(Unit_AI newTarget, Unit_AI currentTarget)
    {
        return CalculatePriorityScore(newTarget) > CalculatePriorityScore(currentTarget);
    }
}

