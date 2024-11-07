using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface GridInterface
{
    public void Init(GameObject _gameObject);
}

public abstract class GridAbstract
{
    public GameObject gameObject;

    public void Init(GameObject _gameObject)
    {
        gameObject = _gameObject;
    }
}


public class PanelRenderQueueManager : CustomSingleton<PanelRenderQueueManager>
{

    public static List<GameObject> PanelPrefabs = new List<GameObject>();
    public static Dictionary<string, GameObject> SpawnedPanels = new Dictionary<string, GameObject>();
    public static List<PanelAbstract> OpenPanelList = new List<PanelAbstract>();

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
        if (Input.GetKeyUp(KeyCode.I))
        {
            OpenPanel("Panel1");
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            OpenPanel("Panel2");
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            OpenPanel("Panel3");
        }
    }

    public static void OpenPanel(string panelName)
    {
        var CanvasTransform = GameObject.FindWithTag("Canvas").transform;

        if (SpawnedPanels.TryGetValue(panelName, out GameObject panel) == false)
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
        }
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
        var last = OpenPanelList.Last();
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
    }
}
