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
        if (playerProfile[clientId] != null) {
            playerProfile[clientId].sprite = newPlayerProfile[clientId];
        } else {
            Debug.Log($"Player Profile dengan clientID {clientId} tidak ada!");
        }
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


}