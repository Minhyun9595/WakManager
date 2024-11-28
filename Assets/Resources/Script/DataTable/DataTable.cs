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
    bool isLoaded = false;

    public void Initialize()
    {
        if (isLoaded)
            return;
        isLoaded = true;

        Inltialize_DT_Role();
        Inltialize_DT_Skill();
        Inltialize_DT_Trait();
        Inltialize_DT_TraitValue();
        Inltialize_DT_Dialogue();
        Inltialize_DT_Condition();
        Inltialize_DT_Const();
        Inltialize_DT_TierInfo();
        Inltialize_DT_TeamUpgrade();
        Inltialize_DT_TeamTierInfo();
        Initialize_DT_Potential();

        Inltialize_DT_UnitInfo_Immutable();
        Inltialize_DT_UnitStat();
    }
}