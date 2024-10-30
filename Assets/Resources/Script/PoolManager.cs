using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : CustomSingleton<PoolManager>
{
    // �����պ� ������Ʈ Ǯ ��ųʸ�
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // ������ ���� ��ųʸ�
    private Dictionary<string, GameObject> prefabDictionary;

    // ������ �̸� ����Ʈ
    private List<string> prefabNames;

    new void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        prefabDictionary = new Dictionary<string, GameObject>();
        prefabNames = new List<string>();
        LoadPrefabs();
    }

    void LoadPrefabs()
    {
        // Resources �������� ������ �ε�
        GameObject[] prefabs = Resources.LoadAll<GameObject>("InGame/Prefab/PoolObject");

        foreach (GameObject prefab in prefabs)
        {
            // �������� �̸����� ����
            prefabDictionary.Add(prefab.name, prefab);
            prefabNames.Add(prefab.name);

            // �ش� �������� ������Ʈ Ǯ ����
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // �ʱ� Ǯ ũ�� ���� (�ʿ信 ���� ����)
            int initialPoolSize = 1;

            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(prefab.name, objectPool);
        }
    }

    // ������Ʈ Ǯ���� ������Ʈ ��������
    public GameObject GetFromPool(string prefabName)
    {
        if (!poolDictionary.ContainsKey(prefabName))
        {
            Debug.LogWarning("PoolManager: " + prefabName + " Ǯ�� �������� �ʽ��ϴ�.");
            return null;
        }

        Queue<GameObject> objectPool = poolDictionary[prefabName];

        if (objectPool.Count > 0)
        {
            GameObject obj = objectPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // Ǯ�� ���� ������Ʈ�� ������ ���ο� ������Ʈ ����
            GameObject obj = Instantiate(prefabDictionary[prefabName]);
            obj.SetActive(true);
            return obj;
        }
    }

    // ������Ʈ Ǯ�� ��ȯ�ϱ�
    public void ReturnToPool(string prefabName, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(prefabName))
        {
            Debug.LogWarning("PoolManager: " + prefabName + " Ǯ�� �������� �ʽ��ϴ�. ������Ʈ�� �ı��մϴ�.");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        poolDictionary[prefabName].Enqueue(obj);
    }
}
