using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Image[] playerProfile;
    public Sprite[] newPlayerProfile;
    void Start()
    {
        Debug.Log("LobbyManager Active");
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerJoined;
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }


}