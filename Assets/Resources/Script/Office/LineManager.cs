using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class LineManager : MonoBehaviour
{
    public static LineManager _lineManager;
    public static LineManager Instance { get
        {
            if (_lineManager == null)
            {
                _lineManager = FindObjectOfType<LineManager>();
            }
            return _lineManager;
        } 
    }
    public Color lineColor = Color.green; // Line ����
    public float lineWidth = 0.02f; // LineRenderer�� ��
    public float snapInterval = 0.3f; // XY ��ǥ ���� ����

    private Vector3? lineStart = null; // Line�� ���� ����
    private Vector3? lineEnd = null;   // Line�� �� ����

    public Dictionary<Vector2, List<Vector2>> graph = new Dictionary<Vector2, List<Vector2>>(); // �׷��� ������
    private List<GameObject> lines = new List<GameObject>(); // ������ LineRenderer ������Ʈ ���

    public void Awake()
    {
        string csv = "-0.75,1.05,-1.5,-0.15\n-0.75,1.05,-0.6,-0.3\n-0.75,1.05,0.9,0.45\n-1.5,-0.15,-0.75,1.05\n-1.5,-0.15,0.6,-0.9\n0.6,-0.9,-1.5,-0.15\n0.6,-0.9,-0.6,-0.3\n0.6,-0.9,0.9,0.45\n-0.6,-0.3,0.6,-0.9\n-0.6,-0.3,-0.75,1.05\n0.9,0.45,-0.75,1.05\n0.9,0.45,0.6,-0.9\n"; // ���⿡ CSV ���ڿ��� �ٿ��ֱ�
        LoadGraphFromCSV(csv);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            HandleLineCreation();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ClearAllLines();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            lineStart = null;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            LogGraphAsCSV(); // �׷��� �����͸� CSV �������� ���
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            string csv = "-0.75,1.05,-1.5,-0.15\n-0.75,1.05,-0.6,-0.3\n-0.75,1.05,0.9,0.45\n-1.5,-0.15,-0.75,1.05\n-1.5,-0.15,0.6,-0.9\n0.6,-0.9,-1.5,-0.15\n0.6,-0.9,-0.6,-0.3\n0.6,-0.9,0.9,0.45\n-0.6,-0.3,0.6,-0.9\n-0.6,-0.3,-0.75,1.05\n0.9,0.45,-0.75,1.05\n0.9,0.45,0.6,-0.9\n"; // ���⿡ CSV ���ڿ��� �ٿ��ֱ�
            LoadGraphFromCSV(csv);
        }
    }

    private void HandleLineCreation()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        Vector3 snappedPosition = SnapToGrid(mouseWorldPosition);

        // LineStart ����
        if (lineStart == null)
        {
            lineStart = snappedPosition;
            Debug.Log("Line Start set to: " + lineStart.Value);
        }
        // LineEnd ���� �� Line ����
        else if (lineEnd == null)
        {
            lineEnd = snappedPosition;
            Debug.Log("Line End set to: " + lineEnd.Value);

            CreateLine(lineStart.Value, lineEnd.Value);

            // �׷��� ������Ʈ
            UpdateGraph(lineStart.Value, lineEnd.Value);

            // �ʱ�ȭ
            lineStart = null;
            lineEnd = null;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // ȭ�� ��ǥ���� ���� ��ǥ�� ��ȯ
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane; // ī�޶��� Ŭ�� �÷��� �Ÿ� ����
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        // XY ��ǥ�� ���� �������� ����
        float snappedX = Mathf.Round(position.x / snapInterval) * snapInterval;
        float snappedY = Mathf.Round(position.y / snapInterval) * snapInterval;
        return new Vector3(snappedX, snappedY, position.z);
    }

    private void CreateLine(Vector3 start, Vector3 end)
    {
        // �� GameObject ���� �� LineRenderer �߰�
        GameObject lineObject = new GameObject("Line");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        // LineRenderer ����
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2; // �� ������ ������ ��
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // �⺻ ���̴�
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        // ������ ���� ��Ͽ� �߰�
        lines.Add(lineObject);
    }

    private void UpdateGraph(Vector3 start, Vector3 end)
    {
        // Vector3�� Vector2�� ��ȯ (Z�� ����)
        Vector2 startPoint = new Vector2(start.x, start.y);
        Vector2 endPoint = new Vector2(end.x, end.y);

        // ���� ������ �׷����� �߰�
        if (!graph.ContainsKey(startPoint))
        {
            graph[startPoint] = new List<Vector2>();
        }

        // �� ������ �׷����� �߰�
        if (!graph.ContainsKey(endPoint))
        {
            graph[endPoint] = new List<Vector2>();
        }

        // ����� ���� �߰�
        if (!graph[startPoint].Contains(endPoint))
        {
            graph[startPoint].Add(endPoint);
        }

        if (!graph[endPoint].Contains(startPoint))
        {
            graph[endPoint].Add(startPoint);
        }
    }

    private void LogGraphAsCSV()
    {
        StringBuilder csvBuilder = new StringBuilder();
        foreach (var node in graph)
        {
            foreach (var connection in node.Value)
            {
                csvBuilder.AppendLine($"{node.Key.x},{node.Key.y},{connection.x},{connection.y}");
            }
        }
        Debug.Log($"Graph Data (CSV):\n{csvBuilder.ToString()}");
    }

    private void LoadGraphFromCSV(string csv)
    {
        // �׷��� ������ �ʱ�ȭ
        graph.Clear();
        ClearAllLines();

        string[] lines = csv.Split('\n');
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = line.Split(',');
            if (values.Length != 4) continue;

            float x1 = float.Parse(values[0]);
            float y1 = float.Parse(values[1]);
            float x2 = float.Parse(values[2]);
            float y2 = float.Parse(values[3]);

            Vector2 startPoint = new Vector2(x1, y1);
            Vector2 endPoint = new Vector2(x2, y2);

            // �׷����� �߰�
            if (!graph.ContainsKey(startPoint))
            {
                graph[startPoint] = new List<Vector2>();
            }
            if (!graph[startPoint].Contains(endPoint))
            {
                graph[startPoint].Add(endPoint);
            }

            if (!graph.ContainsKey(endPoint))
            {
                graph[endPoint] = new List<Vector2>();
            }
            if (!graph[endPoint].Contains(startPoint))
            {
                graph[endPoint].Add(startPoint);
            }

            // ���� ����
            //CreateLine(new Vector3(x1, y1, 0), new Vector3(x2, y2, 0));
        }

        Debug.Log("Graph loaded from CSV.");
    }

    private void OnDrawGizmos()
    {
        // LineStart�� LineEnd ���� �� Gizmo�� ǥ��
        if (lineStart != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lineStart.Value, 0.3f);
        }

        if (lineEnd != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(lineEnd.Value, 0.3f);
        }

        // �׷��� ���ἱ ǥ��
        Gizmos.color = Color.yellow;
        foreach (var node in graph)
        {
            foreach (var connection in node.Value)
            {
                Gizmos.DrawLine(new Vector3(node.Key.x, node.Key.y, 0), new Vector3(connection.x, connection.y, 0));
            }
        }
    }

    public void ClearAllLines()
    {
        // ������ ��� ������ ����
        foreach (var line in lines)
        {
            Destroy(line);
        }
        lines.Clear();

        // �׷��� �ʱ�ȭ
        graph.Clear();

        Debug.Log("All lines and graph cleared.");
    }
}
