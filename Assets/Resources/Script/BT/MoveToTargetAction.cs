using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MoveToTargetAction : ActionNode
{
    private Blackboard blackboard;

    public MoveToTargetAction(Blackboard bb)
    {
        blackboard = bb;
    }

    public override NodeStatus Execute()
    {
        if (blackboard.targetTransform == null)
        {
            // 타겟이 없으면 반환
            return NodeStatus.Success;
        }

        // 타겟의 위치 가져오기
        Vector3 targetPosition = blackboard.targetTransform.position;
        Vector3 currentPosition = blackboard.myTransform.position;

        // 타겟과의 거리 계산
        float distance = Vector3.Distance(currentPosition, targetPosition);

        // 사거리를 원으로 표시
        QUtility.UIUtility.DrawDebugCircle(blackboard.myTransform.position, blackboard.unitData.Range, Color.green);

        if (distance <= blackboard.unitData.Range)
        {
            // 목표 지점에 도달하면 성공 반환
            return NodeStatus.Success;
        }
        else
        {
            // 타겟 방향으로 이동
            var movement = MoveTowards(targetPosition);

            // 방향 전환
            LookDirection(movement);

            // 이동 중이므로 Running 반환
            return NodeStatus.Running;
        }
    }

    private Vector3 MoveTowards(Vector3 targetPosition)
    {
        // 이동 방향 계산
        Vector3 direction = (targetPosition - blackboard.myTransform.position).normalized;

        // 이동 속도 적용
        Vector3 movement = new Vector3(blackboard.unitData.MoveSpeed_X * direction.x, blackboard.unitData.MoveSpeed_Y * direction.y, 0) * Time.deltaTime * ConstValue.speedRatio;

        // 실제 이동 적용
        blackboard.myTransform.position += movement;

        return movement;
    }

    private void LookDirection(Vector3 movement)
    {
        Vector3 localScale = blackboard.myBodyTransform.localScale;

        if (movement.x > 0)
        {
            // Keep the original scale, only adjusting the sign for x
            blackboard.myBodyTransform.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
        else if (movement.x < 0)
        {
            blackboard.myBodyTransform.localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
    }
}
