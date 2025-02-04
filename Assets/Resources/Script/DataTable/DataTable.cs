using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

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
        Initialize_DT_Market();

        Inltialize_DT_UnitInfo_Immutable();
        Inltialize_DT_UnitStat();
    }
}