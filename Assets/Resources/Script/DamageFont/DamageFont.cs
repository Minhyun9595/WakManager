using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageFont : MonoBehaviour
{
    public static string prefabName = EPrefabType.DamageFont.ToString();
    public static GameObject Spawn(Vector3 position, int damage, Color color, float _delayTime = 0.0f)
    {
        GameObject damageFont = PoolManager.Instance.GetFromPool(prefabName);
        damageFont.transform.position = position;
        var textMeshPro = damageFont.GetComponent<TextMeshPro>();
        textMeshPro.text = ((int)damage).ToString();
        textMeshPro.color = color;
        damageFont.GetComponent<DamageFont>().Invoke_PlayDamageFont(_delayTime);

        return damageFont;
    }

    public static GameObject Spawn(Vector3 position, float damage, Color color, float _delayTime = 0.0f)
    {
        GameObject damageFont = PoolManager.Instance.GetFromPool(prefabName);
        damageFont.transform.position = position;
        var textMeshPro = damageFont.GetComponent<TextMeshPro>();

        // 정수인지 여부를 확인하고 포맷 설정
        if (damage % 1 == 0)
        {
            // 정수일 경우 소수점 없이 출력
            textMeshPro.text = string.Format("{0:0}", damage);
        }
        else
        {
            // 소수일 경우 소수점 두 자리까지 출력
            //textMeshPro.text = string.Format("{0:0.00}", damage);
            textMeshPro.text = string.Format("{0:0}", damage);
        }

        textMeshPro.color = color;
        damageFont.GetComponent<DamageFont>().Invoke_PlayDamageFont(_delayTime);

        return damageFont;
    }


    private Animator animator;
    private TextMeshPro textMeshPro;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        textMeshPro = GetComponent<TextMeshPro>();
        animator.enabled = false;
    }

    public void Invoke_PlayDamageFont(float _delayTime)
    {
        textMeshPro.enabled = false;
        animator.enabled = false;
        if (0 < _delayTime)
        {
            Invoke("PlayDamageFont", _delayTime);
        }
        else
        {
            PlayDamageFont();
        }
    }

    public void PlayDamageFont()
    {
        animator.enabled = true;
        textMeshPro.enabled = true;
        animator.Play(prefabName);
        animator.speed = CustomTime.timeScale;
    }

    public void AnimationEnd()
    {
        PoolManager.Instance.ReturnToPool(prefabName, this.gameObject);
    }
}