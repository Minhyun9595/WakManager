using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        // ������ ��ġ�� �þ� ������ ������
        Vector3 unitPosition = blackboard.myTransform.position;
        float sightRange = 10;
        TextMesh nameText = null;

        // �ֺ��� �� ���ֵ��� Ž��
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(unitPosition, sightRange);

        float closestDistance = Mathf.Infinity;
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

        if (closestEnemy != null)
        {
            blackboard.target = closestEnemy;

            // �̸� �ؽ�Ʈ ������Ʈ
            nameText = blackboard.myTransform.Find("NameText").GetComponent<TextMesh>();
            var enemyBlackBoard = closestEnemyUnit.GetBlackboard();
            nameText.text = blackboard.unitInfo.GetColorName(blackboard.teamColor) + "\nŸ�� : " + enemyBlackBoard.unitInfo.GetColorName(enemyBlackBoard.teamColor);

            return NodeStatus.Success;
        }
        else
        {
            // �ֺ��� ���� ���� ���
            nameText = blackboard.myTransform.Find("NameText").GetComponent<TextMesh>();
            nameText.text = blackboard.unitInfo.GetColorName(blackboard.teamColor);

            return NodeStatus.Failure;
        }
    }
}
