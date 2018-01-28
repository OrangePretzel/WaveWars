using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Entity
{
    public new Rigidbody2D rigidbody;
    public float speed;
    private bool isTransmitting;
    private PlayerInput playerInput;
    private WaveEmitter waveEmitter;

    public float MaxCharge = 10;

    [SerializeField]
    private float charge;

    private Vector3 playerPosition;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        charge = MaxCharge;
        isTransmitting = false;
        playerInput = InputManager.GetPlayerInput(PlayerID);
        waveEmitter = GetComponentInChildren<WaveEmitter>();
        waveEmitter?.SetEntityProperties(teamID, playerID, reflective);
    }

    void Update()
    {
        if (GameManager.IsGamePaused)
            return;

        playerInput = InputManager.GetPlayerInput(playerID);
        rigidbody.velocity = new Vector2(playerInput.HorizontalMovement * speed, playerInput.VerticalMovement * speed);
        rigidbody.MovePosition(rigidbody.position + rigidbody.velocity * Time.deltaTime);
        playerPosition = this.transform.position;

        HandleTransmission();
    }

    void HandleTransmission()
    {
        var aimVec = playerInput.GetNormalizedAim(transform.position, Camera.main);
        isTransmitting = aimVec.sqrMagnitude != 0
            && (playerInput.PlayerDevice != InputManager.KEYBOARD_AND_MOUSE || Input.GetKey(KeyCode.Mouse0));

        if (isTransmitting && charge > 0)
        {
            charge -= Time.deltaTime;

            ShootTransmission(aimVec);
            Debug.DrawRay(playerPosition, aimVec, Color.yellow, 5.0f, true);
        }

        if (!isTransmitting || charge <= 0)
        {
            waveEmitter?.StopTransmit();
        }

        if (!isTransmitting && charge < MaxCharge)
        {
            charge += Time.deltaTime;
            if (charge >= MaxCharge)
                charge = MaxCharge;
        }
    }

    private void ShootTransmission(Vector3 direction)
    {
        if (waveEmitter)
        {
            var q = new Quaternion();
            q.SetLookRotation(Vector3.forward, direction);
            waveEmitter.transform.localRotation = q;
            waveEmitter.Transmit();
        }
    }
}
