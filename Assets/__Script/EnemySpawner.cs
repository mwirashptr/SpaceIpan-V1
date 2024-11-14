using Unity.Netcode;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    public NetworkVariable<bool> isOn = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> isEnd = new NetworkVariable<bool>(false);
    [SerializeField] private float time = 0;

    public float countEnemy;

    public GameObject _Phoenix;
    public GameObject _Torik;
    public GameObject _Ipan;
    public GameObject _UpIncrease;
    public GameObject _UpFireRate;

    private void Update()
    {
        if (!IsServer) return;
        if (!isOn.Value) return;
        if (time <= 0)
        {
            if (countEnemy == 10)
            {
                SpawnPowerUpFireRate();
                time = 3;
                countEnemy++;
            }
            else if (countEnemy == 20)
            {
                SpawnPowerUpIncrease();
                time = 3;
                countEnemy++;
            }
            else if (countEnemy > 35)
            {
                isEnd.Value = true;
            }
            else if (countEnemy > 30)
            {
                SpawnIpan();
                time = 5;
                countEnemy++;
            }
            else if (countEnemy > 20)
            {
                SpawnTorik();
                time = 2;
                countEnemy++;
            }
            else
            {
                SpawnPhoenix();
                time = 1;
                countEnemy++;
            }
        }
        else
        {
            time -= Time.deltaTime;
        }
    }

    private void SpawnPhoenix()
    {
        Vector3 random = GetRandomSpawn();
        GameObject spawned = Instantiate(_Phoenix, random, Quaternion.identity);
        spawned.transform.rotation = Quaternion.Euler(0, 180, 0);
        spawned.GetComponent<NetworkObject>().Spawn();
    }

    private void SpawnTorik()
    {
        Vector3 random = GetRandomSpawn();
        GameObject spawned = Instantiate(_Torik, random, Quaternion.identity);
        spawned.GetComponent<NetworkObject>().Spawn();
    }

    private void SpawnIpan()
    {
        Vector3 random = new Vector3(10, 0.5f, 0);
        GameObject spawned = Instantiate(_Ipan, random, Quaternion.identity);
        spawned.GetComponent<NetworkObject>().Spawn();
    }

    private void SpawnPowerUpIncrease()
    {
        Vector3 random = GetRandomSpawn();
        GameObject spawned = Instantiate(_UpIncrease, random, Quaternion.identity);
        spawned.GetComponent<NetworkObject>().Spawn();
    }

    private void SpawnPowerUpFireRate()
    {
        Vector3 random = GetRandomSpawn();
        GameObject spawned = Instantiate(_UpFireRate, random, Quaternion.identity);
        spawned.GetComponent<NetworkObject>().Spawn();
    }

    public Vector3 GetRandomSpawn()
    {
        int random = Random.Range(0, 4);
        if (random == 0) return new Vector3(9.5f, 3f, 0);
        else if (random == 1) return new Vector3(9.5f, 1f, 0);
        else if (random == 2) return new Vector3(9.5f, -1f, 0);
        else return new Vector3(9.5f, -3f, 0);
    }
}