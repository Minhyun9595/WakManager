using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AttackAction : ActionNode
{
    private Blackboard blackboard;

    public AttackAction(Blackboard bb)
    {
        blackboard = bb;
    }

    public override NodeStatus Execute()
    {
        if (blackboard.targetTransform == null)
        {
            // 타겟이 없으면 실패 반환
            return NodeStatus.Failure;
        }

        // 공격 범위 내에 있는지 확인
        float distance = Vector3.Distance(blackboard.myTransform.position, blackboard.targetTransform.position);
        if (distance <= blackboard.unitData.Range)
        {
            // 공격 수행
            
            // 공격 후 성공 반환
            return NodeStatus.Success;
        }
        else
        {
            // 타겟이 공격 범위 밖에 있으면 실패 반환
            return NodeStatus.Failure;
        }
    }

    public void Attack()
    {
        var myUnitData = blackboard.unitData;

        var myDamageType = myUnitData.GetDamageType();
        var myDamageCount = myUnitData.MeleeDamageCount;
        var myDamage = myUnitData.MeleeDamage;

        blackboard.targetBoard.unitFieldInfo.Hit(myDamageType, myDamage);
    }
}
