using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Notification
{
    public string date;
    public string desc;

    public Notification(string _date, string _desc)
    {
        this.date = _date;
        this.desc = _desc;
    }
}

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;

    public static NotificationManager Instance
    {
        get
        {
            instance = FindObjectOfType<NotificationManager>();

            return instance;
        }
    }

    public GameObject notificationPrefab;
    public float displayTime = 3f;       // 알림이 유지되는 시간
    public float fadeDuration = 2f;      // 알림이 서서히 사라지는 시간

    private Queue<string> notificationQueue = new Queue<string>();
    private bool isShowingNotification = false;


    private void Awake()
    {
        displayTime = 2.0f;
        displayTime = 2.0f;
    }

    public void ShowNotification(string message)
    {
        notificationQueue.Enqueue(message);

        // PlayerManager의 알림 기록에도 추가 (날짜 정보 추가)
        var today = PlayerManager.Instance.gameSchedule.GetToday();
        PlayerManager.Instance.notifications.Add(new Notification(today, message));

        // 현재 알림이 표시 중이지 않다면, 큐 처리를 시작
        if (!isShowingNotification)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isShowingNotification = true;

        // 큐에 메시지가 있는 동안 반복
        while (notificationQueue.Count > 0)
        {
            string message = notificationQueue.Dequeue();

            var panel_Notification = PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Notification, PanelRenderQueueManager.ECanvasType.FrontCanvas)
                .GetComponent<Panel_Notification>();

            GameObject notificationObj = Instantiate(notificationPrefab, panel_Notification.LeftBottom);
            TMP_Text textComponent = notificationObj.GetComponentInChildren<TMP_Text>();
            textComponent.text = message;

            yield return new WaitForSeconds(displayTime);

            CanvasGroup canvasGroup = notificationObj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = notificationObj.AddComponent<CanvasGroup>();
            }

            // 알림 fade-out 처리
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                yield return null;
            }

            // 알림 객체 파괴
            Destroy(notificationObj);
        }

        isShowingNotification = false;
    }
}
