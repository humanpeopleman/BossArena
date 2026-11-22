using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FanSweepIndicator : MonoBehaviour
{
    [SerializeField] private float radius = 4f;
    [SerializeField] private float angle = 90f;
    [SerializeField] private int segments = 30;

    private Mesh mesh;

    private void Awake()
    {
        // Mesh 생성 및 초기화
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "Indicator"; // 원하는 Sorting Layer 이름으로 변경
        meshRenderer.sortingOrder = 0; // 원하는 Sorting Order 값으로 변경
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        // 부채꼴 정점 생성
        Vector3[] vertices = new Vector3[segments + 2];
        vertices[0] = Vector3.zero; // 중심점
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -angle / 2 + (angle / segments) * i;
            float rad = Mathf.Deg2Rad * currentAngle;
            vertices[i + 1] = new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0);
        }
        // 삼각형 생성
        int[] triangles = new int[segments * 3];
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }
        // Mesh에 데이터 적용
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
    private void OnDestroy()
    {
        Destroy(mesh);
    }
}