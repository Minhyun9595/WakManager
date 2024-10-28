using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;


public class Role
{
    public int Index;
    public string Name;
    public string Characteristic_1;
    public string Characteristic_2;
}

public partial class DataTable : CustomSingleton<DataTable>
{
    public void Start()
    {
        Debug.Log("asdasd");
        Initialize();
    }

    public void Initialize()
    {
        Inltialize_DT_Unit();
    }
}