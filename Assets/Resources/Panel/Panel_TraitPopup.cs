using QUtility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_TraitPopup : PanelAbstract
{
    private bool isLoaded = false;
    public Transform TraitPopup;
    public TextMeshProUGUI TraitPopupRankText;
    public Transform TraitPopupImageBG;
    public Image TraitPopupImage;
    public TextMeshProUGUI TraitPopupNameText;
    public TextMeshProUGUI TraitPopupDescText;

    public void Init()
    {
        if (isLoaded)
            return;
        isLoaded = true;

        TraitPopup = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "TraitPopup");
        TraitPopupRankText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TraitPopupRankText");
        TraitPopupImageBG = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "TraitPopupImageBG");
        TraitPopupImage = UIUtility.FindComponentInChildrenByName<Image>(gameObject, "TraitPopupImage");
        TraitPopupNameText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TraitPopupNameText");
        TraitPopupDescText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "TraitPopupDescText");

        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnClick_TraitPopupBG);
    }

    public void Open(int _index)
    {
        base.Open();
        Init();

        Debug.Log(_index);
        var dt_Trait = DT_Trait.GetInfoByIndex(_index);
        TraitPopupRankText.text = dt_Trait.Rank.ToString();
        TraitPopupImage.sprite = UIUtility.GetSprite(dt_Trait.IconSprite);
        TraitPopupNameText.text = dt_Trait.Name;

        string descriptionText = string.Join("\n",
            string.Format(dt_Trait.Desc1, dt_Trait.Value1),
            string.Format(dt_Trait.Desc2, dt_Trait.Value2));

        TraitPopupDescText.text = descriptionText;
    }

    public void OnClick_TraitPopupBG()
    {
        Close();
    }
}
