using QUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrontInfoCanvas : PanelAbstract
{
    private static FrontInfoCanvas _instance;

    public static FrontInfoCanvas Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FrontInfoCanvas>();
            }
            return _instance;
        }
    }

    public TextMeshProUGUI PanelNameText;
    public TextMeshProUGUI DateText;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI MaintenanceCostText;

    private void Awake()
    {
        _instance = this;
        PanelNameText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "PanelNameText");
        DateText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "DateText");
        MoneyText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "MoneyText");
        MaintenanceCostText = UIUtility.FindComponentInChildrenByName<TextMeshProUGUI>(gameObject, "MaintenanceCostText");
    }

    void UpdateFrontPanel()
    {
        
    }

    public void SetPanelName(string _panelName)
    {
        PanelNameText.text = _panelName;
    }

    public void SetDateText(DateTime _dateTime)
    {
        DateText.text = _dateTime.ToLongDateString();
    }

    public void SetMoneyText(int _money)
    {
        MoneyText.text = $"자금: {UIUtility.GetUnitizeText(_money)} $";
    }

    public void SetMaintenanceText(int _maintenanceCost)
    {
        MoneyText.text = $"유지비: {UIUtility.GetUnitizeText(_maintenanceCost)} $";
    }
}
