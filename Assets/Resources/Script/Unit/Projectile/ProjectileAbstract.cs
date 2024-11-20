using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ProjectileInterface
{
    public void Init(Unit_AI _ownerAI);
    public void SetStartDelay(float _delayTime);
    public bool StartDelayUpdate();
}

public abstract class ProjectileAbstract : MonoBehaviour, ProjectileInterface
{
    public string prefabName;
    public bool useStartDelay;
    public float startDelayTime;
    public int teamIndex;

    public Unit_AI ownerUnitAI;
    public Collider2D myCollider2D;
    public SpriteRenderer spriteRenderer;
    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = ConstValue.Layer_Effect;
    }

    public void Init(Unit_AI _ownerAI)
    {
        ownerUnitAI = _ownerAI;
        teamIndex = ownerUnitAI.blackboard.teamIndex;
        myCollider2D = GetComponent<Collider2D>();
    }

    public void SetPrefabName(string _prefabName)
    {
        prefabName = _prefabName;
    }

    public void SetStartDelay(float _delayTime)
    {
        startDelayTime = _delayTime;
        useStartDelay = true;
        myCollider2D.enabled = false;
    }

    public bool StartDelayUpdate()
    {
        if (useStartDelay)
        {
            if (0 <= startDelayTime)
            {
                startDelayTime -= ConstValue.DeltaTimeValue;
                return false;
            }
            else
            {
                startDelayTime = 0;
                myCollider2D.enabled = true;
                useStartDelay = false;
            }
        }

        return true;
    }

    public bool Attack(Unit_AI targetUnitAI, EPrefabType ePrefabType, EDamageType eDamageType, List<DamageInfo> damageInfoList)
    {
        if (targetUnitAI == null)
            return false;

        if (targetUnitAI.blackboard.teamIndex == teamIndex)
            return false;

        if (targetUnitAI.blackboard.unitFieldInfo.IsDead())
            return false;

        targetUnitAI.blackboard.unitFieldInfo.Hit(eDamageType, damageInfoList, targetUnitAI.transform.position);
        ProjectileEffect.Spawn(ePrefabType.ToString(), targetUnitAI.transform.position);

        return true;
    }
}
