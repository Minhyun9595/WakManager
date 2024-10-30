using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeStatus { Success, Failure, Running }

public abstract class BehaviorNode
{
    public abstract NodeStatus Execute();
}

public abstract class ActionNode : BehaviorNode
{
    // 필요에 따라 공통적으로 사용할 변수나 메서드를 정의할 수 있습니다.

    // 추상 메서드 Execute()는 BehaviorNode에서 상속받습니다.
    // 여기서는 별도의 구현이 필요하지 않습니다.
}