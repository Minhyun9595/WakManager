using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileAbstract : MonoBehaviour
{
    public static string prefabName = EPrefabType.Projectile_Wizzard.ToString();

    public Vector3 normalizeDirection = Vector3.zero;
    public float speed;
    public int teamIndex;
    public Unit_AI ownerUnitAI;

    public static GameObject Spawn_Straight(Unit_AI _ownerUnitAI, Vector3 _startPosition, Vector3 _targetPosition, float _speed)
    {
        GameObject projectileObject = PoolManager.Instance.GetFromPool(prefabName);
        projectileObject.transform.position = _startPosition;
        var projectile = projectileObject.GetComponent<ProjectileAbstract>();

        Vector3 calculatedDirection = (_targetPosition - _startPosition).normalized;
        projectile.SetProjectile(_ownerUnitAI, calculatedDirection, _speed);
        return projectileObject;
    }

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(normalizeDirection * speed * Time.deltaTime, Space.World);
    }

    public void SetProjectile(Unit_AI _ownerUnitAI, Vector3 _normalizeDirection, float _speed)
    {
        ownerUnitAI = _ownerUnitAI;
        normalizeDirection = _normalizeDirection;
        speed = _speed;
        teamIndex = _ownerUnitAI.blackboard.teamIndex;
        RotateProjectile();
    }

    private void RotateProjectile()
    {
        // 2D 이동 방향에 따라 투사체가 바라보는 방향 설정
        float angle = Mathf.Atan2(normalizeDirection.y, normalizeDirection.x) * Mathf.Rad2Deg + 180;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Unit")
            return;

        var targetUnitAI = collision.gameObject.GetComponent<Unit_AI>();

        if (targetUnitAI == null)
            return;

        if (targetUnitAI.blackboard.teamIndex == teamIndex)
            return;

        if (targetUnitAI.blackboard.unitFieldInfo.IsDead())
            return;

        // 데미지를 꺼내서 공격
        var blackboard = ownerUnitAI.blackboard;
        var damageList = ownerUnitAI.GetDamageList();
        var myDamageType = blackboard.unitData.GetDamageType();

        targetUnitAI.blackboard.unitFieldInfo.Hit(myDamageType, damageList, blackboard.targetUnitAI.transform.position);
        blackboard.unitFieldInfo.Attack();
        ProjectileEffect.Spawn(EPrefabType.Projectile_Wizzard_Hit.ToString(), blackboard.targetUnitAI.transform.position);
        PoolManager.Instance.ReturnToPool(prefabName, gameObject);
    }
}
