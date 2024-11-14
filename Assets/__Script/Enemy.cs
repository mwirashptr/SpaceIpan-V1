using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    public NetworkVariable<int> _health = new NetworkVariable<int>(100);
    private NetworkVariable<int> _currentHealth = new NetworkVariable<int>(100);
    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private int _speed = 8;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer) return;
        _rb.freezeRotation = true;
        _currentHealth = _health;
    }

    private void Update()
    {
        if (!IsServer) return;
        _rb.MovePosition(_rb.position + Vector2.left * _speed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (!IsServer) return;
        _currentHealth.Value -= damage;
        if (_currentHealth.Value <= 0)
        {
            NetworkObject.Despawn();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Wall")) return;
        if (!IsServer) return;
        NetworkObject.Despawn();
        Destroy(this);
    }
}