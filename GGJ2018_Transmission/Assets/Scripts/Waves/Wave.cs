using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : Entity
{
    public Wavelet waveletPrefab;

    private float wavePower = 10.0f;
    public float speed = 2.0f;
    public int density = 2;
    public float angle = 20;
    public int numWavelets = 5;
    public float distToWaveletSpawn = 0.2f;
    public List<Wavelet> wavelets;
    private float cosHalfAngle;
    public LayerMask collisionLayer;

    void Start()
    {
        //CreateWavelets();
    }

    public void CreateWavelets()
    {
        wavelets.Clear();
        cosHalfAngle = Mathf.Cos(Mathf.Deg2Rad * (angle + 4) / 2);
        float waveletAngle = angle / numWavelets;
        float spawnAngle = (-angle / 2) + (waveletAngle / 2);
        float arcLength = distToWaveletSpawn * Mathf.Deg2Rad * angle;
        float waveletWidth = arcLength / numWavelets;

        for (int i = 0; i < numWavelets; i++)
        {
            var w = Instantiate(waveletPrefab, transform);
            w.transform.Rotate(Vector3.forward, -spawnAngle);
            w.transform.Translate(distToWaveletSpawn * Mathf.Sin(Mathf.Deg2Rad * spawnAngle),
                                  distToWaveletSpawn * Mathf.Cos(Mathf.Deg2Rad * spawnAngle),
                                  0.0f, transform);
            w.Initialize(wavePower, waveletWidth, waveletAngle, speed, density);
            wavelets.Add(w);

            spawnAngle += waveletAngle;
        }
    }

    public void SetWavePower(float power)
    {
        wavePower = power;
    }

    public void Scan()
    {
        var friendlies = GameManager.GetFriendlyEntities(TeamID);

        foreach (var friend in friendlies)
        {
            var v = friend.transform.position - transform.position;
            if (v.magnitude < wavePower)
            {
                if (Vector3.Dot(v.normalized, transform.up) > cosHalfAngle)
                {
                    var hit = Physics2D.Raycast(transform.position, v.normalized, wavePower, collisionLayer.value);
                    //Debug.Log(hit.collider);
                    //Debug.DrawRay(transform.position, v.normalized * hit.distance);
                    if (!hit.collider || v.magnitude < hit.distance)
                    {
                        var minion = friend as Minion;
                        minion.AffectByWave(transform.up, playerID);
                    }
                }
            }
        }
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            Disengage();
            CreateWavelets();
        }*/
    }

    public void UpdateWaveAngle(int newAngle)
    {
        angle = newAngle;

        // add subtract wavelets and sync their phases
    }


    public void Disengage()
    {
        // For testing. Later this should just prepare the wave to collapse.
        for (int i = 0; i < wavelets.Count; i++)
        {
            Destroy(wavelets[i].gameObject);
        }
        wavelets.Clear();
    }

}
