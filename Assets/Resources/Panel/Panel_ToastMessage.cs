using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using QUtility;

public class Panel_ToastMessage : PanelAbstract
{
    public static void OpenToast(string toastMessage, bool isStack)
    {
        var dlg = PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_ToastMessage, PanelRenderQueueManager.ECanvasType.FrontCanvas);
        dlg.GetComponent<Panel_ToastMessage>().Open(toastMessage, isStack);
    }

    public Animator PopupTextBG_Animator;
    public TextMeshProUGUI PopupText;
    public PopupTextBG PopupTextBG;

    private Queue<string> messageQueue = new Queue<string>();
    private bool isAnimating = false;

    private void Awake()
    {
        isCanClose = false;
        PopupText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "PopupText");
        PopupTextBG_Animator = UIUtility.FindComponentInChildrenByName<Animator>(gameObject, "PopupTextBG");
        PopupTextBG = UIUtility.FindComponentInChildrenByName<PopupTextBG>(gameObject, "PopupTextBG");

        PopupTextBG.action = OnAnimationEnd;
    }

    public void Open(string toastMessage, bool isStack)
    {
        // �޽����� ť�� �߰�
        if(isStack || (isAnimating == false && isStack == false))
        {
            messageQueue.Enqueue(toastMessage);
        }

        // �ִϸ��̼��� ���� ������ ������ �ٷ� ����
        if (!isAnimating)
        {
            ShowNextMessage();
        }
    }

    private void ShowNextMessage()
    {
        if (messageQueue.Count == 0)
        {
            isAnimating = false;
            return;
        }

        string nextMessage = messageQueue.Dequeue();
        PopupText.text = nextMessage;

        isAnimating = true; 
        PopupTextBG_Animator.Play("PopupAnimation", 0, 0f);
    }

    // �ִϸ��̼� ���� �̺�Ʈ ó��
    public void OnAnimationEnd()
    {
        isAnimating = false;

        // ���� �޽����� ǥ��
        ShowNextMessage();
    }
}
