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
        // ����� ���� ���� ��� ���
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // ������Ʈ �̸��� ���̺� ������ ���� ����
        saveFolderPath = Path.Combine(documentsPath, "WM2025", "SaveData");

        // ������ ���� ��� ����
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
        saveData.saveName = $"�̸�_{index + 1}";
        saveData.saveTime = DateTime.Now.ToString("yyyy��-MM��-dd�� HH:mm");

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
