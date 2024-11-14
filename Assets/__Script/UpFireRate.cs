using Unity.Netcode;
using UnityEngine;

public class UpFireRate : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    [SerializeField] private float _fireRate;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!IsServer) return;
        Player player = other.GetComponent<Player>();
        player.ChangeFireRate(_fireRate);
        NetworkObject.Despawn();
        Destroy(this);
    }

    private void Update()
    {
        if (!IsServer) return;
        _rb.MovePosition(_rb.position + Vector2.left * _speed * Time.fixedDeltaTime);
    }
}