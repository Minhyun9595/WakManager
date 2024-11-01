using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QUtility
{
    public static class UIUtility
    {
        public static string GetTeamTextColor(int teamIndex)
        {
            switch (teamIndex)
            {
                case 0:
                    return "<color=red>";
                case 1:
                    return "<color=blue>";
                case 2:
                    return "<color=green>";
                case 3:
                    return "<color=yellow>";
                default:
                    return "<color=white>";
            }
        }

        public static void DrawDebugCircle(Vector3 center, float radius, Color color, int segments = 12)
        {
            float angleStep = 360f / segments;
            for (int i = 0; i < segments; i++)
            {
                float angle1 = Mathf.Deg2Rad * angleStep * i;
                float angle2 = Mathf.Deg2Rad * angleStep * (i + 1);

                Vector3 point1 = center + new Vector3(Mathf.Cos(angle1), Mathf.Sin(angle1), 0) * radius;
                Vector3 point2 = center + new Vector3(Mathf.Cos(angle2), Mathf.Sin(angle2), 0) * radius;

                Debug.DrawLine(point1, point2, color, 0.1f); // 각 라인 지속 시간
            }
        }

        public static Color GetDamageColor(bool isCritical)
        {
            Color color;
            string hexCode = "#0079FFFF";
            if (isCritical)
            {
                hexCode = "#D7790CFF";
            }

            if (ColorUtility.TryParseHtmlString(hexCode, out color))
            {
                return color;
            }

            return Color.white;
        }

    }

}