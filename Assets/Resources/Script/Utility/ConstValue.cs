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
    // ����Ʈ ����
    DamageFont,
    Projectile_Wizzard,
    Projectile_Wizzard_Hit,
    Projectile_Bezier,
    Projectile_Bezier_Hit,

    Effect,

    // ����
    Unit,


    // �κ�
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
    public static string �ٰŸ�_���� = "�ٰŸ� ����"; // �ٰŸ� ������ ��� �ߵ��մϴ�.	�⺻ ������ {0}%�� �߰� ���ظ� �����ϴ�.
    public static string ���Ÿ�_���� = "���Ÿ� ����"; // ���Ÿ� ������ ��� �ߵ��մϴ�.	�⺻ ������ {0}%�� �߰� ���ظ� �����ϴ�.
    public static string ������ = "������"; // ���� ����� {0}% �������ϴ�.
    public static string ����_���� = "���� ����"; // ���� ������ {0}�� ���ҵ� ���ظ� �Խ��ϴ�. (��Ʈ �� ����)
    public static string �γ��� = "�γ���"; // ���� ���ظ� {0}% ���ҽ�ŵ�ϴ�.
    public static string ��Ƽ�� = "��Ƽ��"; // ����ü {0}���� �߰��� �����մϴ�.
    public static string ��ø�� = "��ø��"; // �̵��ӵ��� {0}% �����մϴ�.
    public static string ���ݼ� = "���ݼ�"; // ������ �Ÿ� 1M���� {0}%�� �߰� ���ظ� �ݴϴ�.
    public static string ����_�� = "���� ��"; // ��Ÿ��� {0} �����մϴ�.
    public static string �����Ŀ = "�����Ŀ"; // �αⰡ �� ���� �����մϴ�.
}

