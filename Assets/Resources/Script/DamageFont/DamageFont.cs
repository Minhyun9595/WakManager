using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageFont : MonoBehaviour
{
    public static GameObject Spawn(Vector3 position, int damage, Color color)
    {
        GameObject damageFont = PoolManager.Instance.GetFromPool(prefabName);
        damageFont.transform.position = position;
        var textMeshPro = damageFont.GetComponent<TextMeshPro>();
        textMeshPro.text = ((int)damage).ToString();
        textMeshPro.color = color;

        return damageFont;
    }

    public static GameObject Spawn(Vector3 position, float damage, Color color)
    {
        GameObject damageFont = PoolManager.Instance.GetFromPool(prefabName);
        damageFont.transform.position = position;
        var textMeshPro = damageFont.GetComponent<TextMeshPro>();
        textMeshPro.text =((int)damage).ToString();
        textMeshPro.color = color;

        return damageFont;
    }

    public static string prefabName = EPrefabType.DamageFont.ToString();

    private TextMeshPro textMeshPro;

    public void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
    }

    public void OnEnable()
    {
        gameObject.GetComponent<Animator>().Play(prefabName);
    }

    public void AnimationEnd()
    {
        PoolManager.Instance.ReturnToPool(prefabName, this.gameObject);
    }
}