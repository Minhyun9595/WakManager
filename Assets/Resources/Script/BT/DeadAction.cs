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
            Debug.Log($"��� ���� {blackboard.unitData.Name}");
            return NodeStatus.Success;
        }
        else
        {
            return NodeStatus.Failure;
        }
    }

}
