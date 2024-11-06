using QUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Blackboard
{
    public int teamIndex { get; set; }
    public GameObject myGameObject { get; set; }
    public Unit_AI myUnitAI { get; set; }
    public Transform myTransform { get; set; }
    public Transform myBodyTransform { get; set; }
    public TextMeshPro myNameText { get; set; }
    public DT_Unit unitData { get; set; }
    public Unit_FieldData unitFieldInfo { get; set; }
    public UnitAnimator unitAnimator { get; set; }

    public Unit_AI targetUnitAI { get; set; } // 현재 타겟을 저장하는 속성
    public bool isAnimationPlaying { get; set; }
    public Vector3 destination { get; set; }

    public Blackboard(int _teamIndex, int _unitIndex, GameObject _myGameObject) 
    {
        teamIndex = _teamIndex;
        var originalDataRef = DT_Unit.GetInfoByIndex(_unitIndex);
        if (originalDataRef == null)
        {
            Debug.LogError("originalDataRef is null : " + _unitIndex);
            new OnApplicationPause();
        }

        this.myGameObject = _myGameObject;
        this.myTransform = _myGameObject.transform;
        this.myNameText = myTransform.Find("NameText").GetComponent<TextMeshPro>();
        this.myBodyTransform = myTransform.Find("Body").GetComponent<Transform>();
        this.myUnitAI = myTransform.GetComponent<Unit_AI>();
        this.unitAnimator = myBodyTransform.GetComponent<UnitAnimator>();

        unitData = new DT_Unit(originalDataRef);
        myNameText.gameObject.SetActive(true);
        myNameText.text = unitData.Name;
        myNameText.faceColor = UIUtility.GetTeamColor(teamIndex);

        unitFieldInfo = new Unit_FieldData(this, unitData);
        unitAnimator.InitAnimationController(this, unitData.Animation);
    }

    public void Update(float deltaTime)
    {
        if (isAnimationPlaying)
            return;

        unitFieldInfo.Update(deltaTime);
    }
}

public class Unit_AI : MonoBehaviour
{
    private BehaviorNode rootNode;
    [SerializeField] public Blackboard blackboard;

