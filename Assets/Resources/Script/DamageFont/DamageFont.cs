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

        // �������� ���θ� Ȯ���ϰ� ���� ����
        if (damage % 1 == 0)
        {
            // ������ ��� �Ҽ��� ���� ���
            textMeshPro.text = string.Format("{0:0}", damage);
        }
        else
        {
            // �Ҽ��� ��� �Ҽ��� �� �ڸ����� ���
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