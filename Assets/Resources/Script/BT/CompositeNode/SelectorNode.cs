    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
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
            else if (childStatus == NodeStatus.Success)
            {
                // 현재 인덱스 초기화
                currentChildIndex = 0;
                return NodeStatus.Success;
            }
            else // Failure
            {
                currentChildIndex++;
            }
        }

        // 모든 자식 노드가 실패했을 때
        currentChildIndex = 0;
        return NodeStatus.Failure;
    }
}
