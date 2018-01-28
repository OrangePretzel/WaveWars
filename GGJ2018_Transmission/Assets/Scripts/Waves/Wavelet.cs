using UnityEngine;
using System.Collections;

public class Wavelet: MonoBehaviour
{
    private float wavePower = 10.0f;
    private float maxLength = 0.0f;
    private float length = 0.0f;
    private float speed = 0.5f;
    private int waveDensity = 2;

    private float angle = 4.0f;
    private float baseWidth = 0.3f;

    private float phase = 0.0f;
    private float tiling = 0.0f;
    private MeshFilter mf;
    private Mesh mesh;
    public Vector3[] vertices;
    public Vector2[] uvs;
    private float spreadRatio;
    private float halfWidth;
    private float leftover;
    public LayerMask collisionLayer;

    void Start()
    {
        

    }

    public void Initialize(float power, float width, float angle, float speed, int density)
    {
        // Find the half angle, convert to radians, and use tan() to get the ratio of
        // of length to width.
        spreadRatio = Mathf.Tan(Mathf.Deg2Rad * (angle / 2.0f));
        baseWidth = width;
        halfWidth = baseWidth / 2.0f;
        this.angle = angle;
        wavePower = power;
        maxLength = power;
        this.speed = speed;
        waveDensity = density;


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
        mesh.triangles = new int[] { 0, 2, 1, 1, 2, 3 };

        mf.mesh = mesh;
    }

    void Update()
    {
        phase += speed * Time.deltaTime;

        var hit = Physics2D.Raycast(transform.position, transform.up, wavePower, collisionLayer.value);

        if (hit.collider != null)
        {
            SetWaveLength(hit.distance);
        }
        else
        {
            SetWaveLength(wavePower);
        }
        length = Mathf.Clamp(length + (speed * Time.deltaTime) + 0.05f, 0.0f, maxLength);

        tiling = (int)length * waveDensity;
        leftover = (length - (int)length) * waveDensity;

        vertices[2].y = length;
        vertices[2].x = -halfWidth - (length * spreadRatio);
        vertices[3].y = length;
        vertices[3].x = halfWidth + (length * spreadRatio);

        mesh.vertices = vertices;

        if (phase >= 1.0f)
        {
            phase = phase - 1.0f;
        }

        if (length < maxLength)
        {
            uvs[2].y = 1 + leftover;
            uvs[3].y = 1 + leftover;
        }
        uvs[2].y = 1 + leftover - (phase * waveDensity);
        uvs[3].y = 1 + leftover - (phase * waveDensity);
        uvs[0].y = 1 - tiling - (phase * waveDensity);
        uvs[1].y = 1 - tiling - (phase * waveDensity);

        mesh.uv = uvs;
        
    }

    void SetWaveLength(float newMaxLength)
    {
        maxLength = Mathf.Clamp(newMaxLength, 0.0f, wavePower);
    }

}
