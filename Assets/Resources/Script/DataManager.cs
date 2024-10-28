using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;
using Unity.VisualScripting;
using UnityEngine;


public class DataManager : CustomSingleton<DataManager>
{
    public List<T> LoadCSV<T>(string filePath) where T : new()
    {
        List<T> dataList = new List<T>();
        string[] lines = File.ReadAllLines(filePath);

        if (lines.Length <= 1)
        {
            Debug.LogWarning("CSV ���Ͽ� �����Ͱ� �����ϴ�.");
            return dataList;
        }

        // ù ��° �ٿ��� �÷� �̸��� �н��ϴ�.
        string[] headers = lines[0].Split(',');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            T dataItem = new T();

            // �ʵ� �̸��� ������� ����
            foreach (var field in typeof(T).GetFields())
            {
                for (int j = 0; j < headers.Length; j++)
                {
                    if (field.Name.Equals(headers[j], StringComparison.OrdinalIgnoreCase))
                    {
                        object convertedValue = ConvertValue(values[j], field.FieldType);
                        field.SetValue(dataItem, convertedValue);
                        break;
                    }
                }
            }

            dataList.Add(dataItem);
        }

        return dataList;
    }

    private object ConvertValue(string value, Type fieldType)
    {
        if (fieldType == typeof(int))
        {
            if (int.TryParse(value, out int intValue))
            {
                return intValue;
            }
            return 0;
        }
        else if (fieldType == typeof(float))
        {
            if (float.TryParse(value, out float floatValue))
            {
                return floatValue;
            }
            return 0f;
        }
        else if (fieldType == typeof(bool))
        {
            if (bool.TryParse(value, out bool boolValue))
            {
                return boolValue;
            }
            return false;
        }
        else if (fieldType == typeof(string))
        {
            return value;
        }

        Debug.LogWarning($"Unsupported field type: {fieldType.Name}");
        return null;
    }
}