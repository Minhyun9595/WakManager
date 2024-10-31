using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageFont : MonoBehaviour
{
    public static GameObject Spawn(Vector3 position, int damage)
    {
        GameObject damageFont = PoolManager.Instance.GetFromPool(prefabName);
        damageFont.transform.position = position;
        damageFont.GetComponent<TextMeshPro>().text = damage.ToString();

        return damageFont;
    }

    public static string prefabName = EPrefabType.DamageFont.ToString();

    private TextMeshPro meshPro;

    public void Start()
    {
        meshPro = GetComponent<TextMeshPro>();
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