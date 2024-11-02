using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    public static string prefabName = EPrefabType.Projectile.ToString();
    public GameObject Spawn_Straight(Unit_AI ownerUnitAI, Vector3 _startPosition, Vector3 _direction)
    {
        GameObject projectileObject = PoolManager.Instance.GetFromPool(prefabName);
        projectileObject.transform.position = _startPosition;
        var projectile = projectileObject.GetComponent<Projectile>();
        projectile.SetProjectile(ownerUnitAI, _direction);
        return projectileObject;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetProjectile(Unit_AI ownerUnitAI, Vector3 _direction)
    {

    }
}
