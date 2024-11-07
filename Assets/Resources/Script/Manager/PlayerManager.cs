using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string saveName;
    public string saveTime;
}

public class PlayerManager : CustomSingleton<PlayerManager>
{
    public string saveName;
    private string saveFolderPath;

    void Start()
    {
        // 사용자 문서 폴더 경로 얻기
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // 프로젝트 이름과 세이브 데이터 폴더 설정
        saveFolderPath = Path.Combine(documentsPath, "WM2025", "SaveData");

        // 폴더가 없는 경우 생성
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
    }

    void Update()
    {

    }

    public void SaveData(int index)
    {
        SaveData saveData = new SaveData();
        saveData.saveName = $"이름_{index + 1}";
        saveData.saveTime = DateTime.Now.ToString("yyyy년-MM월-dd일 HH:mm");

        var filePath = Path.Combine(saveFolderPath, $"saveData_{index}.json");
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, json);
    }

    public SaveData LoadData(int index)
    {
        var filePath = Path.Combine(saveFolderPath, $"saveData_{index}.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data;
        }
        else
        {
            return null;
        }
    }
}
