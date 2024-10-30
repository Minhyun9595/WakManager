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
                // 현재 인덱스 초기화
                currentChildIndex = 0;
                return NodeStatus.Failure;
            }
            else // Success
            {
                currentChildIndex++;
            }
        }

        // 모든 자식 노드가 성공했을 때
        currentChildIndex = 0;
        return NodeStatus.Success;
    }
}
