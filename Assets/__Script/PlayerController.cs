using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    public NetworkVariable<bool> isOn = new NetworkVariable<bool>(false);

    [SerializeField] private Rigidbody2D _rb;
    private Vector2 _movement;
    [SerializeField] private float _speed = 5f;

    public BulletSpawn bulletSpawn;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        bulletSpawn = GetComponentInChildren<BulletSpawn>();
        if (!IsOwner) return;
        _rb.freezeRotation = true;
        Vector3 spawnPosition = GetSpawnPosition();
        transform.position = spawnPosition;

        isOn.OnValueChanged += OnIsOnChanged;
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (!isOn.Value) return;
        _movement.x = Input.GetAxis("Horizontal");
        _movement.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * _speed * Time.fixedDeltaTime);
    }

    private Vector3 GetSpawnPosition()
    {
        if (OwnerClientId == 0)  // Pemain pertama
        {
            return new Vector3(-8f, 1.5f, 0f);
        }
        else  // Pemain kedua atau lainnya
        {
            return new Vector3(-8f, -1.5f, 0f);
        }
    }

    private void OnIsOnChanged(bool previousValue, bool newValue)
    {
        if (newValue && IsOwner) // Only the owner should control their movement
        {
            // Allow movement and any other controls needed to start the game
            Debug.Log("Player can now move.");
        }
    }
}