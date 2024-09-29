using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using System.Collections.Generic;

public class HostTrial : MonoBehaviour
{
    public static HostTrial Instance;
    private string relayCode;
    private List<string> playerNames = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start the host and create a Relay lobby
    public async void StartHost()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(5);  // Max 5 players
        relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        // Set up the host using Netcode
        NetworkManager.Singleton.StartHost();
    }

    public string GetRelayCode()
    {
        return relayCode;
    }

    // Add player names to the lobby list
    public void AddPlayer(string playerName)
    {
        playerNames.Add(playerName);
        LobbyManager.Instance.UpdatePlayerList();
    }

    public List<string> GetPlayerNames()
    {
        return playerNames;
    }
}
