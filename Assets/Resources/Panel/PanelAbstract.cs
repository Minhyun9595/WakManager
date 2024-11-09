using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAbstract : MonoBehaviour
{
    public void Open()
    {
        PanelRenderQueueManager.Instance.PushPanel(this);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        PanelRenderQueueManager.Instance.ClosePanel(this);
    }
}
