using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QUtility
{
    public static class MathUtility
    {
        public static Vector3 GetRandomVector3_XY(float minX, float maxX, float minY, float maxY)
        {
            return new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
        }

        public static List<int> GetRandomIndices(int totalCount, int count)
        {
            List<int> indices = new List<int>();

            // 1부터 N까지의 숫자를 리스트에 추가
            List<int> availableIndices = new List<int>();
            for (int i = 0; i < totalCount; i++)
            {
                availableIndices.Add(i);
            }

            // 무작위로 인덱스 뽑기
            for (int i = 0; i < count && availableIndices.Count > 0; i++)
            {
                int index = Random.Range(0, availableIndices.Count);
                indices.Add(availableIndices[index]);
                availableIndices.RemoveAt(index); // 중복 방지를 위해 선택된 인덱스 제거
            }

            return indices;
        }

        public static IEnumerable<Unit_AI> GetAllEnemiesInRange(Unit_AI _myUnitAI, int _myTeamIndex, Vector3 _startPosition, float _range)
        {
            List<Unit_AI> enemiesInRange = new List<Unit_AI>();

            // 탐색 반경 내의 모든 Collider 가져오기
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_startPosition, _range);

            UIUtility.DrawDebugCircle(_startPosition, _range, Color.yellow);
            foreach (Collider2D collider in colliders)
            {
                Unit_AI enemyUnit = collider.GetComponent<Unit_AI>();
                if (enemyUnit != null &&
                    enemyUnit != _myUnitAI &&
                    enemyUnit.blackboard.unitFieldInfo.IsCanNotTarget() == false &&
                    enemyUnit.GetBlackboard().teamIndex != _myTeamIndex) // 자신은 제외 // 같은 팀은 제외 // 죽은 유닛 제외
                {
                    enemiesInRange.Add(enemyUnit);
                }
            }

            return enemiesInRange;
        }
    }
}
