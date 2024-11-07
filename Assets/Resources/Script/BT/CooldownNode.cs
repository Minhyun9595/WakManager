using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CooldownNode : BehaviorNode
{
    private float cooldownTime;
    private float lastExecutionTime;
    private Blackboard blackboard;

    public CooldownNode(Blackboard bb, float _cooldownTime)
    {
        blackboard = bb;
        cooldownTime = _cooldownTime;
        lastExecutionTime = Time.time; // 쿨타임 대기 후 실행

        //lastExecutionTime = -cooldownTime; // 처음에는 바로 실행 가능하도록 설정
    }

    public bool CanExecute()
    {
        Debug.Log((Time.time - lastExecutionTime >= cooldownTime).ToString());
        return (Time.time - lastExecutionTime >= cooldownTime);
    }

    public override NodeStatus Execute()
    {
        if (CanExecute())
        {
            lastExecutionTime = Time.time;
            return NodeStatus.Success;
        }

        return NodeStatus.Failure;
    }
}
