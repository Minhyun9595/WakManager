using QUtility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Panel_FieldBattleEnd : PanelAbstract
{
    public Animator fieldBattleEndAnimator;
    public Button ExitButton;
    public TextMeshProUGUI FieldBattleEndText;
    public TextMeshProUGUI ExitText;

    private void Awake()
    {
        fieldBattleEndAnimator = transform.GetComponent<Animator>();
        ExitButton = UIUtility.FindComponentInChildrenByName<Button>(gameObject, "ExitButton");
        FieldBattleEndText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "FieldBattleEndText");
        ExitText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "ExitText");

        ExitButton.onClick.AddListener(OnClick_Exit);
    }

    public override void Open()
    {
        base.Open();
    }

    public void OnClick_Exit()
    {
        PlayerManager.Instance.SetSceneChangeType(SceneChangeType.FieldBattle_Scream_End);
        SceneManager.LoadScene((int)ESceneType.Lobby);
    }
}
