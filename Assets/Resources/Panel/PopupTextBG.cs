using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTextBG : MonoBehaviour
{
    public Action action;

    public void AnimationEnd()
    {
        if(action != null) 
            action();
    }
}
