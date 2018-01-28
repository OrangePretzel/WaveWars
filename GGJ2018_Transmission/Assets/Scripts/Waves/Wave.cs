using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : Entity
{
    public Wavelet waveletPrefab;

    public float wavePower = 10.0f;
    public float speed = 2.0f;
    public float density = 2.0f;
    public float angle = 20;
    public int numWavelets = 5;
    public float distToWaveletSpawn = 0.2f;
    public List<Wavelet> wavelets;

    void Start()
    {
        CreateWavelets();
    }

    private void CreateWavelets()
    {
        wavelets.Clear();
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

    void Scan()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Disengage();
            CreateWavelets();
        }
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
        //Destroy(this);
    }

}
