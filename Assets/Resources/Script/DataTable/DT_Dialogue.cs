using Doublsb.Dialog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DT_Dialogue
{
    public static Dictionary<int, DT_Dialogue> infoDictionary = new Dictionary<int, DT_Dialogue>();
    public static List<DT_Dialogue> listInfo = new List<DT_Dialogue>();
    public static Dictionary<int, List<DT_Dialogue>> groupedDialogueDictionary = new Dictionary<int, List<DT_Dialogue>>();


    public int Index;
    public int DialogueIndex;
    public string Name;
    public string Messanger;
    public string Dialogue;
    public int Value1;
    public int Value2;
    public int Value3;
    public int Value4;

    public DT_Dialogue() { }

    public static DT_Dialogue GetInfoByIndex(int dialogueIndex)
    {
        if (infoDictionary.TryGetValue(dialogueIndex, out var info))
        {
            return info;
        }

        Debug.LogWarning($"Index {dialogueIndex} not found in InfoManager.");
        return null;
    }

    public static List<DialogData> GetDialogueDatas(int dialogueIndex)
    {
        var dialogTexts = new List<DialogData>();

        // 주어진 dialogueIndex에 해당하는 데이터를 가져옴
        if (DT_Dialogue.groupedDialogueDictionary.TryGetValue(dialogueIndex, out var dialogues))
        {
            foreach (var dialogue in dialogues)
            {
                // 각 DT_Dialogue 객체를 DialogData로 변환하여 추가
                dialogTexts.Add(new DialogData(dialogue.Dialogue, dialogue.Messanger));
            }
        }
        else
        {
            Debug.LogWarning($"No dialogues found for DialogueIndex: {dialogueIndex}");
        }
        return dialogTexts;
    }
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Inltialize_DT_Dialogue()
    {
        List<DT_Dialogue> infoList = DataLoader.Instance.LoadCSV<DT_Dialogue>(Path.Combine(Application.dataPath, "Resources/DataSet/Dialogue.csv"));

        foreach (var info in infoList)
        {
            DT_Dialogue.infoDictionary[info.DialogueIndex] = info;
            DT_Dialogue.listInfo.Add(info);

            if (DT_Dialogue.groupedDialogueDictionary.ContainsKey(info.DialogueIndex) == false)
            {
                DT_Dialogue.groupedDialogueDictionary[info.DialogueIndex] = new List<DT_Dialogue>();
            }
            DT_Dialogue.groupedDialogueDictionary[info.DialogueIndex].Add(info);
        }

        foreach (var kvp in DT_Dialogue.groupedDialogueDictionary)
        {
            kvp.Value.Sort((a, b) => a.Index.CompareTo(b.Index));
        }
    }
}
