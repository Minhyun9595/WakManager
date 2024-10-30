using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Blackboard
{
    // �ڽ��� ����
    public GameObject myGameObject;
    public Transform myTransform;
    public Unit unitInfo;
    public int teamIndex;

    // 
    public Transform target;     // ��ǥ�� �ϴ� ��� (Transform)
    public Vector3 destination;  // Ư�� ��ġ�� ��ǥ�� �� ��� ��� (���� ����)
    public string teamColor;

    public Blackboard(int _teamIndex, int _unitIndex, GameObject _myGameObject) 
    {
        teamIndex = _teamIndex;
        var originalDataRef = DataTable.Instance.GetInfoByIndex(_unitIndex);
        if (originalDataRef == null)
        {
            Debug.LogError(_unitIndex);
            new OnApplicationPause();
        }

        unitInfo = new Unit(originalDataRef);
        this.myGameObject = _myGameObject;
        this.myTransform = _myGameObject.transform;
        var nameText = myTransform.Find("NameText").GetComponent<TextMesh>();
        nameText.text = unitInfo.Name;
        teamColor = "#ff0000";
        if(_teamIndex == 1)
        {
            teamColor = "#00ff00";
        }
    }
}

public class Unit_AI : MonoBehaviour
{
    private BehaviorNode rootNode;
    private Blackboard blackboard;

    private void Start()
    {

    }

    public void Initialize(int _teamIndex, int _unitIndex)
    {
        blackboard = new Blackboard(_teamIndex, _unitIndex, this.gameObject);
        rootNode = CreateTankBehaviorTree();
    }

    BehaviorNode CreateTankBehaviorTree()
    {
        var findEnemyWarriorAction = new FindEnemyAction(blackboard);
        var moveToTargetAction = new MoveToTargetAction(blackboard);
        //var attack = new AttackAction();
        var idleAction = new IdleAction(blackboard);

        var attackSequence = new SequenceNode();
        attackSequence.AddChild(findEnemyWarriorAction);
        attackSequence.AddChild(moveToTargetAction);
        attackSequence.AddChild(idleAction);

        var rootSelector = new SelectorNode();
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
        rootNode.Execute();
        //CheckCone();
        //CheckLine();
    }


    public Blackboard GetBlackboard()
    {
        return blackboard;
    }


    public float rayDistance = 5f;   // Ray�� ����
    public LayerMask layerMask;      // �浹�� ���̾� ����

    public float coneAngle = 60f;          // ��ä�� ���� (��ü ����)
    public int rayCount = 10;              // �߻��� Ray ����

    private void CheckCone()
    {
        float startAngle = -coneAngle / 2;     // ��ä�� ���� ����
        float angleStep = coneAngle / (rayCount - 1); // �� Ray ���� ���� ����

        HashSet<Collider2D> hitColliders = new HashSet<Collider2D>();

        for (int i = 0; i < rayCount; i++)
        {
            // ������ �������� ���� ���
            float currentAngle = startAngle + (angleStep * i);
            float radian = (currentAngle + transform.eulerAngles.z) * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

            // RaycastAll�� ����Ͽ� ��� �浹ü ����
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, rayDistance, layerMask);

            // ��� �浹ü �˻� �� �ð�ȭ
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

        // ��ä�� ���� ������ �浹�� ������Ʈ�� �ִ��� �Ǵ�
        if (hitColliders.Count > 0)
        {
            //Debug.Log($"Total unique objects hit in cone: {hitColliders.Count}");
        }
    }

    private void CheckLine()
    {
        Vector2 direction = transform.right; // ������Ʈ�� ������(�ٶ󺸴� ����)
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + direction * rayDistance;

        RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, direction, rayDistance, layerMask);

        Color color = hits.Length > 1 ? Color.green : Color.red;
        Debug.DrawLine(startPosition, endPosition, color);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                //Debug.Log($"Hit {hit.collider.name} at distance {hit.distance}");
            }
        }
    }
}
