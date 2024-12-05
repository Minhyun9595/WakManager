using System.Collections;
using System.Collections.Generic;
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

    public static GameObject SpawnEffect(string effectAnimatorName, Vector3 _startPosition)
    {
        var _prefabName = "Effect";
        GameObject effectObject = PoolManager.Instance.GetFromPool(_prefabName);

        if (effectObject != null)
        {
            effectObject.transform.position = _startPosition;
            var projectileEffect = effectObject.GetComponent<ProjectileEffect>();
            projectileEffect.prefabName = _prefabName;
            projectileEffect.SetAnimator(effectAnimatorName);
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
        animator.speed = CustomTime.timeScale;
        waitUntil = new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        StartCoroutine(CheckAnimationEnd());
    }

    void Update()
    {
        
    }

    public void SetAnimator(string effectAnimatorName)
    {
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>($"Animation/Effect/{effectAnimatorName}");
        if(controller != null)
        {
            animator.runtimeAnimatorController = controller;
        }
    }

    private IEnumerator CheckAnimationEnd()
    {
        yield return waitUntil;
        PoolManager.Instance.ReturnToPool(prefabName, gameObject);
    }
}
