using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUnitTier
{
    Challenger,
    Master,
    Gold,
    Silver,
    Bronze,
    Iron
}

public enum ETeamTier
{
    First,
    Second,
    Third,
}

public enum EUnitConditionType
{
    VeryGood, // 90 ~ 100
    Good, // 80 ~ 89
    Okay, // 65 ~ 79
    Poor, // 40 ~ 64
    VeryPoor, // 0 ~ 39
}

public enum ESceneType
{
    Menu,
    Lobby,
    InGame,
}


public enum EPrefabType
{
    // 이펙트 관련
    DamageFont,
    Projectile_Wizzard,
    Projectile_Wizzard_Hit,
    Projectile_Bezier,
    Projectile_Bezier_Hit,

    Effect,

    // 유닛
    Unit,


    // 로비
    OfficeUnitObject,
}

public enum EJobType
{
    Tanker,
    Warrior,
    Magician,
    Assassin,
    Archer,
    Supporter
}

public enum EDamageType
{
    Physical,
    Magical,
    True,
}

public enum EAnimationType
{ 
    Idle,
    Move,
    Jump,
    Hit,
    Death,
    Attack1,
    Attack2,
    Skill,
}

public enum EAxsType
{
    X,
    Y,
    Z
}

public enum EPanelPrefabType
{
    Panel_SaveData,
    Panel_Market,
    Panel_TraitPopup,
    Panel_Squad,
    Panel_Schedule,
    Panel_Traning,
    Panel_StoryDialogue,
    Panel_Scream,
    Panel_ToastMessage,
    Panel_TeamUnitList,
    Panel_TeamUpgrade,
    Panel_InternationalActivity,
    Panel_Notification,
    Panel_Record,
    Panel_FieldBattleEnd,
    Panel_TeamHome,
    Panel_EventPopup,
    Panel_UnitInfo
}

public class ConstValue : CustomSingleton<ConstValue>
{
    public static float speedRatio = 0.5f;
    public static float CriticalDamageCoefficient = 0.0001f;
    public static float RangeCoefficient = 0.015f;
    public static int Layer_Character = 1000;
    public static int Layer_Effect = 10000;

}

public class TraitType
{
    public static string MeleeDealer = "근거리 딜러"; // 근거리 유닛인 경우 발동합니다.	기본 공격이 {0}%의 추가 피해를 입힙니다.
    public static string RangedDealer = "원거리 딜러"; // 원거리 유닛인 경우 발동합니다.	기본 공격이 {0}%의 추가 피해를 입힙니다.
    public static string Agile = "날렵함"; // 공격 모션이 {0}% 빨라집니다.
    public static string ToughHide = "질긴 가죽"; // 받은 피해의 {0}이 감소된 피해를 입습니다. (도트 당 적용)
    public static string Endurance = "인내심"; // 받은 피해를 {0}% 감소시킵니다.
    public static string MultiShot = "멀티샷"; // 투사체 {0}개를 추가로 생성합니다.
    public static string Mobility = "민첩함"; // 이동속도가 {0}% 증가합니다.
    public static string Sniper = "저격수"; // 대상과의 거리 1M마다 {0}%의 추가 피해를 줍니다.
    public static string EagleEye = "매의 눈"; // 사거리가 {0} 증가합니다.
    public static string Showmaker = "쇼메이커"; // 인기가 더 많이 증가합니다.
}

public static class Colors
{
    public static string Red = "#BB4642";
    public static string VividRed = "#BF3B3C";
    public static string LightRed = "#DD534A";
    public static string DeepRed = "#993E41";
    public static string FadedRed = "#B06F68";
    public static string DullRed = "#935B55";
    public static string DarkRed = "#633D43";
    public static string AshRed = "#927A78";
    public static string DarkAshRed = "#685351";
    public static string BlackRed = "#544546";

    public static string Orange = "#F2773D";
    public static string VividOrange = "#F17845";
    public static string LightOrange = "#FF9058";
    public static string DeepOrange = "#DA6730";
    public static string FadedOrange = "#E0805B";
    public static string DullOrange = "#C99176";
    public static string ReddishOrange = "#DB5D3B";
    public static string VividReddishOrange = "#E15637";
    public static string LightReddishOrange = "#F56E48";
    public static string DullReddishOrange = "#AC7062";
    public static string YellowOrange = "#FF982E";
    public static string VividYellowOrange = "#FF9913";
    public static string LightYellowOrange = "#FFB12B";
    public static string DeepYellowOrange = "#DB8628";
    public static string PaleYellowOrange = "#FFB66A";
    public static string FadedYellowOrange = "#FABE93";
    public static string DullYellowOrange = "#C28B69";

