using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        DataTable.Instance.Initialize();

        var panel = PanelManager.Instance;
        var poolManager = PoolManager.Instance;
        var fieldManager = FieldManager.Instance;

        InitMapData();
        fieldManager.StartGame();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1) && SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2) && SceneManager.GetActiveScene().buildIndex != 1)
        {
            SceneManager.LoadScene(1);
        }
    }

    public GameObject StageBG;

    private Vector3 center;               // �߾� ��ġ
    private float leftBound, rightBound;  // �¿� ��� ��
    private float topBound, bottomBound;  // ���� ��� ��

    private void InitMapData()
    {
        // ���� SpriteRenderer ������Ʈ ��������
        if (StageBG == null)
            return;

        SpriteRenderer spriteRenderer = StageBG.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer�� �����ϴ�!");
            return;
        }

        // Bounds ���
        Bounds bounds = spriteRenderer.bounds;
        center = bounds.center;
        leftBound = bounds.min.x;
        rightBound = bounds.max.x;
        bottomBound = bounds.min.y;
        topBound = bounds.max.y;
    }

    // �� ���� �� ���ʿ��� ���� ��ǥ ��ȯ
    public Vector3 GetRandomLeftPosition()
    {
        float randomX = Random.Range(leftBound, center.x);
        float randomY = Random.Range(bottomBound, topBound);
        return new Vector3(randomX, randomY, 0);
    }

    // �� ���� �� �����ʿ��� ���� ��ǥ ��ȯ
    public Vector3 GetRandomRightPosition()
    {
        float randomX = Random.Range(center.x, rightBound);
        float randomY = Random.Range(bottomBound, topBound);
        return new Vector3(randomX, randomY, 0);
    }

    // �� ������ ���� ��ǥ ��ȯ
    public Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(leftBound, rightBound);
        float randomY = Random.Range(bottomBound, topBound);
        return new Vector3(randomX, randomY, 0);
    }

    // �� ������ ������ �ʵ��� �����ϴ� �Լ�
    public Vector3 ClampPosition(Vector3 position)
    {
        float clampedX = Mathf.Clamp(position.x, leftBound, rightBound);
        float clampedY = Mathf.Clamp(position.y, bottomBound, topBound);
        return new Vector3(clampedX, clampedY, position.z);
    }

    private void OnDrawGizmos()
    {
        // SpriteRenderer ������Ʈ ��������
        if (StageBG == null)
            return;

        SpriteRenderer spriteRenderer = StageBG.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            return;

        // Sprite�� Bounds (���) ��������
        Bounds bounds = spriteRenderer.bounds;

        // Gizmo ���� ����
        Gizmos.color = Color.green;

        // �����¿� ��ǥ ���
        Vector3 topLeft = new Vector3(bounds.min.x, bounds.max.y, 0);
        Vector3 topRight = new Vector3(bounds.max.x, bounds.max.y, 0);
        Vector3 bottomLeft = new Vector3(bounds.min.x, bounds.min.y, 0);
        Vector3 bottomRight = new Vector3(bounds.max.x, bounds.min.y, 0);

        // �簢�� �׸���
        Gizmos.DrawLine(topLeft, topRight);     // ���� ��
        Gizmos.DrawLine(topRight, bottomRight); // ������ ��
        Gizmos.DrawLine(bottomRight, bottomLeft); // �Ʒ��� ��
        Gizmos.DrawLine(bottomLeft, topLeft);    // ���� ��
    }
}
