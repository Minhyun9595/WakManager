using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetActionNode : ActionNode
{
    private Blackboard blackboard;
    DT_Trait speedTrait;

    public MoveToTargetActionNode(Blackboard bb)
    {
        blackboard = bb;
        speedTrait = blackboard.realUnitData.GetTrait(TraitType.Agile);
    }

    public override NodeStatus Execute()
    {
        if (blackboard.targetUnitAI == null)
        {
            Debug.Log("Target is null");
            // 타겟이 없으면 반환
            return NodeStatus.Failure;
        }

        // 타겟의 위치 가져오기
        Vector3 targetPosition = blackboard.targetUnitAI.transform.position;
        Vector3 currentPosition = blackboard.myTransform.position;

        // 타겟과의 거리 계산
        float distance = Vector3.Distance(currentPosition, targetPosition);

        // 사거리를 원으로 표시
        var range = blackboard.realUnitData.GetRange();
        QUtility.UIUtility.DrawDebugCircle(blackboard.myTransform.position, range, Color.green);

        if (distance <= range)  
        {
            // 목표 지점에 도달하면 성공 반환
            return NodeStatus.Success;
        }
        else
        {
            // 타겟 방향으로 이동
            var movement = MoveTowards(targetPosition);

            if(movement != Vector3.zero)
            {
                // 애니메이션 전환
                blackboard.unitAnimator.SetAnimation(EAnimationType.Move);

                // 방향 전환
                LookDirection(movement);
            }


            // 이동 중이므로 Running 반환
            return NodeStatus.Running;
        }
    }


    private Vector3 MoveTowards(Vector3 targetPosition)
    {
        // 이동 방향 계산
        Vector3 delta = targetPosition - blackboard.myTransform.position;
        var moveSpeed_X = blackboard.realUnitData.unitStat.MoveSpeed_X;
        var moveSpeed_Y = blackboard.realUnitData.unitStat.MoveSpeed_Y;

        if (speedTrait != null)
        {
            moveSpeed_X *= speedTrait.Value1;
            moveSpeed_Y *= speedTrait.Value1;
        }

        // X와 Y 각각의 방향에 따라 속도 계산
        float movementX = Mathf.Sign(delta.x) * moveSpeed_X * CustomTime.deltaTime * ConstValue.speedRatio;
        float movementY = Mathf.Sign(delta.y) * moveSpeed_Y * CustomTime.deltaTime * ConstValue.speedRatio;

        // 이동 거리 제한: 목표 지점을 초과하지 않도록 보정
        if (Mathf.Abs(movementX) > Mathf.Abs(delta.x)) movementX = delta.x;
        if (Mathf.Abs(movementY) > Mathf.Abs(delta.y)) movementY = delta.y;

        // 이동 벡터 계산
        Vector3 movement = new Vector3(movementX, movementY, 0);

        // 실제 이동 적용
        blackboard.myTransform.position += movement;

        return movement;
    }

    private void LookDirection(Vector3 movement)
    {
        Vector3 localScale = blackboard.myBodyTransform.localScale;

        if (movement.x > 0)
        {
            blackboard.myBodyTransform.localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
        else if (movement.x < 0)
        {
            blackboard.myBodyTransform.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
    }
}
