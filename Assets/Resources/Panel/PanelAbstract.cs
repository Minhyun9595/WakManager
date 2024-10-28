using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAbstract : MonoBehaviour
{
    public void Open()
    {
        PanelManager.Instance.PushPanel(this);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Debug.Log("PanelClose");
        gameObject.SetActive(false);
        PanelManager.Instance.ClosePanel(this);
    }
}
