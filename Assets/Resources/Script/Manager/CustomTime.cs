using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTime : CustomSingleton<CustomTime>
{
    private float[] timeScales = { 1.0f, 2.0f, 3.0f }; // 배속 값들
    private int currentIndex = 0; // 현재 배속의 인덱스
    private static float _timeScale = 1.0f;
    private static float _customTime = 0.0f;
    private static float _lastRealTime = 0.0f;

    public static float timeScale
    {
        get => _timeScale;
        set
        {
            _timeScale = Mathf.Max(0, value); // 음수 방지
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
            // 인덱스 순환 (다음 배속으로 변경)
            currentIndex = (currentIndex + 1) % timeScales.Length;
            _timeScale = timeScales[currentIndex];

            Debug.Log($"Time Scale: {_timeScale}");
        }
    }
}
