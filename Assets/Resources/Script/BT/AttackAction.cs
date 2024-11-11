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
            // 타겟이 없으면 실패 반환
            return NodeStatus.Failure;
        }

        // 공격 범위 내에 있는지 확인
        float distance = Vector3.Distance(blackboard.myTransform.position, blackboard.targetUnitAI.transform.position);
        if (distance <= blackboard.realUnitData.GetRange())
        {
            // 공격 수행
            blackboard.unitAnimator.SetAnimation(EAnimationType.Attack1);

            // 공격 후 성공 반환
            return NodeStatus.Success;
        }
        else
        {
            // 타겟이 공격 범위 밖에 있으면 실패 반환
            return NodeStatus.Failure;
        }
    }
}
