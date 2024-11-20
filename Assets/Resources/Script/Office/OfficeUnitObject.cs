using QUtility;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.U2D;

public class OfficeUnitObject : MonoBehaviour
{
    private static EPrefabType prefabType = EPrefabType.OfficeUnitObject;
    public static OfficeUnitObject Spawn(UnitData unitData)
    {
        GameObject projectileObject = PoolManager.Instance.GetFromPool(prefabType.ToString());
        OfficeUnitObject officeUnitObject = projectileObject.GetComponent<OfficeUnitObject>();
        officeUnitObject.Set(unitData);

        return officeUnitObject;
    }

    public float speed = 2f; // �̵� �ӵ�
    private Vector2 currentPoint; // ���� ��ġ
    private Vector2 nextPoint; // ���� ������
    private Vector2 previousPoint; // ���� ��ġ
    private bool isMoving = true; // �̵� ���� �÷���
    private Animator animator;
    private SpriteRenderer sprite;
    public string unitUniqueID;

    public Transform MessageBox;
    public SpriteRenderer MessageBox_SpriteRenderer;
    public TextMeshPro MessageText;

    private void Awake()
    {
        animator = UIUtility.FindComponentInChildrenByName<Animator>(gameObject, "Sprite");
        sprite = UIUtility.FindComponentInChildrenByName<SpriteRenderer>(gameObject, "Sprite");
        MessageBox = UIUtility.FindComponentInChildrenByName<Transform>(gameObject, "MessageBox");
        MessageBox_SpriteRenderer = UIUtility.FindComponentInChildrenByName<SpriteRenderer>(gameObject, "MessageBox");
        MessageText = UIUtility.FindComponentInChildrenByName<TextMeshPro>(gameObject, "MessageText");

    }

    public void Set(UnitData unitData)
    {
        MessageBox.gameObject.SetActive(false);
        MessageText.gameObject.SetActive(false);

        unitUniqueID = unitData.unitUniqueID;

        var controller = Resources.Load<RuntimeAnimatorController>($"Animation/UnitAnimation/{unitData.unitInfo_Immutable.Animator}/{unitData.unitInfo_Immutable.Animator}");
        animator.runtimeAnimatorController = controller;
        animator.Play("Move");

        ShowMessage($"�ȳ��ϼ��� ������� {unitData.unitInfo_Immutable.Name}�Դϴ�.");
    }

    void Start()
    {
        InitializeStartPoint();
        SetNextPoint();
    }

    void Update()
    {
        if (!isMoving) return;

        Vector2 direction = nextPoint - (Vector2)transform.position;
        transform.position = Vector2.MoveTowards(transform.position, nextPoint, Time.deltaTime * speed);

        if (direction.x > 0)
        {
            sprite.flipX = true;
        }
        else if (direction.x < 0)
        {
            sprite.flipX = false;
        }

        if (Vector2.Distance(transform.position, nextPoint) < 0.1f)
        {
            StartCoroutine(WaitBeforeNextMove());
        }
    }

    private void InitializeStartPoint()
    {
        if (LineManager.Instance.graph.Count > 0)
        {
            var graphKeys = new List<Vector2>(LineManager.Instance.graph.Keys); // Ű�� ����Ʈ�� ��ȯ
            var randomIndex = Random.Range(0, graphKeys.Count); // ������ �ε��� ����
            var randomKey = graphKeys[randomIndex]; // ������ Ű ����

            currentPoint = randomKey; // ������ Ű�� ���� �������� ����
            transform.position = currentPoint; // ��ü�� �ش� ��ġ�� �̵�
            previousPoint = currentPoint; // ���� ��ġ�� �ʱ�ȭ
        }
    }

    private IEnumerator WaitBeforeNextMove()
    {
        isMoving = false; // �̵� ����

        var rand = Random.Range(0, 100);
        if (rand < 20)
        {
            animator.Play("Idle");
            yield return new WaitForSeconds(Random.Range(0.5f, 3f)); // 3~5�� ���
            animator.Play("Move");
        }
        currentPoint = nextPoint; // ���� ��ġ�� ����
        SetNextPoint(); // ���ο� ������ ����
        isMoving = true; // �̵� �簳
    }

