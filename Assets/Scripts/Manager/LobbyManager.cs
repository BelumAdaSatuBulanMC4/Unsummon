using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    public Image[] playerProfile;
    public Sprite[] newPlayerProfile;
    public Button startButton;
    void Start()
    {
        Debug.Log("LobbyManager Active");
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerJoined;
        startButton.onClick.AddListener(OnStartButtonPressed);
    }

    // Callback saat pemain bergabung
    private void OnPlayerJoined(ulong clientId)
    {
        Debug.Log("Player Joined dengan Client ID: " + clientId);
        SendImageToAllClientRpc(clientId);
    }

    // Server ke semua Client (dieksekusi di client)
    [ClientRpc]
    private void SendImageToAllClientRpc(ulong clientId) {
        // if (playerProfile[clientId] != null) {
        //     playerProfile[clientId].sprite = newPlayerProfile[clientId];
        // } else {
        //     Debug.Log($"Player Profile dengan clientID {clientId} tidak ada!");
        // }
        // if (OwnerClientId == clientId) {
        //     int totalPlayer = (int)clientId;
        //     for (int i = 0; i < totalPlayer-1; i++) {
        //         playerProfile[clientId].sprite = newPlayerProfile[clientId];
        //         Debug.Log($"Profile player {clientId} telah diubah");
        //     }
        // }
        int totalPlayer = (int)clientId;
        for (int i = 0; i <= totalPlayer; i++) {            
            if (playerProfile[i] != null) {
                playerProfile[i].sprite = newPlayerProfile[i];
                Debug.Log($"Profile player {clientId} telah diubah");
            } else {
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
    
    public void OnBackButtonPressed() {
        // Shutdown NetworkManager ketika kembali ke Main Menu
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost) {
            NetworkManager.Singleton.Shutdown();
            Debug.Log("NetworkManager shut down successfully.");
        }

        // Kembali ke Main Menu (implementasi LoadScene sesuai dengan logika kamu)
        SceneManager.LoadScene("MainMenu");
    }


}