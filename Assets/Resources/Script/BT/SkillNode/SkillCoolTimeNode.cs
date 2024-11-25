using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SkillCoolTimeNode : BehaviorNode
{
    private float cooldownTime;
    private float lastExecutionTime;
    private float castingTime;
    private string castingAnimation;
    private Blackboard blackboard;

    public SkillCoolTimeNode(Blackboard bb, bool runImmediately, float _cooldownTime, float _castingTime, string _castingAnimation)
    {
        blackboard = bb;
        cooldownTime = _cooldownTime;
        castingTime = _castingTime;
        castingAnimation = _castingAnimation;

        if(runImmediately)
        {
            lastExecutionTime = -cooldownTime; // ��� ����
        }
        else
        {
            lastExecutionTime = CustomTime.time; // ��Ÿ�� ��� �� ����
        }
    }

    public bool CanExecute()
    {
        return (CustomTime.time - lastExecutionTime >= cooldownTime);
    }

    public override NodeStatus Execute()
    {
        if (CanExecute())
        {
            Debug.Log("Execute");
            lastExecutionTime = CustomTime.time;
            blackboard.unitAnimator.SetAnimation(EAnimationType.Skill);

            return NodeStatus.Success;
        }

        return NodeStatus.Failure;
    }
}
