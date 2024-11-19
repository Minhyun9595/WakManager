using QUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Traning : PanelAbstract
{
    public Button TraningButton_1;
    public Button TraningButton_2;
    public Button TraningButton_3;
    public Button TraningButton_4;


    void Start()
    {
        TraningButton_1 = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "TraningButton_1");
        TraningButton_2 = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "TraningButton_2");
        TraningButton_3 = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "TraningButton_3");
        TraningButton_4 = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "TraningButton_4");

        TraningButton_1.onClick.AddListener(OnClick_Traning_Stat_Health);
        TraningButton_2.onClick.AddListener(OnClick_Traning_Trait);
        TraningButton_3.onClick.AddListener(OnClick_Traning_Mental);
        TraningButton_4.onClick.AddListener(OnClick_Traning_Scream);
    }
    
    public override void Open()
    {
        base.Open();
        FrontInfoCanvas.Instance.SetPanelName("ศฦทร");
    }

    void OnClick_Traning_Stat_Health()
    {
        Debug.Log("OnClick_Traning_Stat");
    }

    void OnClick_Traning_Stat_Health2()
    {
        Debug.Log("OnClick_Traning_Stat");
    }

    void OnClick_Traning_Stat_Health3()
    {
        Debug.Log("OnClick_Traning_Stat");
    }


    void OnClick_Traning_Trait()
    {
        Debug.Log("OnClick_Traning_Trait");
    }

    void OnClick_Traning_Mental()
    {
        Debug.Log("OnClick_Traning_Mental");
    }

    void OnClick_Traning_Scream()
    {
        PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Scream);
    }

}