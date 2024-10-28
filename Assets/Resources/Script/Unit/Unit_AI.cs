using System.Collections.Generic;
using UnityEngine;

public class Unit_AI : MonoBehaviour
{
    Unit info;
    public float rayDistance = 5f;   // Ray�� ����
    public LayerMask layerMask;      // �浹�� ���̾� ����

    // ��ä��
    public float coneAngle = 60f;          // ��ä�� ���� (��ü ����)
    public int rayCount = 10;              // �߻��� Ray ����

    private void Update()
    {
        CheckCone();
        CheckLine();
    }

    private void CheckCone()
    {
        float startAngle = -coneAngle / 2;     // ��ä�� ���� ����
        float angleStep = coneAngle / (rayCount - 1); // �� Ray ���� ���� ����

        HashSet<Collider2D> hitColliders = new HashSet<Collider2D>();

        for (int i = 0; i < rayCount; i++)
        {
            // ������ �������� ���� ���
            float currentAngle = startAngle + (angleStep * i);
            float radian = (currentAngle + transform.eulerAngles.z) * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

            // RaycastAll�� ����Ͽ� ��� �浹ü ����
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, rayDistance, layerMask);

            // ��� �浹ü �˻� �� �ð�ȭ
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != gameObject)
                {
                    hitColliders.Add(hit.collider);  // �ߺ� ����
                    Debug.Log($"Hit {hit.collider.name} at angle {currentAngle} degrees");
                }
            }

            Color color = hits.Length > 1 ? Color.green : Color.red;
            Debug.DrawRay(transform.position, direction * rayDistance, color);
        }

        // ��ä�� ���� ������ �浹�� ������Ʈ�� �ִ��� �Ǵ�
        if (hitColliders.Count > 0)
        {
            //Debug.Log($"Total unique objects hit in cone: {hitColliders.Count}");
        }
    }

    private void CheckLine()
    {
        Vector2 direction = transform.right; // ������Ʈ�� ������(�ٶ󺸴� ����)
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + direction * rayDistance;

        RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, direction, rayDistance, layerMask);

        Color color = hits.Length > 1 ? Color.green : Color.red;
        Debug.DrawLine(startPosition, endPosition, color);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                //Debug.Log($"Hit {hit.collider.name} at distance {hit.distance}");
            }
        }
    }
}