    private void SetNextPoint()
    {
        List<Vector2> connectedPoints = LineManager.Instance.graph[currentPoint]; // ���� ��ġ���� ����� ���� ��������

        // �̵� ������ ���� ���� ���, ���� �������� ���ư�
        if (connectedPoints.Count == 1 && connectedPoints[0] == previousPoint)
        {
            nextPoint = previousPoint;
            previousPoint = currentPoint; // ���� ��ġ�� ������Ʈ
            return;
        }

        // ����� ���� 1���� ��� ������ ����
        if (connectedPoints.Count == 1)
        {
            nextPoint = connectedPoints[0];
            return;
        }

        // ����� ���� 2�� �̻��� ��� ���� �� �����ϰ� ����
        do
        {
            nextPoint = connectedPoints[Random.Range(0, connectedPoints.Count)];
        } while (connectedPoints.Count > 1 && nextPoint == previousPoint);

        previousPoint = currentPoint; // ���� ��ġ ������Ʈ
    }

    public void ReturnToPool()
    {
        PoolManager.Instance.ReturnToPool(prefabType.ToString(), gameObject);
    }

    public void ShowMessage(string message)
    {
        StartCoroutine(DisplayMessageCoroutine(message));
    }

    private IEnumerator DisplayMessageCoroutine(string message)
    {
        // �޽��� ��� ����
        MessageBox.gameObject.SetActive(true);
        MessageText.gameObject.SetActive(true);

        MessageText.text = ""; // �ؽ�Ʈ �ʱ�ȭ
        for (int i = 0; i < message.Length; i++)
        {
            MessageText.text += message[i]; // 1���ھ� �߰�
            AdjustMessageBoxSize(); // MessageBox ũ�� ����
            yield return new WaitForSeconds(0.05f); // 0.02�� ���
        }

        // ��� ���� ��� �� 1�� ���
        yield return new WaitForSeconds(2.5f);

        // �޽��� �����
        MessageBox.gameObject.SetActive(false);
        MessageText.gameObject.SetActive(false);

        // ���� �޽��� ��� ��� �ð� (5~25�� ����)
        float waitTime = Random.Range(5f, 15f);
        yield return new WaitForSeconds(waitTime);

        // �ٽ� �޽��� ��� (������ �޽����� ����ϰų� �Ű������� �ݺ������� ���)
        ShowMessage(message);
    }

    private Vector2 padding = new Vector2(0.2f, 0.1f); // �ؽ�Ʈ �ܺ��� ���� (x: �¿�, y: ���Ʒ�)

    private void AdjustMessageBoxSize()
    {
        if (MessageText == null || MessageBox == null) return;

        // TextMeshPro�� Preferred Values ���
        float textWidth = MessageText.renderedWidth;  // ���� �������� ���� ����
        float textHeight = MessageText.renderedHeight; // ���� �������� ���� ����

        // MessageBox ũ�� ���
        Vector2 newSize = new Vector2(
            Mathf.Max(textWidth + padding.x * 2, 0.5f), // �ؽ�Ʈ �ʺ� + �¿� ����
            Mathf.Max(textHeight + padding.y * 2, 0.2f) // �ؽ�Ʈ ���� + ���Ʒ� ����
        );

        // SpriteRenderer ũ�� ����
        SpriteRenderer messageBoxRenderer = MessageBox.GetComponent<SpriteRenderer>();
        if (messageBoxRenderer != null)
        {
            messageBoxRenderer.drawMode = SpriteDrawMode.Sliced; // Sliced ��� ����
            messageBoxRenderer.size = newSize; // ũ�� ����
        }

        // MessageBox ��ġ ���� (�ؽ�Ʈ �߽ɿ� ���߱�)
        MessageBox.localPosition = new Vector3(
            MessageText.transform.localPosition.x,
            MessageText.transform.localPosition.y,
            MessageBox.localPosition.z // ���� Z�� �� ����
        );
    }

}
