// Di client baru masih belum update player yang lamanya!
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    private List<string> playerNames = new();
    public Image[] playerProfile;
    public Sprite[] newPlayerProfile;
    public TextMeshProUGUI[] playerName;
    public Button startButton;
    public Button backButton;

    void Start()
    {
        Debug.Log("LobbyManager Active");
        startButton.onClick.AddListener(OnStartButtonPressed);
        backButton.onClick.AddListener(OnBackButtonPressed);
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerJoined;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerLeave;
    }

    // Callback saat pemain bergabung
    private void OnPlayerJoined(ulong clientId)
    {
        Debug.Log($"{DataPersistence.LoadUsername()}");
        // Debug.Log("Player Joined dengan Client ID: " + clientId);
        SendProfileToAllClientRpc(clientId);
        // SendPlayerNameServerRpc();
        NotifyPlayerJoinClientRpc(clientId);
    }

    private void OnPlayerLeave(ulong clientId)
    {
        NotifyPlayerLeaveClientRpc(clientId);
    }

    // [ServerRpc(RequireOwnership = false)]
    // private void SendPlayerNameServerRpc()
    // {
    //     // SendPlayerNameClientRpc(OwnerClientId, DataPersistence.LoadUsername());
    //     SendPlayerNameClientRpc(OwnerClientId);
    // }

    // [ClientRpc]
    // private void SendPlayerNameClientRpc(ulong clientId)
    // {
    //     playerName[clientId].text = $"Player {clientId}";
    //     // Debug.Log($"Player yang join bernama {playerNameJoin}");
    // }


    // Server ke semua Client (dieksekusi di client)
    [ClientRpc]
    private void SendProfileToAllClientRpc(ulong clientId)
    {
        int totalPlayer = (int)clientId;
        for (int i = 0; i <= totalPlayer; i++)
        {
            playerName[i].text = $"Player {i + 1}";
            if (playerProfile[i] != null)
            {
                playerProfile[i].sprite = newPlayerProfile[i];
                Debug.Log($"Profile player {clientId} telah diubah");
            }
            else
            {
                Debug.Log($"Player Profile dengan clientID {clientId} tidak ada!");
            }
        }
    }

    public void OnStartButtonPressed()
    {
        Debug.Log("Tombol start berhasil ditekan!");
        if (IsHost)
        {
            LoadGamePlaySceneServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void LoadGamePlaySceneServerRpc()
    {
        NetworkManager.SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
    }

    public void OnBackButtonPressed()
    {
        // Shutdown NetworkManager ketika kembali ke Main Menu
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            Debug.Log("NetworkManager in Host shut down successfully.");
            ReturnToMainMenuClientRpc();
        }
        else if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
            Debug.Log("NetworkManager in Client shut down successfully.");
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Server membuat semua client berpindah ke MainMenu
    [ClientRpc]
    private void ReturnToMainMenuClientRpc()
    {
        SceneManager.LoadScene("MainMenu");
    }

    [ClientRpc]
    private void NotifyPlayerJoinClientRpc(ulong clientId)
    {
        Debug.Log($"JOIN Player ID: {clientId}");
    }

    [ClientRpc]
    private void NotifyPlayerLeaveClientRpc(ulong clientId)
    {
        Debug.Log($"LEAVE Player ID: {clientId}");
    }

}