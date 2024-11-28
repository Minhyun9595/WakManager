using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DT_Potential
{
    public static Dictionary<int, DT_Potential> infoDictionary = new Dictionary<int, DT_Potential>();
    public static List<DT_Potential> listInfo = new List<DT_Potential>();

    public int Index;
    public int Min;
    public int Max;

    public DT_Potential() { }

    public static DT_Potential GetInfoByIndex(int index)
    {
        if (infoDictionary.TryGetValue(index, out var result))
        {
            return result;
        }

        Debug.LogWarning($"Index {index} not found in DT_Potential.");
        return null;
    }

    public static bool IsWithinRange(int index, int value)
    {
        if (infoDictionary.TryGetValue(index, out var potential))
        {
            return value >= potential.Min && value <= potential.Max;
        }

        Debug.LogWarning($"Index {index} not found in DT_Potential.");
        return false;
    }

    public static int GetRandomPotentialValue(int index)
    {
        if (infoDictionary.TryGetValue(index, out var potential))
        {
            return Random.Range(potential.Min, potential.Max + 1); // Unity의 Random.Range는 Max를 포함하지 않으므로 +1
        }

        Debug.LogWarning($"Index {index} not found in DT_Potential.");
        return -1; // 적절한 실패 값을 반환
    }
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Initialize_DT_Potential()
    {
        List<DT_Potential> infoList = DataLoader.Instance.LoadCSV<DT_Potential>(Path.Combine(Application.dataPath, "Resources/DataSet/Potential.csv"));

        // Dictionary에 데이터를 저장
        foreach (var info in infoList)
        {
            if (!DT_Potential.infoDictionary.ContainsKey(info.Index))
            {
                DT_Potential.infoDictionary[info.Index] = info;
            }
            else
            {
                Debug.LogWarning($"Duplicate index found in Potential data: {info.Index}");
            }

            DT_Potential.listInfo.Add(info);
        }
    }
}
