using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QUtility
{
    public static class UIUtility
    {
        public static string GetTeamRichTextColor(int teamIndex)
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

        public static Color GetTeamColor(int teamIndex)
        {
            switch (teamIndex)
            {
                case 0:
                    return Color.red;
                case 1:
                    return Color.blue;
                default:
                    return Color.white;
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

                Debug.DrawLine(point1, point2, color, Time.deltaTime); // 각 라인 지속 시간
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

        public static T FindComponentInChildrenByName<T>(GameObject parent, string childName) where T : Component
        {
            Transform[] children = parent.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in children)
            {
                if (child.name == childName)
                {
                    T component = child.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }

            Debug.LogWarning($"자식 오브젝트 '{childName}' 또는 해당 컴포넌트를 찾을 수 없습니다.");
            return null;
        }

        public static GameObject GetChildAutoCraete(Transform parent, int childIndex)
        {
            if (childIndex < parent.childCount)
            {
                // 요청한 인덱스에 자식이 존재하면 해당 자식을 반환
                return parent.GetChild(childIndex).gameObject;
            }
            else if (parent.childCount > 0)
            {
                // 첫 번째 자식을 복제하여 추가하고 반환
                GameObject clonedChild = Object.Instantiate(parent.GetChild(0).gameObject, parent);
                clonedChild.transform.SetSiblingIndex(childIndex);
                return clonedChild;
            }
            else
            {
                Debug.LogWarning("부모 오브젝트에 자식이 존재하지 않습니다.");
                return null;
            }
        }

        public static Sprite GetSprite(string iconSprite)
        {
            var sprite = Resources.Load<Sprite>($"Sprite/icon/{iconSprite}");
            return sprite;
        }

        public static Color HexToColor(string hex)
        {
            if (hex.StartsWith("#"))
            {
                hex = hex.Substring(1);
            }

            if (hex.Length != 6)
            {
                Debug.LogWarning("Invalid hex color length. Expected 6 characters (e.g., '00AB40').");
                return Color.black;
            }

            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            return new Color32(r, g, b, 255);
        }

        public static string GetUnitizeText(int unit)
        {
            string formattedMoney;

            if (unit >= 10_000_000) // 1000만 이상이면 M 단위
            {
                formattedMoney = $"{unit / 10_000_000f:0.##}M";
            }
            else if (unit >= 10_000) // 10000 이상이면 K 단위
            {
                formattedMoney = $"{unit / 10_000f:0.##}K";
            }
            else // 1000 미만이면 그냥 숫자 표시
            {
                formattedMoney = unit.ToString();
            }

            return formattedMoney;
        }
    }

}