public static class Colors
{
    public static string ���� = "#BB4642";
    public static string �����ѻ��� = "#BF3B3C";
    public static string �������� = "#DD534A";
    public static string ���ѻ��� = "#993E41";
    public static string �帰���� = "#B06F68";
    public static string Ź�ѻ��� = "#935B55";
    public static string ��ο�� = "#633D43";
    public static string ȸ���� = "#927A78";
    public static string ��ο�ȸ���� = "#685351";
    public static string �������� = "#544546";
    public static string ��Ȳ = "#F2773D";
    public static string ��������Ȳ = "#F17845";
    public static string ������Ȳ = "#FF9058";
    public static string ������Ȳ = "#DA6730";
    public static string �帰��Ȳ = "#E0805B";
    public static string Ź����Ȳ = "#C99176";
    public static string ������Ȳ = "#DB5D3B";
    public static string �����ѻ�����Ȳ = "#E15637";
    public static string ����������Ȳ = "#F56E48";
    public static string Ź�ѻ�����Ȳ = "#AC7062";
    public static string �����Ȳ = "#FF982E";
    public static string �����ѳ����Ȳ = "#FF9913";
    public static string ���������Ȳ = "#FFB12B";
    public static string ���ѳ����Ȳ = "#DB8628";
    public static string ���ѳ����Ȳ = "#FFB66A";
    public static string �帰�����Ȳ = "#FABE93";
    public static string Ź�ѳ����Ȳ = "#C28B69";
    public static string ��� = "#FFD10D";
    public static string ���ѳ�� = "#FFBB14";
    public static string ���ѳ�� = "#FCDA94";
    public static string �帰��� = "#E4C77E";
    public static string ���� = "#F3E6C7";
    public static string ȸȲ�� = "#DBCCB0";
    public static string ����ȸȲ�� = "#E5D7BA";
    public static string ���� = "#94C056";
    public static string �����ѿ��� = "#77A832";
    public static string �������� = "#ABD664";
    public static string ���ѿ��� = "#648C35";
    public static string ���ѿ��� = "#B8D38F";
    public static string �帰���� = "#A0BB7F";
    public static string Ź�ѿ��� = "#8AA269";
    public static string ������� = "#D8C949";
    public static string �����ѳ������ = "#E2CD26";
    public static string ����������� = "#EEDA51";
    public static string ���ѳ������ = "#C6B13C";
    public static string ���ѳ������ = "#F3E591";
    public static string �帰������� = "#D2CA80";
    public static string Ź�ѳ������ = "#C3B56B";
    public static string �쿬�� = "#65A854";
    public static string �����ѳ쿬�� = "#5BAC49";
    public static string �����쿬�� = "#7FC26A";
    public static string ���ѳ쿬�� = "#ADD797";
    public static string �帰�쿬�� = "#91BD86";
    public static string Ź�ѳ쿬�� = "#7BA570";
    public static string �򿬵� = "#E0E9CB";
    public static string ȸ���� = "#96A08A";
    public static string ����ȸ���� = "#C6D1B5";
    public static string �ʷ� = "#247C4D";
    public static string �������ʷ� = "#059651";
    public static string �����ʷ� = "#36935B";
    public static string �����ʷ� = "#3C6249";
    public static string �����ʷ� = "#95DAAF";
    public static string �帰�ʷ� = "#6AAA87";
    public static string Ź���ʷ� = "#4A7859";
    public static string ��ο��ʷ� = "#3E4E43";
    public static string ���ʷ� = "#D2EDD5";
    public static string ȸ��� = "#73877A";
    public static string ����ȸ��� = "#BCD0BC";
    public static string ��ο�ȸ��� = "#525D53";
    public static string �����ʷ� = "#454E48";
    public static string û�� = "#006B70";
    public static string ����û�� = "#00979C";
    public static string ����û�� = "#21555A";
    public static string ����û�� = "#66C4C3";
    public static string �帰û�� = "#6FA6A5";
    public static string Ź��û�� = "#437779";
    public static string ��ο�û�� = "#345052";
    public static string ��û�� = "#CEEBE6";
    public static string ȸû�� = "#708889";
    public static string ����ȸû�� = "#9CB8B7";
    public static string ��ο�ȸû�� = "#495E5B";
    public static string ����û�� = "#3E4C4E";
    public static string �Ķ� = "#0F7CA8";
    public static string �������Ķ� = "#008CC3";
    public static string �����Ķ� = "#4AA8D8";
    public static string �����Ķ� = "#344F65";
    public static string �����Ķ� = "#A6D3E9";
    public static string �帰�Ķ� = "#75A6C0";
    public static string Ź���Ķ� = "#4A7691";
    public static string ��ο��Ķ� = "#3A4E5B";
    public static string ���Ķ� = "#DFE9ED";
    public static string ȸû�� = "#7A8790";
    public static string ����ȸû�� = "#ABB6BC";
    public static string ��ο�ȸû�� = "#525E66";
    public static string �����Ķ� = "#444E56";
    public static string ���� = "#414A67";
    public static string �������� = "#445C91";
    public static string �帰���� = "#4D5B7B";
    public static string ��ο�� = "#464D5E";
    public static string ȸ���� = "#696F7A";
    public static string �������� = "#484B54";
    public static string ���� = "#665581";
    public static string �����Ѻ��� = "#7A6397";
    public static string �������� = "#8977AD";
    public static string ���Ѻ��� = "#554466";
    public static string ���Ѻ��� = "#BBABD3";
    public static string �帰���� = "#A699BB";
    public static string Ź�Ѻ��� = "#766789";
    public static string ��ο�� = "#4F4658";
    public static string �򺸶� = "#EAE7EC";
    public static string ȸ���� = "#86828E";
    public static string ����ȸ���� = "#B7B3BD";
    public static string ��ο�ȸ���� = "#625D6A";
    public static string �������� = "#4C4751";
    public static string ���� = "#8B425F";
    public static string ���������� = "#AB5071";
    public static string �������� = "#CD5D85";
    public static string �������� = "#683D51";
    public static string �������� = "#BB6C85";
    public static string �帰���� = "#A97683";
    public static string Ź������ = "#7C4D5E";
    public static string ��ο����� = "#5B444E";
    public static string �������� = "#8B4156";
    public static string �������ڻ� = "#6D3E4D";
    public static string Ź�����ڻ� = "#7B4C58";
    public static string ��ο����ڻ� = "#5D454D";
    public static string ȸ���� = "#928286";
    public static string ��ο�ȸ���� = "#66555A";
    public static string �������� = "#54464B";
    public static string ��ȫ = "#E99EA8";
    public static string ���Ѻ�ȫ = "#DB8291";
    public static string ���Ѻ�ȫ = "#F6BABF";
    public static string �帰��ȫ = "#DDA3AB";
    public static string Ź�Ѻ�ȫ = "#C38C94";
    public static string �����ȫ = "#FF9477";
    public static string ���ѳ����ȫ = "#E77A62";
    public static string ���ѳ����ȫ = "#FFB8A2";
    public static string �帰�����ȫ = "#E6A18F";
    public static string Ź�ѳ����ȫ = "#C18375";
    public static string ���ȫ = "#F5E6E3";
    public static string ȸ��ȫ = "#C2B1B3";
    public static string ����ȸ��ȫ = "#DAC8C9";
    public static string ���޺���ȫ = "#E79EB9";
    public static string �������޺���ȫ = "#D284A3";
    public static string �������޺���ȫ = "#F2C0D1";
    public static string �帰���޺���ȫ = "#D9A8B8";
    public static string Ź�����޺���ȫ = "#BC8D9E";
    public static string ���� = "#975A3E";
    public static string �������� = "#CB6B31";
    public static string ���Ѱ��� = "#78503E";
    public static string ���Ѱ��� = "#AA704A";
    public static string �帰���� = "#A47A68";
    public static string Ź�Ѱ��� = "#8C6354";
    public static string ��ο�� = "#5C443B";
    public static string �������� = "#8B433C";
    public static string ���������� = "#A75544";
    public static string ���������� = "#61413F";
    public static string �帰������ = "#965C51";
    public static string Ź�������� = "#7B4D47";
    public static string ��ο������� = "#584342";
    public static string ������� = "#AE7734";
    public static string ����Ȳ���� = "#D88231";
    public static string ����Ȳ���� = "#C98C4F";
    public static string �帰Ȳ���� = "#B59375";
    public static string Ź��Ȳ���� = "#A77A4D";
    public static string �찥�� = "#796D3D";
    public static string �����찥�� = "#99893C";
    public static string �帰�찥�� = "#92885A";
    public static string Ź�ѳ찥�� = "#746D48";
    public static string ��ο�찥�� = "#5D5A3F";
    public static string ȸ���� = "#927D75";
    public static string ��ο�ȸ���� = "#69554E";
    public static string �������� = "#544843";
    public static string �Ͼ� = "#F2F3F0";
    public static string ����Ͼ� = "#EAE3D4";
    public static string �ʷϺ��Ͼ� = "#DDEDDF";
    public static string �Ķ��Ͼ� = "#E0E8E9";
    public static string �������Ͼ� = "#E9E7E8";
    public static string ��ȫ���Ͼ� = "#EFE5E3";
    public static string ȸ�� = "#818383";
    public static string ����ȸ�� = "#B2B5B3";
    public static string ��ο�ȸ�� = "#5F6261";
    public static string ����ȸ�� = "#8D8382";
    public static string ��ο���ȸ�� = "#5F5353";
    public static string ���ȸ�� = "#D4CBBB";
    public static string �ʷϺ�ȸ�� = "#7E8681";
    public static string ������ȸ�� = "#C2CFC4";
    public static string ��ο��ȸ�� = "#555D59";
    public static string �Ķ�ȸ�� = "#7F878A";
    public static string ����ûȸ�� = "#ADB5B7";
    public static string ��ο�ûȸ�� = "#575C61";
    public static string ������ȸ�� = "#86848A";
    public static string ����������ȸ�� = "#B4B3B7";
    public static string ��ο����ȸ�� = "#5B5960";
    public static string ��ȫ��ȸ�� = "#BBB2B3";
    public static string ��ȸ�� = "#8E817D";
    public static string ��οȸ�� = "#615855";
    public static string ���� = "#3B3B3B";
    public static string �������� = "#4E4747";
    public static string �ʷϺ����� = "#474B49";
    public static string �Ķ����� = "#474C4F";
    public static string ���������� = "#4B494D";
    public static string ����� = "#4D4948";
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