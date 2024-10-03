// Di client baru masih belum update player yang lamanya!
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    public Image[] playerProfile;
    public Sprite[] newPlayerProfile;
    public TextMeshProUGUI[] playerName;
    public Button startButton;

    void Start()
    {
        Debug.Log("LobbyManager Active");
        startButton.onClick.AddListener(OnStartButtonPressed);
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerJoined;
    }

    // Callback saat pemain bergabung
    private void OnPlayerJoined(ulong clientId)
    {
        Debug.Log($"{DataPersistence.LoadUsername()}");
        Debug.Log("Player Joined dengan Client ID: " + clientId);
        SendProfileToAllClientRpc(clientId);
        // SendPlayerNameServerRpc();
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
            playerName[i].text = $"Player {i}";
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
            Debug.Log("NetworkManager shut down successfully.");
        }

        // Kembali ke Main Menu (implementasi LoadScene sesuai dengan logika kamu)
        SceneManager.LoadScene("MainMenu");

    }


}