    public Action<Unit_AI> OnDeath; // 유닛이 죽을 때 호출되는 이벤트
    private HashSet<int> subscribers = new HashSet<int>(); // 타겟으로 설정한 객체들
    public LayerMask layerMask;

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
        gameObject.name = $"{_teamIndex + 1}_{blackboard.unitData.Name}";
    }

    BehaviorNode CreateTankBehaviorTree()
    {
        var rootSelector = new SelectorNode();
        var deadAction = new DeadAction(blackboard);
        var findTargetAction = new FindTargetAction(blackboard);
        var moveToTargetAction = new MoveToTargetAction(blackboard);
        var attackAction = new AttackAction(blackboard);
        var idleAction = new IdleAction(blackboard);

        // 사망 시퀀스
        var deadSequence = new SequenceNode();
        deadSequence.AddChild(deadAction);

        rootSelector.AddChild(deadSequence);

        // 스킬 셀렉터: 스킬 쿨타임을 검사하여 스킬을 사용
        var skillSelector = new SelectorNode();
        if (blackboard != null)
        {
            foreach (var skillName in blackboard.unitData.SkillNameList)
            {
                var skillSequence = new SequenceNode();

                if (skillName == "Sirian_Skill_Node")
                {
                    var coolDownNode = new CooldownNode(blackboard, 4); // 쿨타임 설정
                    var sirianSkillNode = new Sirian_Skill_Node(blackboard);

                    skillSequence.AddChild(coolDownNode);  // 쿨타임이 되면
                    skillSequence.AddChild(sirianSkillNode); // 스킬 실행
                }

                skillSelector.AddChild(skillSequence);
            }
        }

        // 공격 시퀀스
        var attackSequence = new SequenceNode();
        attackSequence.AddChild(findTargetAction);
        attackSequence.AddChild(moveToTargetAction);
        attackSequence.AddChild(attackAction);

        rootSelector.AddChild(attackSequence);

        // 대기 시퀀스
        var idleSequence = new SequenceNode();
        idleSequence.AddChild(idleAction);

        rootSelector.AddChild(idleSequence);

        // Parallel 시퀀스
        var rootParallel = new ParallelNode(99, 99);
        rootParallel.AddChild(skillSelector);
        rootParallel.AddChild(rootSelector);

        return rootParallel;
    }

    private void Update()
    {
        if(blackboard  != null)
        {
            blackboard.Update(Time.deltaTime);
        }

        if(blackboard != null && blackboard.isAnimationPlaying == false && rootNode != null)
        {
            rootNode.Execute();
        }
    }

    public Blackboard GetBlackboard()
    {
        return blackboard;
    }

    public void Die()
    {
        blackboard.myNameText.gameObject.SetActive(false);
        OnDeath?.Invoke(this); // 자신을 타겟으로 한 유닛들에게 알림
    }

    public void SetTarget(Unit_AI targetUnitAI)
    {
        if (blackboard.targetUnitAI != null)
        {
            blackboard.targetUnitAI.OnDeath -= HandleTargetDeath;
            blackboard.targetUnitAI = null;
        }

        blackboard.targetUnitAI = targetUnitAI;
        blackboard.targetUnitAI.OnDeath += HandleTargetDeath;
    }

    private void HandleTargetDeath(Unit_AI target)
    {
        // 내 타겟이 죽었으면 null 처리
        if (blackboard.targetUnitAI == target)
        {
            blackboard.targetUnitAI.OnDeath -= HandleTargetDeath;
            blackboard.targetUnitAI = null;
        }
    }

    public void AddSubscriber(Unit_AI subscriber)
    {
        var id = subscriber.gameObject.GetInstanceID();
        if (subscribers.Contains(id) == false) // 구독 중복 방지
        {
            subscribers.Add(id);
            OnDeath += subscriber.HandleTargetDeath;
        }
        Debug.Log(subscribers.Count);
    }

    public void RemoveSubscriber(Unit_AI subscriber)
    {
        var id = subscriber.gameObject.GetInstanceID();
        if (subscribers.Contains(id))
        {
            subscribers.Remove(id);
            OnDeath -= subscriber.HandleTargetDeath;
        }
    }

    public void Attack1()
    {
        var myUnitData = blackboard.unitData;

        var type = myUnitData.GetAttackType(0);

        if (type == "Melee")
        {
            var myDamageType = myUnitData.GetDamageType();
            var damageList = GetDamageList();

            if (blackboard.targetUnitAI != null)
            {
                blackboard.targetUnitAI.blackboard.unitFieldInfo.Hit(myDamageType, damageList, blackboard.targetUnitAI.transform.position);
                blackboard.unitFieldInfo.Attack();
            }
        }
        else if (type == "Projectile")
        {
            var subType = myUnitData.GetAttackType(1);
            if (subType == "Projectile_Wizzard")
            {
                Projectile_Straight.Spawn_Straight(subType, this, transform.position, blackboard.targetUnitAI.transform.position, 2.0f);
            }
        }
    }

    public List<DamageInfo> GetDamageList()
    {
        var myUnitData = blackboard.unitData;
        var myDamageCount = myUnitData.MultiHitCount;

        var damageList = new List<DamageInfo>();
        for (int i = 0; i < myDamageCount; i++)
        {
            var myDamage = myUnitData.Damage;
            //var isCritical = myUnitData.IsCritical();
            var rand = UnityEngine.Random.Range(0, 9999);
            var isCritical = rand < myUnitData.CriticalChance;

            if (isCritical)
            {
                myDamage *= 1 + (myUnitData.CriticalRatio * ConstValue.CriticalDamageCoefficient);
            }

            var damageInfo = new DamageInfo();
            damageInfo.damage = myDamage;
            damageInfo.isCritical = isCritical;

            damageList.Add(damageInfo);
        }

        return damageList;
    }

    private void CheckCone(float _rayDistance = 5.0f, float _coneAngle = 60.0f, int _rayCount = 10)
    {
        float startAngle = -_coneAngle / 2;    
        float angleStep = _coneAngle / (_rayCount - 1);

        HashSet<Collider2D> hitColliders = new HashSet<Collider2D>();

        for (int i = 0; i < _rayCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            float radian = (currentAngle + transform.eulerAngles.z) * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, _rayDistance, layerMask);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != gameObject)
                {
                    hitColliders.Add(hit.collider);  // �ߺ� ����
                }
            }

            Color color = hits.Length > 1 ? Color.green : Color.red;
            Debug.DrawRay(transform.position, direction * _rayDistance, color);
        }

        if (hitColliders.Count > 0)
        {

        }
    }

    private void CheckLine(float _rayDistance = 5.0f)
    {
        Vector2 direction = transform.right;
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + direction * _rayDistance;

        RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, direction, _rayDistance, layerMask);

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
