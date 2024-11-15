using UnityEngine;
using Unity.Netcode;
using TMPro;
using System.Net;
using Unity.Netcode.Transports.UTP;
using Unity.VisualScripting;

public class Network : NetworkBehaviour
{
    private NetworkVariable<int> playersNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    public NetworkVariable<bool> isAllReady = new NetworkVariable<bool>(false);

    public TextMeshProUGUI playersCount;
    public TMP_InputField ipAddressInputField;
    public TMP_InputField portInputField;
    public TextMeshProUGUI hostIPDisplay;

    private bool isConnecting = false;

    public void StartHost()
    {
        NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().ConnectionData.Port = 7777;

        // Get and display the local IP
        string localIP = GetLocalIPAddress();
        if (hostIPDisplay != null) hostIPDisplay.text = "IP Address: " + localIP;

        NetworkManager.Singleton.StartHost();
    }

    public void Connecting()
    {
        isConnecting = true;
    }

    public void StartClient()
    {
        if (!isConnecting) return;
        // Get IP and port from input fields
        string localIp = ipAddressInputField.text;

        NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().ConnectionData.Port = 7777;

        // Set the IP and port for client connection
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = localIp;
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

    private string GetLocalIPAddress()
    {
        // Get the first available IP address for the local machine
        foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "127.0.0.1"; // Default to localhost if no network IP is found
    }
}