using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Skill_Assassin_Jump_Node : BehaviorNode
{
    private Blackboard blackboard;
    private float jumpDuration = 1.0f; // ���� ���� �ð� (��)
    private float maxJumpHeight = 2.0f; // ������ �ִ� ����
    private bool isJumping = false;
    private NodeStatus currentStatus = NodeStatus.Failure; // �⺻ ���¸� Failure�� �ʱ�ȭ
    private Material originalMaterial; // ���� ���׸��� ����
    private Material jumpMaterial; // ������ ���׸���
    public Skill_Assassin_Jump_Node(Blackboard bb)
    {
        blackboard = bb;

        jumpMaterial = Resources.Load<Material>("Materials/Assasin_Jump");
        if (jumpMaterial == null)
        {
            Debug.LogError("Assasin_Jump material not found in Resources/Materials.");
        }

        Renderer renderer = blackboard.GetBodyRenderer();
        if (renderer != null)
        {
            originalMaterial = renderer.material; // ���� ���׸��� ����
        }
    }

    public override NodeStatus Execute()
    {
        if (isJumping)
        {
            blackboard.unitAnimator.SetAnimation(EAnimationType.Move);

            if(currentStatus == NodeStatus.Failure)
            {
                isJumping = false;
            }

            return currentStatus;
        }

        blackboard.unitFieldInfo.isNoneTargeting = true;

        // ���� �� �� ã��
        var enemySpawnUnit = FieldManager.Instance.GetEnemySpawnedUnit_ByTeamIndex(blackboard.teamIndex);
        if (enemySpawnUnit.units == null || enemySpawnUnit.units.Count == 0)
            return NodeStatus.Failure;

        Unit_AI farthestEnemy = null;
        if (blackboard.teamIndex % 2 == 0) // �ִ� X ã��
        {
            farthestEnemy = enemySpawnUnit.units.OrderByDescending(x => x.transform.position.x).FirstOrDefault();
        }
        else // �ּ� X ã��
        {
            farthestEnemy = enemySpawnUnit.units.OrderBy(x => x.transform.position.x).FirstOrDefault();
        }

        if (farthestEnemy == null)
            return NodeStatus.Failure;

        // ��ǥ ��ġ ���
        Vector3 startPosition = blackboard.myUnitAI.transform.position;
        Vector3 targetPosition = farthestEnemy.transform.position + new Vector3(1.0f * (blackboard.teamIndex % 2 == 0 ? -1 : 1), 0, 0); // �� ����
        targetPosition.y = startPosition.y;

        // ���׸��� ����
        Renderer renderer = blackboard.GetBodyRenderer();
        if (renderer != null)
        {
            if (jumpMaterial != null)
            {
                renderer.material = jumpMaterial; // ���� ���׸���� ����
            }
        }

        // �ڷ�ƾ ����
        isJumping = true;
        currentStatus = NodeStatus.Running; // ���¸� Running���� ����
        blackboard.myUnitAI.StartCoroutine(JumpToTarget(startPosition, targetPosition, jumpDuration, renderer)); // 1�� ���� ����
        return currentStatus;
    }

    private IEnumerator JumpToTarget(Vector3 start, Vector3 target, float duration, Renderer renderer)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += CustomTime.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // 0���� 1������ ����

            // XZ ��ġ ���� ����
            Vector3 currentPosition = Vector3.Lerp(start, target, t);

            // Y ��ġ ������ ���
            float height = maxJumpHeight * (1 - Mathf.Pow(2 * t - 1, 2)); // ������ ���� ���
            currentPosition.y += height;

            // ĳ���� ��ġ ������Ʈ
            blackboard.myUnitAI.transform.position = currentPosition;

            yield return null; // ���� �����ӱ��� ���
        }

        // ���׸��� ����
        if (renderer != null && originalMaterial != null)
        {
            renderer.material = originalMaterial;
        }

        // ���� �Ϸ� ó��
        blackboard.myUnitAI.transform.position = target;
        blackboard.unitFieldInfo.isNoneTargeting = false;
        currentStatus = NodeStatus.Failure;
    }
}
