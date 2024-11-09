using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolManager : CustomSingleton<PoolManager>
{
    // 프리팹별 오브젝트 풀 딕셔너리
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // 프리팹 참조 딕셔너리
    private Dictionary<string, GameObject> prefabDictionary;

    // 프리팹 이름 리스트
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

        // Resources 폴더에서 프리팹 로드
        GameObject[] prefabs = Resources.LoadAll<GameObject>("InGame/Prefab/PoolObject");

        foreach (GameObject prefab in prefabs)
        {
            // 프리팹을 이름으로 저장
            prefabDictionary.Add(prefab.name, prefab);
            prefabNames.Add(prefab.name);

            // 해당 프리팹의 오브젝트 풀 생성
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // 초기 풀 크기 설정 (필요에 따라 조절)
            int initialPoolSize = 1;

            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject obj = Instantiate(prefab, null);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(prefab.name, objectPool);
            //Debug.Log("PoolManager: " + prefab.name + " 로드 완료.");
        }
    }

    // 오브젝트 풀에서 오브젝트 가져오기
    public GameObject GetFromPool(string prefabName)
    {
        if (!poolDictionary.ContainsKey(prefabName))
        {
            Debug.LogWarning("PoolManager: " + prefabName + " 풀은 존재하지 않습니다.");
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
            // 풀에 남은 오브젝트가 없으면 새로운 오브젝트 생성
            GameObject obj = Instantiate(prefabDictionary[prefabName], null);
            obj.SetActive(true);
            obj.transform.SetParent(null);
            return obj;
        }
    }

    // 오브젝트 풀에 반환하기
    public void ReturnToPool(string prefabName, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(prefabName))
        {
            Debug.LogWarning("PoolManager: " + prefabName + " 풀은 존재하지 않습니다. 오브젝트를 파괴합니다.");
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

        // 딕셔너리 정리
        poolDictionary.Clear();
        prefabDictionary.Clear();
        prefabNames.Clear();

        Debug.Log("PoolManager: SceneChange로 모든 풀 오브젝트가 제거되었습니다.");
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadPrefabs();
    }
}
