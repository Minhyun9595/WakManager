using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;

public class FindEnemyAction : ActionNode
{
    private Blackboard blackboard;

    public FindEnemyAction(Blackboard bb)
    {
        blackboard = bb;
    }

    public override NodeStatus Execute()
    {
        // ���� ��Ÿ�����̸� ���� ��ȯ
        if(0 < blackboard.unitFieldInfo.NormalAction_LeftCoolTime)
        {
            return NodeStatus.Failure;
        }

        // ������ ��ġ�� �þ� ������ ������
        Vector3 unitPosition = blackboard.myTransform.position;

        // �ֺ��� �� ���ֵ��� Ž��
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(unitPosition, 1000);

        float closestDistance = Mathf.Infinity;
        TextMeshPro nameText = null;
        Transform closestEnemy = null;
        Unit_AI closestEnemyUnit = null;

        foreach (var collider in hitColliders)
        {
            var enemyUnit = collider.GetComponent<Unit_AI>();
            if (enemyUnit != null)
            {
                var enemyBlackBoard = enemyUnit.GetBlackboard();
                if (enemyBlackBoard.teamIndex != blackboard.teamIndex)
                {
                    // ������ �Ÿ� ���
                    float distance = Vector3.Distance(unitPosition, enemyUnit.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = enemyUnit.transform;
                        closestEnemyUnit = enemyUnit;
                    }
                }
            }
        }

        if (closestEnemy != null && closestEnemyUnit.GetBlackboard().unitFieldInfo.IsDead() == false)
        {
            blackboard.targetTransform = closestEnemy;
            blackboard.targetBoard = closestEnemyUnit.GetBlackboard();

            var enemyBlackBoard = closestEnemyUnit.GetBlackboard();

            return NodeStatus.Success;
        }
        else
        {
            return NodeStatus.Failure;
        }
    }
}
