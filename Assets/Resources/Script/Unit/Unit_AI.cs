using System.Collections.Generic;
using UnityEngine;

public class Unit_AI : MonoBehaviour
{
    Unit info;
    public float rayDistance = 5f;   // Ray의 길이
    public LayerMask layerMask;      // 충돌할 레이어 설정

    // 부채꼴
    public float coneAngle = 60f;          // 부채꼴 각도 (전체 각도)
    public int rayCount = 10;              // 발사할 Ray 개수

    private void Update()
    {
        CheckCone();
        CheckLine();
    }

    private void CheckCone()
    {
        float startAngle = -coneAngle / 2;     // 부채꼴 시작 각도
        float angleStep = coneAngle / (rayCount - 1); // 각 Ray 간의 각도 차이

        HashSet<Collider2D> hitColliders = new HashSet<Collider2D>();

        for (int i = 0; i < rayCount; i++)
        {
            // 각도를 기준으로 방향 계산
            float currentAngle = startAngle + (angleStep * i);
            float radian = (currentAngle + transform.eulerAngles.z) * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

            // RaycastAll을 사용하여 모든 충돌체 감지
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, rayDistance, layerMask);

            // 모든 충돌체 검사 및 시각화
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != gameObject)
                {
                    hitColliders.Add(hit.collider);  // 중복 방지
                    Debug.Log($"Hit {hit.collider.name} at angle {currentAngle} degrees");
                }
            }

            Color color = hits.Length > 1 ? Color.green : Color.red;
            Debug.DrawRay(transform.position, direction * rayDistance, color);
        }

        // 부채꼴 범위 내에서 충돌한 오브젝트가 있는지 판단
        if (hitColliders.Count > 0)
        {
            //Debug.Log($"Total unique objects hit in cone: {hitColliders.Count}");
        }
    }

    private void CheckLine()
    {
        Vector2 direction = transform.right; // 오브젝트의 오른쪽(바라보는 방향)
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
