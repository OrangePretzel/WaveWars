using UnityEngine;
using System.Collections;

public class Wavelet: MonoBehaviour
{
    public float wavePower = 10.0f;
    public float maxLength = 0.0f;
    public float length = 0.0f;
    public float speed = 0.5f;
    public float scrollSpeed = -0.1f;
    public float waveDensity = 2.0f;

    public float angle = 10.0f;
    public float baseWidth = 0.3f;

    private MeshFilter mf;
    private Mesh mesh;
    public Vector3[] vertices;
    public Vector2[] uvs;
    private float spreadRatio;
    private float halfWidth;

    void Start()
    {
        // Find the half angle, convert to radians, and use tan() to get the ratio of
        // of length to width.
        spreadRatio = Mathf.Tan(Mathf.Deg2Rad * (angle / 2.0f));
        halfWidth = baseWidth / 2.0f;
        maxLength = wavePower;

        mf = GetComponent<MeshFilter>();
        mesh = new Mesh();
        vertices = new Vector3[] { new Vector3(-halfWidth, 0, 0),
                                   new Vector3(halfWidth, 0, 0),
                                   new Vector3(-halfWidth, 0, 0),
                                   new Vector3(halfWidth, 0, 0) };
        uvs = new Vector2[] { new Vector2(0, 0),
                              new Vector2(1, 0),
                              new Vector2(0, 1),
                              new Vector2(1, 1) };
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = new int[] {0, 2, 1, 1, 2, 3};

        mf.mesh = mesh;
    }

    void Update()
    {
        SetWaveLength(maxLength); // For testing only!

        length = Mathf.Clamp(length + (speed * Time.deltaTime), 0.0f, maxLength);

        vertices[2].y = length;
        vertices[2].x = -halfWidth - (length * spreadRatio);
        vertices[3].y = length;
        vertices[3].x = halfWidth + (length * spreadRatio);

        mesh.vertices = vertices;

        if (length != maxLength)
        {
            TileUVs(length);
        }
        else
        {
            ScrollUVs();
        }
    }

    void SetWaveLength(float newMaxLength)
    {
        maxLength = Mathf.Clamp(newMaxLength, 0.0f, wavePower);

        if (maxLength < length)
        {
            length = maxLength;

            uvs[2].y = uvs[0].y + (length * waveDensity);
            uvs[3].y = uvs[1].y + (length * waveDensity);
        }
    }

    void TileUVs(float length)
    {
        uvs[0].y = uvs[2].y - (length * waveDensity);
        uvs[1].y = uvs[3].y - (length * waveDensity);

        mesh.uv = uvs;
    }

    void ScrollUVs()
    {
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i].y = uvs[i].y - (speed * Time.deltaTime) * 2f;
        }
        mesh.uv = uvs;
    }
}
