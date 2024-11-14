using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
    public BulletSpawn _bulletSpawn;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy") && !col.CompareTag("Wall")) return;
        if (!NetworkManager.Singleton.IsServer) return;
        if (col.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(_bulletSpawn.getDamage());
            if (enemy._health.Value <= 0)
            {
                _bulletSpawn.increaseScore(100);
            }
        }
        NetworkObject.Despawn();
        Destroy(gameObject);
    }
}