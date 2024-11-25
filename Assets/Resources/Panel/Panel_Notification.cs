using QUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Notification : PanelAbstract
{
    public Transform LeftBottom;

    private void Awake()
    {
        LeftBottom = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "LeftBottom");
    }

    public override void Open()
    {
        base.Open();
    }
}
