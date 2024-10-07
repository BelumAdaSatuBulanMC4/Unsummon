using System;
using System.Linq;

// using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    // private List<string> playerNames = new();
    private readonly NetworkList<ulong> connectedClients = new();

    // public Image[] playerProfile;
    // public Sprite[] newPlayerProfile;
    public TextMeshProUGUI[] playerName;
    // [SerializeField] private GameObject[] playerWaiting;
    [SerializeField] private GameObject[] playerJoin;
    public Button startButton;
    public GameObject startButtonObject;
    public Button backButton;

    public TextMeshProUGUI codeRoomOutput;

    void Start()
    {
        Debug.Log("LobbyManager Active");
        startButton.onClick.AddListener(OnStartButtonPressed);
        backButton.onClick.AddListener(OnBackButtonPressed);

        // if (IsServer)
        // {
        //     NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerJoined;
        //     NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerLeave;
        // }
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerJoined;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerLeave;
        connectedClients.OnListChanged += OnConnectedClientsChanged;
    }
    override public void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerJoined;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnPlayerLeave;
        }
        connectedClients.OnListChanged -= OnConnectedClientsChanged;
        connectedClients?.Dispose(); // Ini hanya jika Anda secara manual menginisialisasi connectedClients
    }

    public void UpdateRoomCode(string roomCode)
    {
        codeRoomOutput.text = roomCode;
    }

    // Callback saat pemain bergabung
    private void OnPlayerJoined(ulong clientId)
    {
        if (!IsServer) { startButtonObject.SetActive(false); }
        string roomCode = PlayerPrefs.GetString("RoomCode", "NoCode");
        UpdateRoomCode(roomCode);

        // Debug.Log($"ROOM CODE: {roomCode}");
        // Debug.Log("PLAYER JOIN");
        // Debug.Log($"{DataPersistence.LoadUsername()}");
        if (IsServer)
        {
            Debug.Log($"Client {clientId} connected.");
            connectedClients.Add(clientId);
        }
        // Debug.Log("Player Joined dengan Client ID: " + clientId);
        // SendProfileToAllClientRpc(clientId);
        // SendPlayerNameServerRpc();
        NotifyPlayerJoinClientRpc(clientId);
    }

    private void OnPlayerLeave(ulong clientId)
    {
        if (IsServer)
        {
            Debug.Log($"Client {clientId} disconnected.");
            connectedClients.Remove(clientId);
        }
        NotifyPlayerLeaveClientRpc(clientId);
    }

    // Callback untuk merespon perubahan pada NetworkList
    private void OnConnectedClientsChanged(NetworkListEvent<ulong> changeEvent)
    {
        // Di sini kita bisa bereaksi terhadap perubahan di NetworkList
        switch (changeEvent.Type)
        {
            case NetworkListEvent<ulong>.EventType.Add:
                Debug.Log($"Client {changeEvent.Value} has joined.");
                break;

            case NetworkListEvent<ulong>.EventType.Remove:
                Debug.Log($"Client {changeEvent.Value} has left.");
                break;
        }

        // Debug: cetak seluruh daftar client yang terhubung
        Debug.Log("Current connected clients:");
        foreach (var client in connectedClients)
        {
            Debug.Log($"- ClientID: {client}");
        }
        Debug.Log(connectedClients.Count);
        for (int i = 0; i <= 4; i++)
        {
            playerJoin[i].SetActive(false);
            playerName[i].text = "Waiting";
        }
        for (int i = 1; i <= connectedClients.Count; i++)
        {
            playerJoin[i - 1].SetActive(true);
            playerName[i - 1].text = $"Player {i}";
        }
    }


    // Server ke semua Client (dieksekusi di client)
    // [ClientRpc]
    // private void SendProfileToAllClientRpc(ulong clientId)
    // {
    //     int totalPlayer = (int)clientId;
    //     for (int i = 0; i <= totalPlayer; i++)
    //     {
    //         playerName[i].text = $"Player {i + 1}";
    //         if (playerProfile[i] != null)
    //         {
    //             playerProfile[i].sprite = newPlayerProfile[i];
    //             Debug.Log($"Profile player {clientId} telah diubah");
    //         }
    //         else
    //         {
    //             Debug.Log($"Player Profile dengan clientID {clientId} tidak ada!");
    //         }
    //     }
    // }

    public void OnStartButtonPressed()
    {
        Debug.Log("Tombol start berhasil ditekan!");
        if (IsHost)
        {
            LoadGamePlaySceneServerRpc();
        }
    }

    public void OnBackButtonPressed()
    {
        // Shutdown NetworkManager ketika kembali ke Main Menu
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            Debug.Log("NetworkManager in Host shut down successfully.");
            try
            {
                ReturnToMainMenuClientRpc();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return;
            }
        }
        else if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
            Debug.Log("NetworkManager in Client shut down successfully.");
            SceneManager.LoadScene("MainMenu");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void LoadGamePlaySceneServerRpc()
    {
        NetworkManager.SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
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