using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Tile9Mesh : MonoBehaviour {

    public float width = 1;
    public float height = 1;

    [Range(0, 0.5f)]
    public float uvLeft = 0.2f;
    [Range(0, 0.5f)]
    public float uvRight = 0.2f;
    [Range(0, 0.5f)]
    public float uvTop = 0.2f;
    [Range(0, 0.5f)]
    public float uvBottom = 0.2f;
    public float uvToWorldScaleX = 1;
    public float uvToWorldScaleY = 1;

    private Mesh mesh;

    private Vector3[] vertices;
    private Vector2[] uv;

    private void Start() {
        vertices = new Vector3[16];
        uv = new Vector2[16];

        mesh = new Mesh {
            name = "Tile9Mesh"
        };
        FillGeometry();
        FillMesh();
        mesh.triangles = new[] {
            0, 1, 12, 0, 12, 11,
            1, 2, 13, 1, 13, 12,
            2, 3, 4, 2, 4, 13,
            13, 4, 5, 13, 5, 14,
            14, 5, 6, 14, 6, 7,
            15, 14, 7, 15, 7, 8,
            10, 15, 8, 10, 8, 9,
            11, 12, 15, 11, 15, 10,
            12, 13, 14, 12, 14, 15
        };
        RecalculateMesh();

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    public void UpdateMesh() {
        if (mesh != null) {
            FillGeometry();
            FillMesh();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
    }

    private void FillGeometry() {
        {
            float w = width;
            float h = height;
            float l = uvLeft * uvToWorldScaleX;
            float r = width - uvRight * uvToWorldScaleX;
            float t = height - uvTop * uvToWorldScaleY;
            float b = uvBottom * uvToWorldScaleY;
            vertices[0] = new Vector3(0, 0, 0);
            vertices[1] = new Vector3(0, b, 0);
            vertices[2] = new Vector3(0, t, 0);
            vertices[3] = new Vector3(0, h, 0);
            vertices[4] = new Vector3(l, h, 0);
            vertices[5] = new Vector3(r, h, 0);
            vertices[6] = new Vector3(w, h, 0);
            vertices[7] = new Vector3(w, t, 0);
            vertices[8] = new Vector3(w, b, 0);
            vertices[9] = new Vector3(w, 0, 0);
            vertices[10] = new Vector3(r, 0, 0);
            vertices[11] = new Vector3(l, 0, 0);
            vertices[12] = new Vector3(l, b, 0);
            vertices[13] = new Vector3(l, t, 0);
            vertices[14] = new Vector3(r, t, 0);
            vertices[15] = new Vector3(r, b, 0);
        }
        {
            const float w = 1;
            const float h = 1;
            float l = uvLeft;
            float r = 1 - uvRight;
            float t = 1 - uvTop;
            float b = uvBottom;
            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(0, b);
            uv[2] = new Vector2(0, t);
            uv[3] = new Vector2(0, h);
            uv[4] = new Vector2(l, h);
            uv[5] = new Vector2(r, h);
            uv[6] = new Vector2(w, h);
            uv[7] = new Vector2(w, t);
            uv[8] = new Vector2(w, b);
            uv[9] = new Vector2(w, 0);
            uv[10] = new Vector2(r, 0);
            uv[11] = new Vector2(l, 0);
            uv[12] = new Vector2(l, b);
            uv[13] = new Vector2(l, t);
            uv[14] = new Vector2(r, t);
            uv[15] = new Vector2(r, b);
        }
    }

    private void FillMesh() {
        mesh.vertices = vertices;
        mesh.uv = uv;
    }

    private void RecalculateMesh() {
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

#if UNITY_EDITOR
    private void OnValidate() {
        if (mesh != null) {
            UpdateMesh();
        }
    }
#endif


}