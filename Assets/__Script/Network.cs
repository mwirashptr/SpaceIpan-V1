using UnityEngine;
using Unity.Netcode;
using TMPro;

public class Network : NetworkBehaviour
{
    private NetworkVariable<int> playersNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    public NetworkVariable<bool> isAllReady = new NetworkVariable<bool>(false);

    public TextMeshProUGUI playersCount;

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void GetReady()
    {
        if (!IsServer) return;
        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            isAllReady.Value = true;
        }
    }

    private void Update()
    {
        playersCount.text = "Players Count: " + playersNum.Value.ToString();
        if (!IsServer) return;
        playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }

    public void DefaultReady()
    {
        isAllReady.Value = false;
    }
}