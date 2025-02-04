using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleActionNode : ActionNode
{
    private Blackboard blackboard;

    public IdleActionNode(Blackboard bb)
    {
        blackboard = bb;
    }

    public override NodeStatus Execute()
    {
        if(blackboard.isAnimationPlaying == false)
        {
            blackboard.unitAnimator.SetAnimation(EAnimationType.Idle);
        }
        return NodeStatus.Success;
    }
}
