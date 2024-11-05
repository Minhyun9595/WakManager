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

                Debug.DrawLine(point1, point2, color, 0.2f); // �� ���� ���� �ð�
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
            Transform[] children = parent.GetComponentsInChildren<Transform>();

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

            Debug.LogWarning($"�ڽ� ������Ʈ '{childName}' �Ǵ� �ش� ������Ʈ�� ã�� �� �����ϴ�.");
            return null;
        }

        public static GameObject GetChildAutoCraete(Transform parent, int childIndex)
        {
            if (childIndex < parent.childCount)
            {
                // ��û�� �ε����� �ڽ��� �����ϸ� �ش� �ڽ��� ��ȯ
                return parent.GetChild(childIndex).gameObject;
            }
            else if (parent.childCount > 0)
            {
                // ù ��° �ڽ��� �����Ͽ� �߰��ϰ� ��ȯ
                GameObject clonedChild = Object.Instantiate(parent.GetChild(0).gameObject, parent);
                clonedChild.transform.SetSiblingIndex(childIndex);
                return clonedChild;
            }
            else
            {
                Debug.LogWarning("�θ� ������Ʈ�� �ڽ��� �������� �ʽ��ϴ�.");
                return null;
            }
        }
    }

}