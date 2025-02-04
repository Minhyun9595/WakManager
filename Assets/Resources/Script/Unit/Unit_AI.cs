using QUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Blackboard
{
    public int teamIndex { get; set; }
    public GameObject myGameObject { get; set; }
    public Unit_AI myUnitAI { get; set; }
    public Transform myTransform { get; set; }
    public Transform myBodyTransform { get; set; }
    public Renderer myBodyRenderer { get; set; }
    public TextMeshPro myNameText { get; set; }

    public UnitData realUnitData { get; set; }
    public Unit_FieldData unitFieldInfo { get; set; }
    public UnitAnimator unitAnimator { get; set; }
    public UnitReport unitReport { get; set; }
    public BehaviorNode rootNode { get; set; }


    public Unit_AI targetUnitAI { get; set; } // 현재 타겟
    public bool isAnimationPlaying { get; set; }
    public Vector3 destination { get; set; }

    public Blackboard(int _teamIndex, UnitData _unitData, GameObject _myGameObject) 
    {
        teamIndex = _teamIndex;

        this.myGameObject = _myGameObject;
        this.myTransform = _myGameObject.transform;
        this.myNameText = myTransform.Find("NameText").GetComponent<TextMeshPro>();
        this.myBodyTransform = myTransform.Find("Body").GetComponent<Transform>();
        this.myBodyRenderer = myTransform.Find("Body").GetComponent<Renderer>();
        this.myUnitAI = myTransform.GetComponent<Unit_AI>();
        this.unitAnimator = myBodyTransform.GetComponent<UnitAnimator>();
        this.unitReport = new UnitReport();
        realUnitData = UnitData.CopyNewUnit(_unitData);

        var name = realUnitData.unitInfo_Immutable.Name;
        myNameText.gameObject.SetActive(true);
        myNameText.text = name;
        myNameText.faceColor = UIUtility.GetTeamColor(teamIndex);
        myGameObject.name = $"{_teamIndex + 1}_{name}";

        unitFieldInfo = new Unit_FieldData(this, realUnitData);
        unitAnimator.InitAnimationController(this, realUnitData.unitInfo_Immutable.Animator);
    }

    public void Update(float deltaTime)
    {
        unitFieldInfo.Update(deltaTime);
        rootNode.Execute();
    }

    public List<DamageInfo> GetDamageList()
    {
        var MultiHitCount = realUnitData.unitStat.MultiHitCount;

        var damageList = new List<DamageInfo>();
        for (int i = 0; i < MultiHitCount; i++)
        {
            var myDamage = realUnitData.unitStat.Damage;
            var rand = UnityEngine.Random.Range(0, 9999);
            var isCritical = rand < realUnitData.unitStat.CriticalChance;

            if (isCritical)
            {
                myDamage *= 1 + (realUnitData.unitStat.CriticalRatio * ConstValue.CriticalDamageCoefficient);
            }

            var damageInfo = new DamageInfo();
            damageInfo.damage = myDamage;
            damageInfo.isCritical = isCritical;

            damageList.Add(damageInfo);
        }

        EditDamage_Trait_RangeUnit(ref damageList);

        return damageList;
    }

    public void EditDamage_Trait_RangeUnit(ref List<DamageInfo> damageInfos)
    {
        var IsRangeUnit = realUnitData.unitInfo_Immutable.CheckRangeUnit();
        DT_Trait dT_Trait = null;

        if (IsRangeUnit)
        {
            dT_Trait = realUnitData.GetTrait(TraitType.RangedDealer);
        }
        else
        {
            dT_Trait = realUnitData.GetTrait(TraitType.MeleeDealer);
        }

        if(dT_Trait != null)
        {
            foreach(var damageInfo in damageInfos)
            {
                damageInfo.damage *= (1.0f + (dT_Trait.Value2 * 0.01f));
            }
        }
    }

    public Renderer GetBodyRenderer()
    {
        return myBodyRenderer;
    }
}

public class Unit_AI : MonoBehaviour
{
    [SerializeField] public Blackboard blackboard;

    public Action<Unit_AI> OnDeath; // 유닛이 죽을 때 호출되는 이벤트
    private HashSet<int> subscribers = new HashSet<int>(); // 타겟으로 설정한 객체들
    public LayerMask layerMask;

