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
            // Ÿ���� ������ ���� ��ȯ
            return NodeStatus.Failure;
        }

        // Ÿ���� ��ġ ��������
        Vector3 targetPosition = blackboard.targetTransform.position;
        Vector3 currentPosition = blackboard.myTransform.position;

        // Ÿ�ٰ��� �Ÿ� ���
        float distance = Vector3.Distance(currentPosition, targetPosition);

        // ��Ÿ��� ������ ǥ��
        QUtility.UIUtility.DrawDebugCircle(blackboard.myTransform.position, blackboard.unitData.Range, Color.green);

        if (distance <= blackboard.unitData.Range)
        {
            // ��ǥ ������ �����ϸ� ���� ��ȯ
            return NodeStatus.Success;
        }
        else
        {
            // Ÿ�� �������� �̵�
            var movement = MoveTowards(targetPosition);

            // ���� ��ȯ
            LookDirection(movement);

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
        // �̵� ���⿡ ���� �̹��� ���� ��ȯ
        if (movement.x > 0)
        {
            blackboard.myTransform.localScale = new Vector3(1, 1, 1);
        }
        else if (movement.x < 0)
        {
            blackboard.myTransform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
