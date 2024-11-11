using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileEffect : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    WaitUntil waitUntil;
    public string prefabName;

    public static GameObject Spawn(string _prefabName, Vector3 _startPosition)
    {
        GameObject effectObject = PoolManager.Instance.GetFromPool(_prefabName);

        if(effectObject != null)
        {
            effectObject.transform.position = _startPosition;
            var projectileEffect = effectObject.GetComponent<ProjectileEffect>();
            projectileEffect.prefabName = _prefabName;
        }

        return effectObject;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = ConstValue.Layer_Effect;
    }
    void OnEnable()
    {
        animator.Play("Effect");
        waitUntil = new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        StartCoroutine(CheckAnimationEnd());
    }

    void Update()
    {
        
    }

    private IEnumerator CheckAnimationEnd()
    {
        yield return waitUntil;
        PoolManager.Instance.ReturnToPool(prefabName, gameObject);
    }
}
