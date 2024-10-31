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
        // 공격 쿨타임중이면 실패 반환
        if(0 < blackboard.unitFieldInfo.NormalAction_LeftCoolTime)
        {
            return NodeStatus.Failure;
        }

        // 유닛의 위치와 시야 범위를 가져옴
        Vector3 unitPosition = blackboard.myTransform.position;

        // 주변의 적 유닛들을 탐색
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(unitPosition, 1000);

        float closestDistance = Mathf.Infinity;
        TextMesh nameText = null;
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
                    // 적과의 거리 계산
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
            blackboard.targetTransform = closestEnemy;
            blackboard.targetBoard = closestEnemyUnit.GetBlackboard();

            // 이름 텍스트 업데이트
            nameText = blackboard.myTransform.Find("NameText").GetComponent<TextMesh>();
            var enemyBlackBoard = closestEnemyUnit.GetBlackboard();
            nameText.text = blackboard.unitData.GetColorName(blackboard.teamColor) + "\n타겟 : " + enemyBlackBoard.unitData.GetColorName(enemyBlackBoard.teamColor);

            return NodeStatus.Success;
        }
        else
        {
            // 주변에 적이 없는 경우
            nameText = blackboard.myTransform.Find("NameText").GetComponent<TextMesh>();
            nameText.text = blackboard.unitData.GetColorName(blackboard.teamColor);

            return NodeStatus.Failure;
        }
    }
}
