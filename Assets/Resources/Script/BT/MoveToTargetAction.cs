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
        if (blackboard.targetUnitAI == null)
        {
            // Ÿ���� ������ ��ȯ
            return NodeStatus.Failure;
        }

        // Ÿ���� ��ġ ��������
        Vector3 targetPosition = blackboard.targetUnitAI.transform.position;
        Vector3 currentPosition = blackboard.myTransform.position;

        // Ÿ�ٰ��� �Ÿ� ���
        float distance = Vector3.Distance(currentPosition, targetPosition);

        // ��Ÿ��� ������ ǥ��
        var range = blackboard.unitData.GetRange();
        QUtility.UIUtility.DrawDebugCircle(blackboard.myTransform.position, range, Color.green);

        if (distance <= range)  
        {
            // ��ǥ ������ �����ϸ� ���� ��ȯ
            return NodeStatus.Success;
        }
        else
        {
            // Ÿ�� �������� �̵�
            var movement = MoveTowards(targetPosition);

            if(movement != Vector3.zero)
            {
                // �ִϸ��̼� ��ȯ
                blackboard.unitAnimator.SetAnimation(EAnimationType.Move);

                // ���� ��ȯ
                LookDirection(movement);
            }


            // �̵� ���̹Ƿ� Running ��ȯ
            return NodeStatus.Running;
        }
    }

    private Vector3 MoveTowards(Vector3 targetPosition)
    {
        // �̵� ���� ���
        Vector3 direction = (targetPosition - blackboard.myTransform.position).normalized;

        // �̵� �ӵ� ����
        Vector3 movement = new Vector3(blackboard.unitData.MoveSpeed_X * direction.x, blackboard.unitData.MoveSpeed_Y * direction.y, 0) * Time.deltaTime * ConstValue.speedRatio;

        // ���� �̵� ����
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
