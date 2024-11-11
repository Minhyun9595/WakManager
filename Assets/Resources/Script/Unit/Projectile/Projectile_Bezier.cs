using QUtility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile_Bezier : ProjectileAbstract
{
    public static GameObject Spawn_Bezier(string prefabName, Unit_AI ownerUnitAI, Unit_AI targetUnitAI, Vector3 start, Vector3 _endPosition, 
        float control1Range, float control2Range, float _life, float _startDelayTime)
    {
        var projectileName = prefabName;
        var control1 = MathUtility.GetRandomVector3(-control1Range, control1Range, -control1Range, control1Range);
        var control2 = MathUtility.GetRandomVector3(-control2Range, control2Range, -control2Range, control2Range);

        GameObject projectileObject = PoolManager.Instance.GetFromPool(projectileName);
        Projectile_Bezier projectile = projectileObject.GetComponent<Projectile_Bezier>();
        projectile.SetPrefabName(projectileName);
        projectile.SetBezierPath(ownerUnitAI, targetUnitAI, start, control1, control2, _endPosition, _life);
        projectile.SetStartDelay(_startDelayTime);

        return projectileObject;
    }

    public Vector3 startPosition;
    public Vector3 controlPoint1;
    public Vector3 controlPoint2;
    public Vector3 endPosition;
    public Unit_AI targetAI;
    public Transform targetTransform; // 타겟을 Transform으로 받아서 항상 추적
    public float life;
    private float time;  // 이동을 위한 시간 값 (0 ~ 1 사이)

    private void Awake()
    {
        prefabName = EPrefabType.Projectile_Bezier.ToString();
    }

    private void Start()
    {
        time = 0f;
    }

    private void Update()
    {
        if (StartDelayUpdate() == false)
            return;
        
        if (targetTransform != null)
        {
            endPosition = targetTransform.position;
        }

        time += Time.deltaTime;

        if (time > life)
        {
            if (targetAI != null)
            {
                EPrefabType ePrefabType = EPrefabType.Projectile_Bezier_Hit;
                EDamageType eDamageType = EDamageType.Magical;
                List<DamageInfo> damageInfos = new List<DamageInfo>();
                damageInfos.Add(new DamageInfo(10, false));
                var result = Attack(targetAI, ePrefabType, eDamageType, damageInfos);
            }

            // 끝에 도달하면 해당 투사체를 반환
            if (targetTransform != null)
            {
                // 이벤트 제거
                targetAI.OnDeath -= HandleTargetDeath;
                targetAI = null;
                targetTransform = null;
            }

            PoolManager.Instance.ReturnToPool(prefabName, gameObject);
            return;
        }

        // 베지어 곡선 경로를 따라 현재 위치 계산
        transform.position = GetPointOnCubicBezier(startPosition, controlPoint1, controlPoint2, endPosition, time);

        // 투사체의 방향을 이동 방향으로 회전
        RotateProjectileAlongPath();
    }

    // 3차 베지어 곡선의 현재 위치 계산
    private Vector3 GetPointOnCubicBezier(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end, float t)
    {
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * oneMinusT * start +
               3f * oneMinusT * oneMinusT * t * control1 +
               3f * oneMinusT * t * t * control2 +
               t * t * t * end;
    }

    private void RotateProjectileAlongPath()
    {
        // 다음 위치를 계산하여 투사체의 회전 설정
        float deltaTime = 0.01f;
        Vector3 nextPosition = GetPointOnCubicBezier(startPosition, controlPoint1, controlPoint2, endPosition, Mathf.Clamp01(time + deltaTime));
        Vector3 direction = (nextPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 투사체의 경로를 설정하는 함수
    public void SetBezierPath(Unit_AI _ownerUnitAI, Unit_AI targetUnitAI, Vector3 _startPosition, Vector3 _controlPoint1, Vector3 _controlPoint2, Vector3 _endPosition, float _life)
    {
        Init(_ownerUnitAI);
        targetAI = targetUnitAI;
        targetTransform = targetUnitAI.blackboard.myTransform;
        targetUnitAI.OnDeath += HandleTargetDeath;
        // 타겟의 Death 이벤트에 넣기

        startPosition = _startPosition;
        controlPoint1 = _controlPoint1;
        controlPoint2 = _controlPoint2;
        endPosition = _endPosition;
        life = _life;
        time = 0f; // 초기화

        // 투사체 초기 위치 설정
        transform.position = startPosition;

        // 초기 방향 설정
        RotateProjectileAlongPath();
    }

    private void HandleTargetDeath(Unit_AI target)
    {
        targetAI = null;
        targetTransform = null;
    }
}