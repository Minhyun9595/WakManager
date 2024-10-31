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
        Debug.Log("IdleAction Execute");
        return NodeStatus.Success;
    }

}
