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
        if (blackboard.target == null)
        {
            // 타겟이 없으면 실패 반환
            return NodeStatus.Failure;
        }

        // 타겟의 위치 가져오기
        Vector3 targetPosition = blackboard.target.position;
        Vector3 currentPosition = blackboard.myTransform.position;

        // 타겟과의 거리 계산
        float distance = Vector3.Distance(currentPosition, targetPosition);

        if (distance <= 1.5f)
        {
            // 목표 지점에 도달하면 성공 반환
            return NodeStatus.Success;
        }
        else
        {
            // 타겟 방향으로 이동
            MoveTowards(targetPosition);

            // 이동 중이므로 Running 반환
            return NodeStatus.Running;
        }
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        // 이동 방향 계산
        Vector3 direction = (targetPosition - blackboard.myTransform.position).normalized;

        // 이동 속도 적용
        Vector3 movement = new Vector3(blackboard.unitInfo.MoveSpeed_X * direction.x, blackboard.unitInfo.MoveSpeed_Y * direction.y, 0) * Time.deltaTime * ConstValue.speedRatio;

        // 실제 이동 적용
        blackboard.myTransform.position += movement;

        // 애니메이션 또는 방향 전환 처리 (필요한 경우)
    }
}
