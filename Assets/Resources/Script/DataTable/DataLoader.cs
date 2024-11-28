using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;
using Unity.VisualScripting;
using UnityEngine;


public class DataLoader : CustomSingleton<DataLoader>
{
    public List<T> LoadCSV<T>(string filePath) where T : new()
    {
        List<T> dataList = new List<T>();
        string[] lines = File.ReadAllLines(filePath);

        // [Field] 키워드가 포함된 줄 찾기
        int startIndex = Array.FindIndex(lines, line => line.Contains("[Field]"));
        if (startIndex == -1 || startIndex + 1 >= lines.Length)
        {
            Debug.LogWarning("[Field]를 찾을 수 없거나 데이터가 없습니다.");
            return dataList;
        }

        // [Field] 줄 이후부터 데이터 처리 시작
        string[] headers = lines[startIndex + 1].Split(',');

        for (int i = startIndex + 2; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            T dataItem = new T();

            // 필드 이름을 기반으로 매핑
            foreach (var field in typeof(T).GetFields())
            {
                for (int j = 0; j < headers.Length; j++)
                {
                    if (field.Name.Equals(headers[j].Trim(), StringComparison.OrdinalIgnoreCase))
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