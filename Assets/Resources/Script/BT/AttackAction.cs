using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if(blackboard.unitFieldInfo.IsCanNotTarget())
        {
            return NodeStatus.Failure;
        }

        blackboard.unitAnimator.SetAnimation(EAnimationType.Attack1);

        return NodeStatus.Success;
    }
}
