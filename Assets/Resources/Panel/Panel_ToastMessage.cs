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
        // 메시지를 큐에 추가
        if(isStack || (isAnimating == false && isStack == false))
        {
            messageQueue.Enqueue(toastMessage);
        }

        // 애니메이션이 실행 중이지 않으면 바로 실행
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

    // 애니메이션 종료 이벤트 처리
    public void OnAnimationEnd()
    {
        isAnimating = false;

        // 다음 메시지를 표시
        ShowNextMessage();
    }
}
