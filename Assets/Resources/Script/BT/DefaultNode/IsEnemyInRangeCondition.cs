using QUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEnemyInRangeCondition : BehaviorNode
{
    private Blackboard blackboard;
    private float nextTargetUpdateTime;
    private const float targetUpdateInterval = 0.3f; // 타겟 갱신 주기

    public IsEnemyInRangeCondition(Blackboard bb)
    {
        blackboard = bb;
    }

    public override NodeStatus Execute()
    {
        if (blackboard.targetUnitAI == null || blackboard.targetUnitAI.blackboard.unitFieldInfo.IsCanNotTarget())
        {
            return NodeStatus.Failure;
        }

        float distance = Vector3.Distance(
            blackboard.myTransform.position,
            blackboard.targetUnitAI.transform.position);

        var isInRange = distance <= blackboard.realUnitData.GetRange();
        return isInRange ? NodeStatus.Success : NodeStatus.Failure;
    }
}
