using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<int> playersCount = new NetworkVariable<int>(0);

    [SerializeField] private Canvas NetworkUI;
    [SerializeField] private Canvas GameUI;
    [SerializeField] private Canvas Leaderboard;

    public Player p1;
    public Player p2;

    [SerializeField] private Spawner spawner;
    private bool isStarted = false;

    private void Update()
    {
        if (!IsServer) return;

        playersCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
        if (playersCount.Value < 2) return;

        // Check if the game should start or if we should show the leaderboard
        if (spawner.isEnd.Value)
        {
            ActivateLeaderboardClientRpc();
        }
        else if (NetworkUI.GetComponent<Network>().isAllReady.Value)
        {
            StartGame();
            NetworkUI.GetComponent<Network>().DefaultReady();
        }
    }

    private void StartGame()
    {
        if (isStarted) return;

        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            var playerController = client.Value.PlayerObject.GetComponent<PlayerController>();
            playerController.isOn.Value = true;
            playerController.bulletSpawn.gameObject.SetActive(true);
        }

        AssignPlayers();
        spawner.isOn.Value = true;

        // Call ClientRpc to activate GameUI on all clients
        ActivateGameUIClientRpc();
        isStarted = true;
    }

    private void AssignPlayers()
    {
        // Ensure players are assigned
        var connectedClients = NetworkManager.Singleton.ConnectedClients;
        if (connectedClients.Count > 1)
        {
            p1 = connectedClients[0].PlayerObject.GetComponent<Player>();
            p2 = connectedClients[1].PlayerObject.GetComponent<Player>();
        }
    }

    [ClientRpc]
    private void ActivateGameUIClientRpc()
    {
        NetworkUI.gameObject.SetActive(false);
        GameUI.gameObject.SetActive(true);
    }

    [ClientRpc]
    private void ActivateLeaderboardClientRpc()
    {
        GameUI.gameObject.SetActive(false);
        Leaderboard.gameObject.SetActive(true);
    }

    public Player GetPlayer(int index)
    {
        // Check player index and return p1 or p2
        if (index == 1) return p1;
        else if (index == 2) return p2;
        else return null;
    }
}