    public static string Yellow = "#FFD10D";
    public static string DeepYellow = "#FFBB14";
    public static string PaleYellow = "#FCDA94";
    public static string FadedYellow = "#E4C77E";
    public static string OffWhiteYellow = "#F3E6C7";
    public static string AshYellow = "#DBCCB0";
    public static string LightAshYellow = "#E5D7BA";

    public static string Lime = "#94C056";
    public static string VividLime = "#77A832";
    public static string LightLime = "#ABD664";
    public static string DeepLime = "#648C35";
    public static string PaleLime = "#B8D38F";
    public static string FadedLime = "#A0BB7F";
    public static string DullLime = "#8AA269";

    public static string YellowLime = "#D8C949";
    public static string VividYellowLime = "#E2CD26";
    public static string LightYellowLime = "#EEDA51";
    public static string DeepYellowLime = "#C6B13C";
    public static string PaleYellowLime = "#F3E591";
    public static string FadedYellowLime = "#D2CA80";
    public static string DullYellowLime = "#C3B56B";

    public static string OliveGreen = "#65A854";
    public static string VividOliveGreen = "#5BAC49";
    public static string LightOliveGreen = "#7FC26A";
    public static string PaleOliveGreen = "#ADD797";
    public static string FadedOliveGreen = "#91BD86";
    public static string DullOliveGreen = "#7BA570";
    public static string WhiteOliveGreen = "#E0E9CB";
    public static string GrayOliveGreen = "#96A08A";
    public static string LightGrayOliveGreen = "#C6D1B5";

    public static string Green = "#247C4D";
    public static string VividGreen = "#059651";
    public static string LightGreen = "#36935B";
    public static string DeepGreen = "#3C6249";
    public static string PaleGreen = "#95DAAF";
    public static string FadedGreen = "#6AAA87";
    public static string DullGreen = "#4A7859";
    public static string DarkGreen = "#3E4E43";
    public static string OffWhiteGreen = "#D2EDD5";
    public static string GrayGreen = "#73877A";
    public static string LightGrayGreen = "#BCD0BC";
    public static string DarkGrayGreen = "#525D53";
    public static string BlackGreen = "#454E48";

    public static string Teal = "#006B70";
    public static string LightTeal = "#00979C";
    public static string DeepTeal = "#21555A";
    public static string PaleTeal = "#66C4C3";
    public static string FadedTeal = "#6FA6A5";
    public static string DullTeal = "#437779";
    public static string DarkTeal = "#345052";
    public static string OffWhiteTeal = "#CEEBE6";
    public static string GrayTeal = "#708889";
    public static string LightGrayTeal = "#9CB8B7";
    public static string DarkGrayTeal = "#495E5B";
    public static string BlackTeal = "#3E4C4E";

    public static string Blue = "#0F7CA8";
    public static string VividBlue = "#008CC3";
    public static string LightBlue = "#4AA8D8";
    public static string DeepBlue = "#344F65";
    public static string PaleBlue = "#A6D3E9";
    public static string FadedBlue = "#75A6C0";
    public static string DullBlue = "#4A7691";
    public static string DarkBlue = "#3A4E5B";
    public static string OffWhiteBlue = "#DFE9ED";

    public static string GrayBlue = "#7A8790";
    public static string LightGrayBlue = "#ABB6BC";
    public static string DarkGrayBlue = "#525E66";
    public static string BlackBlue = "#444E56";

    public static string Navy = "#414A67";
    public static string LightNavy = "#445C91";
    public static string FadedNavy = "#4D5B7B";
    public static string DarkNavy = "#464D5E";
    public static string GrayNavy = "#696F7A";
    public static string BlackNavy = "#484B54";

    public static string Purple = "#665581";
    public static string VividPurple = "#7A6397";
    public static string LightPurple = "#8977AD";
    public static string DeepPurple = "#554466";
    public static string PalePurple = "#BBABD3";
    public static string FadedPurple = "#A699BB";
    public static string DullPurple = "#766789";
    public static string DarkPurple = "#4F4658";
    public static string OffWhitePurple = "#EAE7EC";
    public static string GrayPurple = "#86828E";
    public static string LightGrayPurple = "#B7B3BD";
    public static string DarkGrayPurple = "#625D6A";
    public static string BlackPurple = "#4C4751";

