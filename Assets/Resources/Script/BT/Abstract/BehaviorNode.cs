using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeStatus { Success, Failure, Running }

public abstract class BehaviorNode
{
    public abstract NodeStatus Execute();
}