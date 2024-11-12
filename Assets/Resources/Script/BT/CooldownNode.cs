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

        lastExecutionTime = Time.time; // 쿨타임 대기 후 실행
        //lastExecutionTime = -cooldownTime; // 처음에는 바로 실행 가능하도록 설정
    }

    public bool CanExecute()
    {
        return (Time.time - lastExecutionTime >= cooldownTime);
    }

    public override NodeStatus Execute()
    {
        if (CanExecute())
        {
            Debug.Log($"스킬 발동 {dT_Skill.Name}");
            lastExecutionTime = Time.time;
            return NodeStatus.Success;
        }

        return NodeStatus.Failure;
    }
}
