using QUtility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IsEnemyFoundCondition : BehaviorNode
{
    private Blackboard blackboard;

    public IsEnemyFoundCondition(Blackboard bb)
    {
        blackboard = bb;
    }

    public override NodeStatus Execute()
    {
        var enemyList = MathUtility.GetAllEnemiesInRange(blackboard.myUnitAI, blackboard.teamIndex, blackboard.myTransform.position, blackboard.realUnitData.GetRange());

        return 0 < enemyList.Count() ? NodeStatus.Success : NodeStatus.Failure;
    }
}
