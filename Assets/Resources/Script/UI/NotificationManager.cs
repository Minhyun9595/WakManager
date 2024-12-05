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
    public float displayTime = 3f;       // �˸��� �����Ǵ� �ð�
    public float fadeDuration = 2f;      // �˸��� ������ ������� �ð�

    private Queue<GameObject> notifications = new Queue<GameObject>();
    private List<string> messages = new List<string>();

    private void Awake()
    {
        displayTime = 2.0f;
        displayTime = 2.0f;
    }

    public void ShowNotification(string message)
    {
        // Prefab �ν��Ͻ� ����
        var panel_Notification = PanelRenderQueueManager.OpenPanel(EPanelPrefabType.Panel_Notification, PanelRenderQueueManager.ECanvasType.FrontCanvas).GetComponent<Panel_Notification>();
        GameObject notificationTextBG = Instantiate(notificationPrefab, panel_Notification.LeftBottom);
        TMP_Text textComponent = notificationTextBG.GetComponentInChildren<TMP_Text>();
        textComponent.text = message;

        notifications.Enqueue(notificationTextBG);
        messages.Add(message);

        var today = PlayerManager.Instance.gameSchedule.GetToday();
        PlayerManager.Instance.notifications.Add(new Notification(today, message));

        // ���� �޽��� ����
        StartCoroutine(HandleNotification(notificationTextBG));
    }

    private IEnumerator HandleNotification(GameObject notificationTextBG)
    {
        // ���� �ð� ���
        yield return new WaitForSeconds(displayTime);

        // Fade Out ó��
        CanvasGroup canvasGroup = notificationTextBG.GetComponent<CanvasGroup>();
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        // �˸� ����
        notifications.Dequeue();
        Destroy(notificationTextBG);
    }
}
