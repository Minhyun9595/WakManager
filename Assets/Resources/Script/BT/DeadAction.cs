using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadAction : ActionNode
{
    private Blackboard blackboard;

    public DeadAction(Blackboard bb)
    {
        blackboard = bb;
    }

    public override NodeStatus Execute()
    {
        if(blackboard.unitFieldInfo.IsDead())
        {
            return NodeStatus.Success;
        }
        else
        {
            return NodeStatus.Failure;
        }
    }

}
