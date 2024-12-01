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
    public static string 근거리_딜러 = "근거리 딜러"; // 근거리 유닛인 경우 발동합니다.	기본 공격이 {0}%의 추가 피해를 입힙니다.
    public static string 원거리_딜러 = "원거리 딜러"; // 원거리 유닛인 경우 발동합니다.	기본 공격이 {0}%의 추가 피해를 입힙니다.
    public static string 날렵함 = "날렵함"; // 공격 모션이 {0}% 빨라집니다.
    public static string 질긴_가죽 = "질긴 가죽"; // 받은 피해의 {0}이 감소된 피해를 입습니다. (도트 당 적용)
    public static string 인내심 = "인내심"; // 받은 피해를 {0}% 감소시킵니다.
    public static string 멀티샷 = "멀티샷"; // 투사체 {0}개를 추가로 생성합니다.
    public static string 민첩함 = "민첩함"; // 이동속도가 {0}% 증가합니다.
    public static string 저격수 = "저격수"; // 대상과의 거리 1M마다 {0}%의 추가 피해를 줍니다.
    public static string 매의_눈 = "매의 눈"; // 사거리가 {0} 증가합니다.
    public static string 쇼메이커 = "쇼메이커"; // 인기가 더 많이 증가합니다.
}

public static class Colors
{
    public static string 빨강 = "#BB4642";
    public static string 선명한빨강 = "#BF3B3C";
    public static string 밝은빨강 = "#DD534A";
    public static string 진한빨강 = "#993E41";
    public static string 흐린빨강 = "#B06F68";
    public static string 탁한빨강 = "#935B55";
    public static string 어두운빨강 = "#633D43";
    public static string 회적색 = "#927A78";
    public static string 어두운회적색 = "#685351";
    public static string 검은빨강 = "#544546";
    public static string 주황 = "#F2773D";
    public static string 선명한주황 = "#F17845";
    public static string 밝은주황 = "#FF9058";
    public static string 진한주황 = "#DA6730";
    public static string 흐린주황 = "#E0805B";
    public static string 탁한주황 = "#C99176";
    public static string 빨간주황 = "#DB5D3B";
    public static string 선명한빨간주황 = "#E15637";
    public static string 밝은빨간주황 = "#F56E48";
    public static string 탁한빨간주황 = "#AC7062";
    public static string 노란주황 = "#FF982E";
    public static string 선명한노란주황 = "#FF9913";
    public static string 밝은노란주황 = "#FFB12B";
    public static string 진한노란주황 = "#DB8628";
    public static string 연한노란주황 = "#FFB66A";
    public static string 흐린노란주황 = "#FABE93";
    public static string 탁한노란주황 = "#C28B69";
    public static string 노랑 = "#FFD10D";
    public static string 진한노랑 = "#FFBB14";
    public static string 연한노랑 = "#FCDA94";
    public static string 흐린노랑 = "#E4C77E";
    public static string 흰노랑 = "#F3E6C7";
    public static string 회황색 = "#DBCCB0";
    public static string 밝은회황색 = "#E5D7BA";
    public static string 연두 = "#94C056";
    public static string 선명한연두 = "#77A832";
    public static string 밝은연두 = "#ABD664";
    public static string 진한연두 = "#648C35";
    public static string 연한연두 = "#B8D38F";
    public static string 흐린연두 = "#A0BB7F";
    public static string 탁한연두 = "#8AA269";
    public static string 노란연두 = "#D8C949";
    public static string 선명한노란연두 = "#E2CD26";
    public static string 밝은노란연두 = "#EEDA51";
    public static string 진한노란연두 = "#C6B13C";
    public static string 연한노란연두 = "#F3E591";
    public static string 흐린노란연두 = "#D2CA80";
    public static string 탁한노란연두 = "#C3B56B";
    public static string 녹연두 = "#65A854";
    public static string 선명한녹연두 = "#5BAC49";
    public static string 밝은녹연두 = "#7FC26A";
    public static string 연한녹연두 = "#ADD797";
    public static string 흐린녹연두 = "#91BD86";
    public static string 탁한녹연두 = "#7BA570";
    public static string 흰연두 = "#E0E9CB";
    public static string 회연두 = "#96A08A";
    public static string 밝은회연두 = "#C6D1B5";
    public static string 초록 = "#247C4D";
    public static string 선명한초록 = "#059651";
    public static string 밝은초록 = "#36935B";
    public static string 진한초록 = "#3C6249";
    public static string 연한초록 = "#95DAAF";
    public static string 흐린초록 = "#6AAA87";
    public static string 탁한초록 = "#4A7859";
    public static string 어두운초록 = "#3E4E43";
    public static string 흰초록 = "#D2EDD5";
    public static string 회녹색 = "#73877A";
    public static string 밝은회녹색 = "#BCD0BC";
    public static string 어두운회녹색 = "#525D53";
    public static string 검은초록 = "#454E48";
    public static string 청록 = "#006B70";
    public static string 밝은청록 = "#00979C";
    public static string 진한청록 = "#21555A";
    public static string 연한청록 = "#66C4C3";
    public static string 흐린청록 = "#6FA6A5";
    public static string 탁한청록 = "#437779";
    public static string 어두운청록 = "#345052";
    public static string 흰청록 = "#CEEBE6";
    public static string 회청록 = "#708889";
    public static string 밝은회청록 = "#9CB8B7";
    public static string 어두운회청록 = "#495E5B";
    public static string 검은청록 = "#3E4C4E";
    public static string 파랑 = "#0F7CA8";
    public static string 선명한파랑 = "#008CC3";
    public static string 밝은파랑 = "#4AA8D8";
    public static string 진한파랑 = "#344F65";
    public static string 연한파랑 = "#A6D3E9";
    public static string 흐린파랑 = "#75A6C0";
    public static string 탁한파랑 = "#4A7691";
    public static string 어두운파랑 = "#3A4E5B";
    public static string 흰파랑 = "#DFE9ED";
    public static string 회청색 = "#7A8790";
    public static string 밝은회청색 = "#ABB6BC";
    public static string 어두운회청색 = "#525E66";
    public static string 검은파랑 = "#444E56";
    public static string 남색 = "#414A67";
    public static string 밝은남색 = "#445C91";
    public static string 흐린남색 = "#4D5B7B";
    public static string 어두운남색 = "#464D5E";
    public static string 회남색 = "#696F7A";
    public static string 검은남색 = "#484B54";
    public static string 보라 = "#665581";
    public static string 선명한보라 = "#7A6397";
    public static string 밝은보라 = "#8977AD";
    public static string 진한보라 = "#554466";
    public static string 연한보라 = "#BBABD3";
    public static string 흐린보라 = "#A699BB";
    public static string 탁한보라 = "#766789";
    public static string 어두운보라 = "#4F4658";
    public static string 흰보라 = "#EAE7EC";
    public static string 회보라 = "#86828E";
    public static string 밝은회보라 = "#B7B3BD";
    public static string 어두운회보라 = "#625D6A";
    public static string 검은보라 = "#4C4751";
    public static string 자주 = "#8B425F";
    public static string 선명한자주 = "#AB5071";
    public static string 밝은자주 = "#CD5D85";
    public static string 진한자주 = "#683D51";
    public static string 연한자주 = "#BB6C85";
    public static string 흐린자주 = "#A97683";
    public static string 탁한자주 = "#7C4D5E";
    public static string 어두운자주 = "#5B444E";
    public static string 빨간자주 = "#8B4156";
    public static string 진한적자색 = "#6D3E4D";
    public static string 탁한적자색 = "#7B4C58";
    public static string 어두운적자색 = "#5D454D";
    public static string 회자주 = "#928286";
    public static string 어두운회자주 = "#66555A";
    public static string 검은자주 = "#54464B";
    public static string 분홍 = "#E99EA8";
    public static string 진한분홍 = "#DB8291";
    public static string 연한분홍 = "#F6BABF";
    public static string 흐린분홍 = "#DDA3AB";
    public static string 탁한분홍 = "#C38C94";
    public static string 노란분홍 = "#FF9477";
    public static string 진한노란분홍 = "#E77A62";
    public static string 연한노란분홍 = "#FFB8A2";
    public static string 흐린노란분홍 = "#E6A18F";
    public static string 탁한노란분홍 = "#C18375";
    public static string 흰분홍 = "#F5E6E3";
    public static string 회분홍 = "#C2B1B3";
    public static string 밝은회분홍 = "#DAC8C9";
    public static string 자줏빛분홍 = "#E79EB9";
    public static string 진한자줏빛분홍 = "#D284A3";
    public static string 연한자줏빛분홍 = "#F2C0D1";
    public static string 흐린자줏빛분홍 = "#D9A8B8";
    public static string 탁한자줏빛분홍 = "#BC8D9E";
    public static string 갈색 = "#975A3E";
    public static string 밝은갈색 = "#CB6B31";
    public static string 진한갈색 = "#78503E";
    public static string 연한갈색 = "#AA704A";
    public static string 흐린갈색 = "#A47A68";
    public static string 탁한갈색 = "#8C6354";
    public static string 어두운갈색 = "#5C443B";
    public static string 빨간갈색 = "#8B433C";
    public static string 밝은적갈색 = "#A75544";
    public static string 진한적갈색 = "#61413F";
    public static string 흐린적갈색 = "#965C51";
    public static string 탁한적갈색 = "#7B4D47";
    public static string 어두운적갈색 = "#584342";
    public static string 노란갈색 = "#AE7734";
    public static string 밝은황갈색 = "#D88231";
    public static string 연한황갈색 = "#C98C4F";
    public static string 흐린황갈색 = "#B59375";
    public static string 탁한황갈색 = "#A77A4D";
    public static string 녹갈색 = "#796D3D";
    public static string 밝은녹갈색 = "#99893C";
    public static string 흐린녹갈색 = "#92885A";
    public static string 탁한녹갈색 = "#746D48";
    public static string 어두운녹갈색 = "#5D5A3F";
    public static string 회갈색 = "#927D75";
    public static string 어두운회갈색 = "#69554E";
    public static string 검은갈색 = "#544843";
    public static string 하양 = "#F2F3F0";
    public static string 노란하양 = "#EAE3D4";
    public static string 초록빛하양 = "#DDEDDF";
    public static string 파란하양 = "#E0E8E9";
    public static string 보랏빛하양 = "#E9E7E8";
    public static string 분홍빛하양 = "#EFE5E3";
    public static string 회색 = "#818383";
    public static string 밝은회색 = "#B2B5B3";
    public static string 어두운회색 = "#5F6261";
    public static string 빨간회색 = "#8D8382";
    public static string 어두운적회색 = "#5F5353";
    public static string 노란회색 = "#D4CBBB";
    public static string 초록빛회색 = "#7E8681";
    public static string 밝은녹회색 = "#C2CFC4";
    public static string 어두운녹회색 = "#555D59";
    public static string 파란회색 = "#7F878A";
    public static string 밝은청회색 = "#ADB5B7";
    public static string 어두운청회색 = "#575C61";
    public static string 보랏빛회색 = "#86848A";
    public static string 밝은보랏빛회색 = "#B4B3B7";
    public static string 어두운보랏빛회색 = "#5B5960";
    public static string 분홍빛회색 = "#BBB2B3";
    public static string 갈회색 = "#8E817D";
    public static string 어두운갈회색 = "#615855";
    public static string 검정 = "#3B3B3B";
    public static string 빨간검정 = "#4E4747";
    public static string 초록빛검정 = "#474B49";
    public static string 파란검정 = "#474C4F";
    public static string 보랏빛검정 = "#4B494D";
    public static string 갈흑색 = "#4D4948";
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