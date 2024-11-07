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
        lastExecutionTime = Time.time; // ��Ÿ�� ��� �� ����

        //lastExecutionTime = -cooldownTime; // ó������ �ٷ� ���� �����ϵ��� ����
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