    public static GameObject TestSpawn(Vector3 _position, int _teamIndex, int _unitIndex)
    {
        UnitData testUnitData = UnitData.CreateNewUnit(EUnitTier.Silver, _unitIndex);
        GameObject unit = PoolManager.Instance.GetFromPool(EPrefabType.Unit.ToString());
        unit.transform.position = _position;
        unit.GetComponent<Unit_AI>().Initialize(_teamIndex, testUnitData);

        return unit;
    }

    public static GameObject Spawn(Vector3 _position, int _teamIndex, UnitData _unitData)
    {
        GameObject unit = PoolManager.Instance.GetFromPool(EPrefabType.Unit.ToString());
        unit.transform.position = _position;
        unit.GetComponent<Unit_AI>().Initialize(_teamIndex, _unitData);

        return unit;
    }

    public void Initialize(int _teamIndex, UnitData _unitData)
    {
        blackboard = new Blackboard(_teamIndex, _unitData, this.gameObject);
        blackboard.rootNode = CreateTankBehaviorTree();
    }

    BehaviorNode CreateTankBehaviorTree()
    {
        var rootSelectorNode = new SelectorNode();
        // 사망 시퀀스 (최우선)
        var dead_SequenceNode = new SequenceNode();
        var deadAction = new DeadActionNode(blackboard);
        dead_SequenceNode.AddChild(deadAction);
        rootSelectorNode.AddChild(dead_SequenceNode);

        // 행동 시퀀스
        var Default_Sequence = new SequenceNode();
        var Default_Selector = new SelectorNode();
        var AttackEnemyAction_Sequence = new SequenceNode();

        // 1. 타겟 탐색: 0.3초마다 타겟을 갱신
        AttackEnemyAction_Sequence.AddChild(new FindTargetConditionNode(blackboard));
        // 2. 타겟이 범위 내에 있는지 확인
        var AttackAction_Selector = new SelectorNode();
        var Attack_Sequence = new SequenceNode();
        Attack_Sequence.AddChild(new IsEnemyInRangeConditionNode(blackboard));
        // 3. 타겟이 범위 내에 있으면 공격, 아니면 이동
        Attack_Sequence.AddChild(new AttackActionNode(blackboard));
        AttackAction_Selector.AddChild(Attack_Sequence);
        AttackAction_Selector.AddChild(new MoveToTargetActionNode(blackboard));
        AttackEnemyAction_Sequence.AddChild(AttackAction_Selector);
        Default_Selector.AddChild(AttackEnemyAction_Sequence);
        // 타겟이 없거나 모든 행동 실패 시 대기
        Default_Selector.AddChild(new IdleActionNode(blackboard));
        Default_Sequence.AddChild(Default_Selector);

        rootSelectorNode.AddChild(Default_Sequence);

        return rootSelectorNode;
    }

    private void Update()
    {
        if(blackboard  != null)
        {
            blackboard.Update(CustomTime.deltaTime);
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
        var myUnitData = blackboard.realUnitData;

        var attackType = myUnitData.unitInfo_Immutable.AttackType;
        var myDamageType = myUnitData.GetDamageType();

        if (attackType == "Melee")
        {
            var damageList = GetDamageList();

            if (blackboard.targetUnitAI != null)
            {
                var hitDamageList = blackboard.targetUnitAI.blackboard.unitFieldInfo.Hit(myDamageType, damageList, blackboard.targetUnitAI.transform.position);
                foreach (var hitDamage in hitDamageList)
                {
                    blackboard.unitReport.AddDamage(myDamageType, hitDamage);
                }

                blackboard.unitFieldInfo.AttackActionResetCoolTime();
            }
        }
        else if (attackType == "Projectile")
        {
            var attackPrefabName = myUnitData.unitInfo_Immutable.AttackPrefabName;
            if (attackPrefabName == "Projectile_Wizzard")
            {
                Projectile_Straight.Spawn_Straight(attackPrefabName, this, transform.position, blackboard.targetUnitAI.transform.position, 2.0f);
            }
        }
    }

    public List<DamageInfo> GetDamageList()
    {
        return blackboard.GetDamageList();
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

public class DamageInfo
{
    public float damage { get; set; }
    public bool isCritical { get; set; }

    public DamageInfo() { }
    public DamageInfo(float damage, bool isCritical)
    {
        this.damage = damage;
        this.isCritical = isCritical;
    }
}