using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAbstract : MonoBehaviour
{
    public bool isCanClose = true;
    public virtual void Open()
    {
        PanelRenderQueueManager.Instance.PushPanel(this);
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        PanelRenderQueueManager.Instance.ClosePanel(this);
    }
}
