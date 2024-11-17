using Doublsb.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_StoryDialogue : PanelAbstract
{
    DialogManager dialogManager;

    private void Awake()
    {
        dialogManager = GetComponent<DialogManager>();
    }

    public void PlayDialogue(int dialogueIndex)
    {
        var dialogDatas = DT_Dialogue.GetDialogueDatas(dialogueIndex);

        if(dialogDatas != null) 
        { 
            dialogManager.Show(dialogDatas, () => Close());        
        }
    }
}
