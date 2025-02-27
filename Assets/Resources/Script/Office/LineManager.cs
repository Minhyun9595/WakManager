using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

// 테스트용
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
    public Color startlineColor = Color.green; // Line 색상
    public Color endlineColor = Color.red; // Line 색상
    public float lineWidth = 0.02f; // LineRenderer의 폭
    public float snapInterval = 0.3f; // XY 좌표 스냅 간격

    private Vector3? lineStart = null; // Line의 시작 지점
    private Vector3? lineEnd = null;   // Line의 끝 지점

    public Dictionary<Vector2, List<Vector2>> graph = new Dictionary<Vector2, List<Vector2>>(); // 그래프 데이터
    private List<GameObject> lines = new List<GameObject>(); // 생성된 LineRenderer 오브젝트 목록

    public void Awake()
    {
        string csv = "-34,-8.5,-29,-8.5\n-29,-8.5,-34,-8.5\n-29,-8.5,-30,-8\n-29,-8.5,-26,-8.5\n-30,-8,-29,-8.5\n-26,-8.5,-29,-8.5\n-26,-8.5,-26.5,-8\n-26,-8.5,-23.5,-8.5\n-26.5,-8,-26,-8.5\n-23.5,-8.5,-26,-8.5\n-23.5,-8.5,-18,-5.5\n-23.5,-8.5,-19,-8.5\n-18,-5.5,-23.5,-8.5\n-18,-5.5,-14.5,-5.5\n-18,-5.5,-19,-5\n-18,-5.5,-23.5,-5.5\n-14.5,-5.5,-18,-5.5\n-19,-5,-18,-5.5\n-23.5,-5.5,-18,-5.5\n-23.5,-5.5,-33.5,-5.5\n-23.5,-5.5,-30,-2.5\n-33.5,-5.5,-23.5,-5.5\n-33.5,-5.5,-40.5,-5.5\n-33.5,-5.5,-34.5,-5\n-33.5,-5.5,-32.5,-5\n-40.5,-5.5,-33.5,-5.5\n-40.5,-5.5,-41.5,-5.5\n-34.5,-5,-33.5,-5.5\n-32.5,-5,-33.5,-5.5\n-30,-2.5,-23.5,-5.5\n-30,-2.5,-25,-2.5\n-30,-2.5,-34,-2.5\n-25,-2.5,-30,-2.5\n-25,-2.5,-32,0.5\n-25,-2.5,-19,-2.5\n-32,0.5,-25,-2.5\n-32,0.5,-23.5,0.5\n-32,0.5,-36.5,0.5\n-23.5,0.5,-32,0.5\n-23.5,0.5,-19,0.5\n-23.5,0.5,-25.5,3.5\n-19,0.5,-23.5,0.5\n-19,0.5,-14.5,0.5\n-19,0.5,-19.5,1\n-14.5,0.5,-19,0.5\n-14.5,0.5,-15,1\n-19.5,1,-19,0.5\n-15,1,-14.5,0.5\n-19,-2.5,-25,-2.5\n-19,-2.5,-14.5,-2.5\n-19,-2.5,-19,-2\n-14.5,-2.5,-19,-2.5\n-19,-2,-19,-2.5\n-34,-2.5,-30,-2.5\n-34,-2.5,-37.5,-2.5\n-34,-2.5,-34.5,-2\n-37.5,-2.5,-34,-2.5\n-37.5,-2.5,-41,-2.5\n-37.5,-2.5,-37.5,-2\n-41,-2.5,-37.5,-2.5\n-34.5,-2,-34,-2.5\n-37.5,-2,-37.5,-2.5\n-36.5,0.5,-32,0.5\n-36.5,0.5,-41,0.5\n-41,0.5,-36.5,0.5\n-19,-8.5,-23.5,-8.5\n-19,-8.5,-16,-8.5\n-16,-8.5,-19,-8.5\n-16,-8.5,-14.5,-8.5\n-14.5,-8.5,-16,-8.5\n-14.5,-8.5,-15.5,-9\n-15.5,-9,-14.5,-8.5\n-15.5,-9,-28.5,-9\n-28.5,-9,-15.5,-9\n-28.5,-9,-35.5,-9\n-35.5,-9,-28.5,-9\n-35.5,-9,-40.5,-9\n-40.5,-9,-35.5,-9\n-40.5,-9,-42.5,-8\n-40.5,-9,-38.5,-7.5\n-40.5,-9,-40.5,-8\n-42.5,-8,-40.5,-9\n-38.5,-7.5,-40.5,-9\n-40.5,-8,-40.5,-9\n-41.5,-5.5,-40.5,-5.5\n-41.5,-5.5,-40.5,-5\n-40.5,-5,-41.5,-5.5\n-25.5,3.5,-23.5,0.5\n-25.5,3.5,-32.5,3.5\n-25.5,3.5,-21,3.5\n-32.5,3.5,-25.5,3.5\n-32.5,3.5,-38,3.5\n-38,3.5,-32.5,3.5\n-38,3.5,-41,3.5\n-41,3.5,-38,3.5\n-21,3.5,-25.5,3.5\n-21,3.5,-14,3.5\n-21,3.5,-20,4\n-14,3.5,-21,3.5\n-20,4,-21,3.5\n";
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
            LogGraphAsCSV(); // 그래프 데이터를 CSV 형식으로 출력
        }
    }

    private void HandleLineCreation()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        Vector3 snappedPosition = SnapToGrid(mouseWorldPosition);

        // LineStart 설정
        if (lineStart == null)
        {
            lineStart = snappedPosition;
            Debug.Log("Line Start set to: " + lineStart.Value);
        }
        // LineEnd 설정 및 Line 생성
        else if (lineEnd == null)
        {
            lineEnd = snappedPosition;
            Debug.Log("Line End set to: " + lineEnd.Value);

            CreateLine(lineStart.Value, lineEnd.Value);

            // 그래프 업데이트
            UpdateGraph(lineStart.Value, lineEnd.Value);

            // 초기화
            lineStart = null;
            lineEnd = null;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // 화면 좌표에서 월드 좌표로 변환
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane; // 카메라의 클립 플레인 거리 설정
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        // XY 좌표를 스냅 간격으로 맞춤
        float snappedX = Mathf.Round(position.x / snapInterval) * snapInterval;
        float snappedY = Mathf.Round(position.y / snapInterval) * snapInterval;
        return new Vector3(snappedX, snappedY, position.z);
    }

    private void CreateLine(Vector3 start, Vector3 end)
    {
        // 새 GameObject 생성 및 LineRenderer 추가
        GameObject lineObject = new GameObject("Line");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        // LineRenderer 설정
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2; // 두 점으로 구성된 선
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 기본 셰이더
        lineRenderer.startColor = startlineColor;
        lineRenderer.endColor = endlineColor;

        // 생성된 선을 목록에 추가
        lines.Add(lineObject);
    }

    private void UpdateGraph(Vector3 start, Vector3 end)
    {
        // Vector3를 Vector2로 변환 (Z축 무시)
        Vector2 startPoint = new Vector2(start.x, start.y);
        Vector2 endPoint = new Vector2(end.x, end.y);

        // 시작 지점을 그래프에 추가
        if (!graph.ContainsKey(startPoint))
        {
            graph[startPoint] = new List<Vector2>();
        }

        // 끝 지점을 그래프에 추가
        if (!graph.ContainsKey(endPoint))
        {
            graph[endPoint] = new List<Vector2>();
        }

        // 양방향 연결 추가
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
        // 그래프 데이터 초기화
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

            // 그래프에 추가
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

            // 라인 생성
            //CreateLine(new Vector3(x1, y1, 0), new Vector3(x2, y2, 0));
        }

        Debug.Log("Graph loaded from CSV.");
    }

    private void OnDrawGizmos()
    {
        // LineStart와 LineEnd 설정 시 Gizmo를 표시
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

        // 그래프 연결선 표시
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
        // 생성된 모든 라인을 제거
        foreach (var line in lines)
        {
            Destroy(line);
        }
        lines.Clear();

        // 그래프 초기화
        graph.Clear();

        Debug.Log("All lines and graph cleared.");
    }
}
