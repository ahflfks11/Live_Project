using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleRangeVisualizer : MonoBehaviour
{
    float radius;
    public int segments = 50; // 원을 그리기 위한 세그먼트 수
    private LineRenderer lineRenderer;
    UnitData _data;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = segments + 1; // 원을 완성하기 위해 시작 점과 끝 점을 연결
        _data = transform.GetComponent<UnitData>();
        radius = _data.FindRange;
        ClearCircle();
    }

    public void DrawCircle()
    {
        lineRenderer.positionCount = segments + 1; // 원을 완성하기 위해 시작 점과 끝 점을 연결
        float angle = 0f;
        Vector3 scale = transform.localScale;

        for (int i = 0; i < segments + 1; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius / scale.x;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius / scale.y;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += (360f / segments);
        }
    }

    public void ClearCircle()
    {
        lineRenderer.positionCount = 0;
    }
}