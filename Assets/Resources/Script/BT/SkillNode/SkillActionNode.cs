using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActionNode : BehaviorNode
{
    Blackboard blackboard;
    DT_Skill dT_Skill;

    public SkillActionNode(Blackboard _blackboard, DT_Skill _dT_Skill)
    {
        blackboard = _blackboard;
        dT_Skill = _dT_Skill;
    }

    public override NodeStatus Execute()
    {
        return NodeStatus.Success;
    }
}