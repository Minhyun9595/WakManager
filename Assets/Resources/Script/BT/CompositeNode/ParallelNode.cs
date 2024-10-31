using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelNode : CompositeNode
{
    private int successThreshold;
    private int failureThreshold;

    public ParallelNode(int successThreshold, int failureThreshold)
    {
        this.successThreshold = successThreshold;
        this.failureThreshold = failureThreshold;
    }

    public override NodeStatus Execute()
    {
        int successCount = 0;
        int failureCount = 0;

        foreach (var child in children)
        {
            NodeStatus childStatus = child.Execute();

            if (childStatus == NodeStatus.Success)
            {
                successCount++;
                if (successCount >= successThreshold)
                {
                    return NodeStatus.Success;
                }
            }
            else if (childStatus == NodeStatus.Failure)
            {
                failureCount++;
                if (failureCount >= failureThreshold)
                {
                    return NodeStatus.Failure;
                }
            }
        }

        return NodeStatus.Running;
    }
}
