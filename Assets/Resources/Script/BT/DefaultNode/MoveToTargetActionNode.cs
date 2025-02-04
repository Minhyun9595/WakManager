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
            // Ÿ���� ������ ��ȯ
            return NodeStatus.Failure;
        }

        // Ÿ���� ��ġ ��������
        Vector3 targetPosition = blackboard.targetUnitAI.transform.position;
        Vector3 currentPosition = blackboard.myTransform.position;

        // Ÿ�ٰ��� �Ÿ� ���
        float distance = Vector3.Distance(currentPosition, targetPosition);

        // ��Ÿ��� ������ ǥ��
        var range = blackboard.realUnitData.GetRange();
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
        Vector3 delta = targetPosition - blackboard.myTransform.position;
        var moveSpeed_X = blackboard.realUnitData.unitStat.MoveSpeed_X;
        var moveSpeed_Y = blackboard.realUnitData.unitStat.MoveSpeed_Y;

        if (speedTrait != null)
        {
            moveSpeed_X *= speedTrait.Value1;
            moveSpeed_Y *= speedTrait.Value1;
        }

        // X�� Y ������ ���⿡ ���� �ӵ� ���
        float movementX = Mathf.Sign(delta.x) * moveSpeed_X * CustomTime.deltaTime * ConstValue.speedRatio;
        float movementY = Mathf.Sign(delta.y) * moveSpeed_Y * CustomTime.deltaTime * ConstValue.speedRatio;

        // �̵� �Ÿ� ����: ��ǥ ������ �ʰ����� �ʵ��� ����
        if (Mathf.Abs(movementX) > Mathf.Abs(delta.x)) movementX = delta.x;
        if (Mathf.Abs(movementY) > Mathf.Abs(delta.y)) movementY = delta.y;

        // �̵� ���� ���
        Vector3 movement = new Vector3(movementX, movementY, 0);

        // ���� �̵� ����
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
