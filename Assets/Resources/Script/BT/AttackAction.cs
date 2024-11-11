using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DamageInfo
{
    public float damage { get; set; }
    public bool isCritical { get; set; }

    public DamageInfo() { }
    public DamageInfo(float damage, bool isCritical)
    {
        this.damage = damage;
        this.isCritical = isCritical;
    }
}

public class AttackAction : ActionNode
{
    private Blackboard blackboard;

    public AttackAction(Blackboard bb)
    {
        blackboard = bb;
    }

    public override NodeStatus Execute()
    {
        if (blackboard.targetUnitAI == null)
        {
            // Ÿ���� ������ ���� ��ȯ
            return NodeStatus.Failure;
        }

        // ���� ���� ���� �ִ��� Ȯ��
        float distance = Vector3.Distance(blackboard.myTransform.position, blackboard.targetUnitAI.transform.position);
        if (distance <= blackboard.realUnitData.GetRange())
        {
            // ���� ����
            blackboard.unitAnimator.SetAnimation(EAnimationType.Attack1);

            // ���� �� ���� ��ȯ
            return NodeStatus.Success;
        }
        else
        {
            // Ÿ���� ���� ���� �ۿ� ������ ���� ��ȯ
            return NodeStatus.Failure;
        }
    }
}