    public static string Magenta = "#8B425F";
    public static string VividMagenta = "#AB5071";
    public static string LightMagenta = "#CD5D85";
    public static string DeepMagenta = "#683D51";
    public static string PaleMagenta = "#BB6C85";
    public static string FadedMagenta = "#A97683";
    public static string DullMagenta = "#7C4D5E";
    public static string DarkMagenta = "#5B444E";
    public static string ReddishMagenta = "#8B4156";
    public static string DeepReddishMagenta = "#6D3E4D";
    public static string DullReddishMagenta = "#7B4C58";
    public static string DarkReddishMagenta = "#5D454D";
    public static string GrayMagenta = "#928286";
    public static string DarkGrayMagenta = "#66555A";
    public static string BlackMagenta = "#54464B";

    public static string Pink = "#E99EA8";
    public static string DeepPink = "#DB8291";
    public static string PalePink = "#F6BABF";
    public static string FadedPink = "#DDA3AB";
    public static string DullPink = "#C38C94";
    public static string YellowPink = "#FF9477";
    public static string DeepYellowPink = "#E77A62";
    public static string PaleYellowPink = "#FFB8A2";
    public static string FadedYellowPink = "#E6A18F";
    public static string DullYellowPink = "#C18375";
    public static string OffWhitePink = "#F5E6E3";
    public static string GrayPink = "#C2B1B3";
    public static string LightGrayPink = "#DAC8C9";
    public static string VioletPink = "#E79EB9";
    public static string DeepVioletPink = "#D284A3";
    public static string PaleVioletPink = "#F2C0D1";
    public static string FadedVioletPink = "#D9A8B8";
    public static string DullVioletPink = "#BC8D9E";

    public static string Brown = "#975A3E";
    public static string LightBrown = "#CB6B31";
    public static string DeepBrown = "#78503E";
    public static string PaleBrown = "#AA704A";
    public static string FadedBrown = "#A47A68";
    public static string DullBrown = "#8C6354";
    public static string DarkBrown = "#5C443B";
    public static string ReddishBrown = "#8B433C";
    public static string LightReddishBrown = "#A75544";
    public static string DeepReddishBrown = "#61413F";
    public static string FadedReddishBrown = "#965C51";
    public static string DullReddishBrown = "#7B4D47";
    public static string DarkReddishBrown = "#584342";
    public static string YellowishBrown = "#AE7734";
    public static string LightYellowishBrown = "#D88231";
    public static string PaleYellowishBrown = "#C98C4F";
    public static string FadedYellowishBrown = "#B59375";
    public static string DullYellowishBrown = "#A77A4D";
    public static string GreenishBrown = "#796D3D";
    public static string LightGreenishBrown = "#99893C";
    public static string FadedGreenishBrown = "#92885A";
    public static string DullGreenishBrown = "#746D48";
    public static string DarkGreenishBrown = "#5D5A3F";
    public static string GrayishBrown = "#927D75";
    public static string DarkGrayishBrown = "#69554E";
    public static string BlackBrown = "#544843";

    public static string White = "#F2F3F0";
    public static string YellowishWhite = "#EAE3D4";
    public static string GreenishWhite = "#DDEDDF";
    public static string BluishWhite = "#E0E8E9";
    public static string PurplishWhite = "#E9E7E8";
    public static string PinkishWhite = "#EFE5E3";

    public static string Gray = "#818383";
    public static string LightGray = "#B2B5B3";
    public static string DarkGray = "#5F6261";
    public static string ReddishGray = "#8D8382";
    public static string DarkReddishGray = "#5F5353";
    public static string YellowishGray = "#D4CBBB";
    public static string GreenishGray = "#7E8681";
    public static string LightGreenishGray = "#C2CFC4";
    public static string DarkGreenishGray = "#555D59";
    public static string BluishGray = "#7F878A";
    public static string LightBluishGray = "#ADB5B7";
    public static string DarkBluishGray = "#575C61";
    public static string PurplishGray = "#86848A";
    public static string LightPurplishGray = "#B4B3B7";
    public static string DarkPurplishGray = "#5B5960";
    public static string PinkishGray = "#BBB2B3";
    public static string BrownishGray = "#8E817D";
    public static string DarkBrownishGray = "#615855";

    public static string Black = "#3B3B3B";
    public static string ReddishBlack = "#4E4747";
    public static string GreenishBlack = "#474B49";
    public static string BluishBlack = "#474C4F";
    public static string PurplishBlack = "#4B494D";
    public static string BrownishBlack = "#4D4948";
}

public class StringUtility
{
    public static void AddSplitList(ref List<string> list, string input, string cut)
    {
        if (string.IsNullOrEmpty(input) == false)
        {
            var skillSplit = input.Split(cut);

            foreach (var skill in skillSplit)
            {
                list.Add(skill);
            }
        }
    }
}