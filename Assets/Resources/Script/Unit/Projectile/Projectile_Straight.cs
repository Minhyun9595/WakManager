using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile_Straight : ProjectileAbstract
{

    public static GameObject Spawn_Straight(string prefabName, Unit_AI _ownerUnitAI, Vector3 _startPosition, Vector3 _targetPosition, float _speed)
    {
        var projectileName = prefabName;

        GameObject projectileObject = PoolManager.Instance.GetFromPool(projectileName);
        projectileObject.transform.position = _startPosition;
        var projectile = projectileObject.GetComponent<Projectile_Straight>();

        Vector3 calculatedDirection = (_targetPosition - _startPosition).normalized;
        projectile.SetPrefabName(projectileName);
        projectile.SetProjectile(_ownerUnitAI, calculatedDirection, _speed);
        return projectileObject;
    }

    public Vector3 normalizeDirection = Vector3.zero;
    public float speed;

    void Start()
    {
    }

    void Update()
    {
        if (StartDelayUpdate() == false)
            return;

        transform.Translate(normalizeDirection * speed * Time.deltaTime, Space.World);
    }

    public void SetProjectile(Unit_AI _ownerUnitAI, Vector3 _normalizeDirection, float _speed)
    {
        Init(_ownerUnitAI);
        normalizeDirection = _normalizeDirection;
        speed = _speed;
        RotateProjectile();
    }

    private void RotateProjectile()
    {
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

        var blackboard = ownerUnitAI.blackboard;
        var damageList = ownerUnitAI.GetDamageList();
        var myDamageType = blackboard.realUnitData.GetDamageType();

        targetUnitAI.blackboard.unitFieldInfo.Hit(myDamageType, damageList, targetUnitAI.transform.position);
        blackboard.unitFieldInfo.AttackActionResetCoolTime();
        ProjectileEffect.Spawn(EPrefabType.Projectile_Wizzard_Hit.ToString(), targetUnitAI.transform.position);
        PoolManager.Instance.ReturnToPool(prefabName, gameObject);
    }
}
