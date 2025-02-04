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

            // 1���� N������ ���ڸ� ����Ʈ�� �߰�
            List<int> availableIndices = new List<int>();
            for (int i = 0; i < totalCount; i++)
            {
                availableIndices.Add(i);
            }

            // �������� �ε��� �̱�
            for (int i = 0; i < count && availableIndices.Count > 0; i++)
            {
                int index = Random.Range(0, availableIndices.Count);
                indices.Add(availableIndices[index]);
                availableIndices.RemoveAt(index); // �ߺ� ������ ���� ���õ� �ε��� ����
            }

            return indices;
        }

        public static IEnumerable<Unit_AI> GetAllEnemiesInRange(Unit_AI _myUnitAI, int _myTeamIndex, Vector3 _startPosition, float _range)
        {
            List<Unit_AI> enemiesInRange = new List<Unit_AI>();

            // Ž�� �ݰ� ���� ��� Collider ��������
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_startPosition, _range);

            UIUtility.DrawDebugCircle(_startPosition, _range, Color.yellow);
            foreach (Collider2D collider in colliders)
            {
                Unit_AI enemyUnit = collider.GetComponent<Unit_AI>();
                if (enemyUnit != null &&
                    enemyUnit != _myUnitAI &&
                    enemyUnit.blackboard.unitFieldInfo.IsCanNotTarget() == false &&
                    enemyUnit.GetBlackboard().teamIndex != _myTeamIndex) // �ڽ��� ���� // ���� ���� ���� // ���� ���� ����
                {
                    enemiesInRange.Add(enemyUnit);
                }
            }

            return enemiesInRange;
        }
    }
}
