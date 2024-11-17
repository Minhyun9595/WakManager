using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface GridInterface
{
    public abstract void Init(GameObject _gameObject);
}

public abstract class GridAbstract
{
    public GameObject gameObject;

    public virtual void Init(GameObject _gameObject)
    {
        gameObject = _gameObject;
    }
}


public class PanelRenderQueueManager : CustomSingleton<PanelRenderQueueManager>
{
    public enum ECanvasType
    {
        Canvas,
        FrontCanvas,
    }


    public static List<GameObject> PanelPrefabs = new List<GameObject>();
    public static Dictionary<string, GameObject> SpawnedPanels = new Dictionary<string, GameObject>();
    public static List<PanelAbstract> OpenPanelList = new List<PanelAbstract>();

    new void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Panel/Prefabs");
        PanelPrefabs.AddRange(prefabs);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            CloseFrontPanel();
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CloseAllPanels();

        GameObject[] prefabs = Resources.LoadAll<GameObject>("Panel/Prefabs");
        PanelPrefabs.AddRange(prefabs);
    }


    public void OnSceneUnloaded(Scene current)
    {

    }

    private void CloseAllPanels()
    {
        OpenPanelList.Clear();
        SpawnedPanels.Clear();
        PanelPrefabs.Clear();
    }

    public static GameObject OpenPanel(string panelName, ECanvasType eCanvasType = ECanvasType.Canvas)
    {
        var CanvasTransform = GameObject.FindWithTag(eCanvasType.ToString()).transform;
        GameObject panel = null;
        if (SpawnedPanels.TryGetValue(panelName, out panel))
        {
            if(panel.activeSelf)
            {
                // 이미 열려있는 경우 최상단으로 이동
                panel.transform.SetAsLastSibling();
                return panel;
            }
        }

        if (SpawnedPanels.TryGetValue(panelName, out panel) == false)
        {
            var findPanelPrefab = PanelPrefabs.Find(x => x.name == panelName);
            if (findPanelPrefab != null)
            {
                var createPanel = Instantiate<GameObject>(findPanelPrefab, CanvasTransform);
                SpawnedPanels.Add(panelName, createPanel);
            }
        }

        if (SpawnedPanels.TryGetValue(panelName, out panel))
        {
            panel.GetComponent<PanelAbstract>().Open();
            panel.transform.SetParent(CanvasTransform);

            return panel;
        }

        return null;
    }

    public static GameObject OpenPanel(EPanelPrefabType ePanelPrefabType, ECanvasType eCanvasType = ECanvasType.Canvas)
    {
        return OpenPanel(ePanelPrefabType.ToString(), eCanvasType);
    }

    private void CloseFrontPanel()
    {
        var frontPanel = PopPanel();
        if (frontPanel != null)
        {
            frontPanel.Close();
        }
    }

    public void PushPanel(PanelAbstract panel)
    {
        OpenPanelList.Add(panel);
    }

    public PanelAbstract PopPanel()
    {
        if (OpenPanelList.Count == 0)
            return null;
        var last = OpenPanelList.Last(x => x.isCanClose == true);
        OpenPanelList.RemoveAt(OpenPanelList.Count - 1);

        return last;
    }

    public void ClosePanel(PanelAbstract panelAbstract)
    {
        Transform DisablePanelParent = GameObject.FindWithTag("DisablePanelParent").transform;
        var index = OpenPanelList.FindIndex(x => x.name == panelAbstract.name);
        if (0 <= index)
        {
            OpenPanelList.RemoveAt(index);
        }

        panelAbstract.gameObject.transform.SetParent(DisablePanelParent);
        panelAbstract.gameObject.SetActive(false);
    }

    public T GetPanel<T>() where T : PanelAbstract
    {
        var panel = OpenPanelList.OfType<T>().First();

        return panel;
    }
}
