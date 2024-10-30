using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : CompositeNode
{
    private int currentChildIndex = 0;

    public override NodeStatus Execute()
    {
        while (currentChildIndex < children.Count)
        {
            NodeStatus childStatus = children[currentChildIndex].Execute();

            if (childStatus == NodeStatus.Running)
            {
                return NodeStatus.Running;
            }
            else if (childStatus == NodeStatus.Failure)
            {
                // ���� �ε��� �ʱ�ȭ
                currentChildIndex = 0;
                return NodeStatus.Failure;
            }
            else // Success
            {
                currentChildIndex++;
            }
        }

        // ��� �ڽ� ��尡 �������� ��
        currentChildIndex = 0;
        return NodeStatus.Success;
    }
}
