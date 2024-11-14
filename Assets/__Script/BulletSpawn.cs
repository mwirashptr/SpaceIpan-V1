using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class BulletSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    private Player _player;
    public bool hasStart = false;
    private Transform _bulletSpawnPoint;

    public float _bulletForce = 20f;

    public override void OnNetworkSpawn()
    {
        _player = GetComponentInParent<Player>();
        _bulletSpawnPoint = this.transform;
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (hasStart) return;
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        hasStart = true;
        while (true)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
            bullet.GetComponent<NetworkObject>().Spawn();
            bullet.GetComponent<Bullet>()._bulletSpawn = this;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(_bulletSpawnPoint.right * _bulletForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(_player.fireRate);
        }
    }

    public int getDamage()
    {
        return _player.damage;
    }

    public void increaseScore(int input)
    {
        _player.IncreaseScore(input);
    }
}