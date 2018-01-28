using UnityEngine;
using System.Collections;

public class WaveEmitter : Entity
{
    public float wavePower = 10.0f;

    public Wave wavePrefab;
    public Wave wave = null;
    public WaveEmitter childEmitter = null;


    void Start()
    {
        Transmit();
    }

    void Update()
    {
        
    }

    void Transmit()
    {
        if (wave == null)
        {
            wave = Instantiate(wavePrefab, transform);
            wave.transform.parent = transform;
        }
    }

    void StopTransmit()
    {
        if (wave != null)
        {
            wave.transform.parent = null;
            wave.Disengage();
            wave = null;
        }
    }

    public void UpdateWaveAngle(int newAngle)
    {
        wave?.UpdateWaveAngle(newAngle);
    }
}
