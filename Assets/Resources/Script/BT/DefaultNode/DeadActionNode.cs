using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadActionNode: ActionNode
{
    private Blackboard blackboard;

    public DeadActionNode(Blackboard bb)
    {
        blackboard = bb;
    }

    public override NodeStatus Execute()
    {
        if(blackboard.unitFieldInfo.IsDead())
        {
            if(blackboard.unitFieldInfo.isDead == false)
            {
                blackboard.unitFieldInfo.isDead = true;
                blackboard.myUnitAI.Die();
            }
            blackboard.unitAnimator.SetAnimation(EAnimationType.Death);
            return NodeStatus.Success;
        }
        else
        {
            return NodeStatus.Failure;
        }
    }

}
