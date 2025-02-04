using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AttackActionNode : ActionNode
{
    private Blackboard blackboard;

    public AttackActionNode(Blackboard bb)
    {
        blackboard = bb;
    }

    public override NodeStatus Execute()
    {
        if(blackboard.unitFieldInfo.IsCanNotTarget())
        {
            return NodeStatus.Failure;
        }
        if(0 < blackboard.unitFieldInfo.NormalAction_LeftCoolTime)
        {
            return NodeStatus.Failure;
        }

        var attackSpeed = 1.0f;
        var trait = blackboard.realUnitData.GetTrait(TraitType.Agile);
        if (trait != null)
        {
            attackSpeed = 1 + (trait.Value1 * 0.01f);
        }

        blackboard.unitAnimator.SetAnimation(EAnimationType.Attack1, attackSpeed);

        return NodeStatus.Success;
    }
}
