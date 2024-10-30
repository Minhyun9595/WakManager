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
            // Ÿ���� ������ ���� ��ȯ
            return NodeStatus.Failure;
        }

        // Ÿ���� ��ġ ��������
        Vector3 targetPosition = blackboard.target.position;
        Vector3 currentPosition = blackboard.myTransform.position;

        // Ÿ�ٰ��� �Ÿ� ���
        float distance = Vector3.Distance(currentPosition, targetPosition);

        if (distance <= 1.5f)
        {
            // ��ǥ ������ �����ϸ� ���� ��ȯ
            return NodeStatus.Success;
        }
        else
        {
            // Ÿ�� �������� �̵�
            MoveTowards(targetPosition);

            // �̵� ���̹Ƿ� Running ��ȯ
            return NodeStatus.Running;
        }
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        // �̵� ���� ���
        Vector3 direction = (targetPosition - blackboard.myTransform.position).normalized;

        // �̵� �ӵ� ����
        Vector3 movement = new Vector3(blackboard.unitInfo.MoveSpeed_X * direction.x, blackboard.unitInfo.MoveSpeed_Y * direction.y, 0) * Time.deltaTime * ConstValue.speedRatio;

        // ���� �̵� ����
        blackboard.myTransform.position += movement;

        // �ִϸ��̼� �Ǵ� ���� ��ȯ ó�� (�ʿ��� ���)
    }
}
