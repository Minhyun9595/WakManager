using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : BehaviorNode
{
    protected List<BehaviorNode> children = new List<BehaviorNode>();

    public void AddChild(BehaviorNode child)
    {
        children.Add(child);
    }
}
