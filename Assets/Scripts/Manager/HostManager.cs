using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class HostManager : MonoBehaviour
{
    public static HostManager Instance { get; private set; }
    public string JoinCode;
    public bool lostConnection = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // public async Task StartHost()
    public async void StartHost()
    {
        JoinCode = "...";
        Allocation allocation;
        try
        {
            allocation = await RelayService.Instance.CreateAllocationAsync(5);
        }
        catch (RelayServiceException ex)
        {
            Debug.Log($"Relay create allocation failed {ex.Message}");
            lostConnection = true;
            throw;
        }

        try
        {
            JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log($"Room Code: {JoinCode}");
        }
        catch (RelayServiceException ex)
        {
            Debug.Log($"Relay join code request failed: {ex.Message}");
            lostConnection = true;
            throw;
        }

        var relayServerData = new RelayServerData(allocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        Debug.Log($"StartHost - server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"StartHost - server: {allocation.AllocationId}");

        // codeRoomOutput.text = JoinCode;

        // if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient)
        // {
        //     Debug.Log("StartHost - Host sudah berjalan, matikan terlebih dahulu.");
        //     NetworkManager.Singleton.Shutdown();
        // }

        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("StartHost - Host started successfully!");
            LobbyManager lobbyManager = FindObjectOfType<LobbyManager>();
            if (lobbyManager != null)
            {
                lobbyManager.UpdateRoomCode(JoinCode);
            }
        }
        else
        {
            Debug.LogError("StartHost - Failed to start host!");
            lostConnection = true;
        }
    }
}
