using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public static ClientManager Instance { get; private set; }
    public TMP_InputField codeRoomInput;

    private void Awake() {
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public async void StartClient() {

        JoinAllocation allocation;
        try {
            allocation = await RelayService.Instance.JoinAllocationAsync(codeRoomInput.text);
        } catch {
            Debug.LogError("Relay get join code request failed");
            throw;
        }
        var relayServerData = new RelayServerData(allocation, "dtls");

        Debug.Log("Trying to connect to: " + codeRoomInput.text);
        
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        // Debug. Log($"client: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        // Debug. Log($"host: {allocation. HostConnectionData[0]} {allocation.HostConnectionData[1]}");
        // Debug. Log($"client: {allocation.AllocationId}");

        if (NetworkManager.Singleton.StartClient()) {
            Debug.Log("Client connect successfully!");
            LobbyDisplay lobbyDisplay = FindObjectOfType<LobbyDisplay>();
            if (lobbyDisplay != null) {
                lobbyDisplay.UpdateRoomCode(codeRoomInput.text); // Memperbarui codeRoomOutput di LobbyDisplay
            }
        } else {
            Debug.LogError("Failed to start client!");
        }
    }
}
