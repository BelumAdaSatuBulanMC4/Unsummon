using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class HostManager : MonoBehaviour
{
    public static HostManager Instance { get; private set; }
    public string JoinCode { get; private set; }

    private void Awake() {
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public async void StartHost() {
        Allocation allocation;
        try {
            allocation = await RelayService.Instance.CreateAllocationAsync(5);
        } catch(Exception e) {
            Debug.Log($"Relay create allocation failed {e.Message}");
            throw;
        }

        try {
            JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log($"Room Code: {JoinCode}");
        } catch {
            Debug.Log($"Relay join code request failed");
            throw;
        }

        var relayServerData = new RelayServerData(allocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"server: {allocation.AllocationId}");

        // codeRoomOutput.text = JoinCode;

        if (NetworkManager.Singleton.StartHost()) {
            Debug.Log("Host started successfully!");
            LobbyDisplay lobbyDisplay = FindObjectOfType<LobbyDisplay>();
            if (lobbyDisplay != null) {
                lobbyDisplay.UpdateRoomCode(JoinCode); // Memperbarui codeRoomOutput di LobbyDisplay
            }
        } else {
            Debug.LogError("Failed to start host!");
        }
    }
}
