using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Blackboard
{
    public int teamIndex { get; set; }
    public GameObject myGameObject { get; set; }
    public Transform myTransform { get; set; }
    public Unit unitData { get; set; }
    public Unit_FieldData unitFieldInfo { get; set; }

    public Transform targetTransform { get; set; }
    public Blackboard targetBoard { get; set; }
    public Vector3 destination { get; set; }
    public string teamColor { get; set; }

    public Blackboard(int _teamIndex, int _unitIndex, GameObject _myGameObject) 
    {
        teamIndex = _teamIndex;
        var originalDataRef = DataTable.Instance.GetInfoByIndex(_unitIndex);
        if (originalDataRef == null)
        {
            Debug.LogError("originalDataRef is null : " + _unitIndex);
            new OnApplicationPause();
        }

        unitData = new Unit(originalDataRef);
        this.myGameObject = _myGameObject;
        this.myTransform = _myGameObject.transform;
        var nameText = myTransform.Find("NameText").GetComponent<TextMesh>();
        nameText.text = unitData.Name;
        teamColor = QUtility.UIUtility.GetTeamTextColor(teamIndex);
        unitFieldInfo = new Unit_FieldData(unitData);
    }

    public void Update(float deltaTime)
    {
        unitFieldInfo.Update(deltaTime);
    }
}

public class Unit_AI : MonoBehaviour
{
    private BehaviorNode rootNode;
    [SerializeField] private Blackboard blackboard;

    public static GameObject Spawn(Vector3 position, int teamIndex, int unitIndex)
    {
        GameObject unit = PoolManager.Instance.GetFromPool(EPrefabType.Unit.ToString());
        unit.transform.position = position;
        unit.GetComponent<Unit_AI>().Initialize(teamIndex, unitIndex);

        return unit;
    }

    public void Initialize(int _teamIndex, int _unitIndex)
    {
        blackboard = new Blackboard(_teamIndex, _unitIndex, this.gameObject);
        rootNode = CreateTankBehaviorTree();
    }

    BehaviorNode CreateTankBehaviorTree()
    {
        var deadAction = new DeadAction(blackboard);
        var findEnemyWarriorAction = new FindEnemyAction(blackboard);
        var moveToTargetAction = new MoveToTargetAction(blackboard);
        var attackAction = new AttackAction(blackboard);
        var idleAction = new IdleAction(blackboard);

        // 사망 시퀀스
        var deadSequence = new SequenceNode();
        deadSequence.AddChild(deadAction);

        // 공격 시퀀스
        var attackSequence = new SequenceNode();
        attackSequence.AddChild(moveToTargetAction);
        attackSequence.AddChild(attackAction);
        attackSequence.AddChild(findEnemyWarriorAction);
        attackSequence.AddChild(idleAction);

        var rootSelector = new SelectorNode();
        rootSelector.AddChild(deadSequence);
        rootSelector.AddChild(attackSequence);
        //rootSelector.AddChild(patrol);

        return rootSelector;
    }

    private void Invoke()
    {
        InvokeRepeating("CheckCone", 1, 1);
    }

    private void Update()
    {
        blackboard.Update(Time.deltaTime);

        if(rootNode != null)
        {
            rootNode.Execute();
        }
    }

    public Blackboard GetBlackboard()
    {
        return blackboard;
    }


    public float rayDistance = 5f;
    public LayerMask layerMask;   

    public float coneAngle = 60f; 
    public int rayCount = 10;     

    private void CheckCone()
    {
        float startAngle = -coneAngle / 2;    
        float angleStep = coneAngle / (rayCount - 1);

        HashSet<Collider2D> hitColliders = new HashSet<Collider2D>();

        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            float radian = (currentAngle + transform.eulerAngles.z) * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, rayDistance, layerMask);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != gameObject)
                {
                    hitColliders.Add(hit.collider);  // �ߺ� ����
                }
            }

            Color color = hits.Length > 1 ? Color.green : Color.red;
            Debug.DrawRay(transform.position, direction * rayDistance, color);
        }

        if (hitColliders.Count > 0)
        {

        }
    }

    private void CheckLine()
    {
        Vector2 direction = transform.right;
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + direction * rayDistance;

        RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, direction, rayDistance, layerMask);

        Color color = hits.Length > 1 ? Color.green : Color.red;
        Debug.DrawLine(startPosition, endPosition, color);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {

            }
        }
    }
}
