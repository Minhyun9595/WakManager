using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sirian_Skill_Node : BehaviorNode
{
    private Blackboard blackboard;

    public Sirian_Skill_Node(Blackboard bb)
    {
        blackboard = bb;
    }

    public override NodeStatus Execute()
    {
        Debug.Log("스킬 발동");
        var enemySpawnUnit = FieldManager.Instance.GetEnemySpawnedUnit_ByTeamIndex(blackboard.teamIndex);

        var aliveEnemyList = enemySpawnUnit.units.FindAll(x => x.blackboard.unitFieldInfo.IsCanNotTarget() == false);
        var aliveEnemyCount = aliveEnemyList.Count;

        if (aliveEnemyList.Count == 0)
            return NodeStatus.Failure;

        var rand = Random.Range(0, aliveEnemyCount);
        var target = aliveEnemyList[rand];

        for (int i = 0; i < 8; i++)
        {
            Projectile_Bezier.Spawn_Bezier(EPrefabType.Projectile_Bezier.ToString(), blackboard.myUnitAI, target, 
                blackboard.myTransform.position, target.transform.position, 3, 3, 1, i * 0.07f);
        }

        return NodeStatus.Success;
    }
}
