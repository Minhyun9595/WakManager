using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolManager : CustomSingleton<PoolManager>
{
    // �����պ� ������Ʈ Ǯ ��ųʸ�
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // ������ ���� ��ųʸ�
    private Dictionary<string, GameObject> prefabDictionary;

    // ������ �̸� ����Ʈ
    private List<string> prefabNames;

    private bool isLoaded = false;
    new void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        prefabDictionary = new Dictionary<string, GameObject>();
        prefabNames = new List<string>();
        isLoaded = false;

        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        LoadPrefabs();
    }

    void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void LoadPrefabs()
    {
        //if (isLoaded)
        //    return;
        //
        //isLoaded = true;

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
                GameObject obj = Instantiate(prefab, null);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(prefab.name, objectPool);
            //Debug.Log("PoolManager: " + prefab.name + " �ε� �Ϸ�.");
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
            obj.transform.SetParent(null);
            return obj;
        }
        else
        {
            // Ǯ�� ���� ������Ʈ�� ������ ���ο� ������Ʈ ����
            GameObject obj = Instantiate(prefabDictionary[prefabName], null);
            obj.SetActive(true);
            obj.transform.SetParent(null);
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
        obj.transform.SetParent(transform);
        poolDictionary[prefabName].Enqueue(obj);
    }

    public void OnSceneUnloaded(Scene current)
    {
        foreach (var pool in poolDictionary.Values)
        {
            while (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                Destroy(obj);
            }
        }

        // ��ųʸ� ����
        poolDictionary.Clear();
        prefabDictionary.Clear();
        prefabNames.Clear();

        Debug.Log("PoolManager: SceneChange�� ��� Ǯ ������Ʈ�� ���ŵǾ����ϴ�.");
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadPrefabs();
    }
}
