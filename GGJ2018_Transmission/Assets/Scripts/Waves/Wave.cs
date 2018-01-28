using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : Entity
{
    public Wavelet waveletPrefab;

    public float wavePower = 10.0f;
    public List<Wavelet> wavelets;
    public int angle = 30;
    public int numWavelets = 5;
    public float distToWaveletSpawn = 0.5f;

    void Start()
    {
        CreateWavelets();
    }

    private void CreateWavelets()
    {
        for (int i = 0; i < numWavelets; i++)
        {
            var w = Instantiate(waveletPrefab, transform);
            w.transform.parent = transform;
            wavelets.Add(w);
        }
    }

    void Scan()
    {

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
            var w = wavelets[i];
            wavelets.Remove(w);
            Destroy(w);
        }
        Destroy(this);
    }
}
