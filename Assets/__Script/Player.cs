using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<int> health = new NetworkVariable<int>(100);

    public NetworkVariable<int> score = new NetworkVariable<int>(0);

    public int damage = 40;
    public float fireRate = 0.5f;

    public void TakeDamage(int damage)
    {
        if (!IsServer) return;
        health.Value -= damage;
        if (health.Value <= 0)
        {
            //Die
        }
    }

    public void ChangeFireRate(float input)
    {
        if (!IsServer) return;
        fireRate = input;
    }

    public void ChangeDamage(int input)
    {
        if (!IsServer) return;
        damage = input;
    }

    public void IncreaseScore(int input)
    {
        if (!IsServer) return;
        score.Value += input;
    }
}