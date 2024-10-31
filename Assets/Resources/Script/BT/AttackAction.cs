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
            // Ÿ���� ������ ���� ��ȯ
            return NodeStatus.Failure;
        }

        // ���� ���� ���� �ִ��� Ȯ��
        float distance = Vector3.Distance(blackboard.myTransform.position, blackboard.targetTransform.position);
        if (distance <= blackboard.unitData.Range)
        {
            // ���� ����
            
            // ���� �� ���� ��ȯ
            return NodeStatus.Success;
        }
        else
        {
            // Ÿ���� ���� ���� �ۿ� ������ ���� ��ȯ
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
