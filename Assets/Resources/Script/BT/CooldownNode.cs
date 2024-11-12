using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CooldownNode : BehaviorNode
{
    private float cooldownTime;
    private float lastExecutionTime;
    private Blackboard blackboard;
    private DT_Skill dT_Skill;

    public CooldownNode(Blackboard bb, DT_Skill _dT_Skill)
    {
        blackboard = bb;
        dT_Skill = _dT_Skill;
        cooldownTime = dT_Skill.CoolTime;

        lastExecutionTime = Time.time; // ��Ÿ�� ��� �� ����
        //lastExecutionTime = -cooldownTime; // ó������ �ٷ� ���� �����ϵ��� ����
    }

    public bool CanExecute()
    {
        return (Time.time - lastExecutionTime >= cooldownTime);
    }

    public override NodeStatus Execute()
    {
        if (CanExecute())
        {
            Debug.Log($"��ų �ߵ� {dT_Skill.Name}");
            lastExecutionTime = Time.time;
            return NodeStatus.Success;
        }

        return NodeStatus.Failure;
    }
}
