using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SPUM_PlayerManager))]
public class SPUM_PlayerManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SPUM_PlayerManager manager = (SPUM_PlayerManager)target;

        if (GUILayout.Button("CREATE UNIT"))
        {
            manager.GetPlayerList();
        }
        if (GUILayout.Button("Align UNIT"))
        {
            manager.SetAlignUnits();
        }
        if (GUILayout.Button("CLEAR UNIT"))
        {
            manager.ClearPlayerList();
        }
        if (GUILayout.Button("CAPTURE UNITS"))
        {
            manager.SetScreenShot();
            AssetDatabase.Refresh();
        }
    }
}