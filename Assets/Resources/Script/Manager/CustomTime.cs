using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTime : CustomSingleton<CustomTime>
{
    private float[] timeScales = { 1.0f, 2.0f, 3.0f }; // ��� ����
    private int currentIndex = 0; // ���� ����� �ε���
    private static float _timeScale = 1.0f;
    private static float _customTime = 0.0f;
    private static float _lastRealTime = 0.0f;

    public static float timeScale
    {
        get => _timeScale;
        set
        {
            _timeScale = Mathf.Max(0, value); // ���� ����
        }
    }

    public static float time => _customTime;

    public static float deltaTime => Time.deltaTime * _timeScale;

    public void Start()
    {
        _lastRealTime = Time.realtimeSinceStartup;
    }

    public void Update()
    {
        float currentRealTime = Time.realtimeSinceStartup;
        float deltaTime = (currentRealTime - _lastRealTime) * _timeScale;
        _customTime += deltaTime;
        _lastRealTime = currentRealTime;

        if (Input.GetKeyUp(KeyCode.P))
        {
            // �ε��� ��ȯ (���� ������� ����)
            currentIndex = (currentIndex + 1) % timeScales.Length;
            _timeScale = timeScales[currentIndex];

            Debug.Log($"Time Scale: {_timeScale}");
        }
    }
}
