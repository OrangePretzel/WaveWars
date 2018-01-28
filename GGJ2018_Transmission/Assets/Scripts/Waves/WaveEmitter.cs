using UnityEngine;
using System.Collections;

public class WaveEmitter : Entity
{
    public float wavePower = 10.0f;

    public Wave wavePrefab;
    public Wave wave = null;
    public WaveEmitter childEmitter = null;

    private bool transmitting = false;


    void Start()
    {
        //Transmit();
    }


    public void Transmit()
    {
        if (!transmitting)
        {
            if (wave == null)
            {
                wave = Instantiate(wavePrefab, transform);
                wave.transform.parent = transform;
                wave.SetEntityProperties(teamID, playerID, reflective);
                wave.SetWavePower(wavePower);
                wave.CreateWavelets();
            }
        }
        wave?.Scan();
        transmitting = true;
    }

    public void StopTransmit()
    {
        if (wave != null)
        {
            wave.transform.parent = null;
            wave.Disengage();
            Destroy(wave.gameObject);
            wave = null;
            transmitting = false;
        }
    }

    public void UpdateWaveAngle(int newAngle)
    {
        wave?.UpdateWaveAngle(newAngle);
    }
}
