using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : ActionNode
{
    private Blackboard blackboard;

    public IdleAction(Blackboard bb)
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
