using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyManager : NetworkBehaviour
{
    private readonly List<PlayerData> playerDataList = new();

    [SerializeField] private TextMeshProUGUI[] playerName;
    [SerializeField] private GameObject[] playerJoin;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject startButtonObject;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject backButtonObject;
    [SerializeField] private TextMeshProUGUI codeRoomOutput;
    private int totalPreviousPlayer;
    private int totalCurrentPlayer;

    private bool serverHasJoin = false;

    private void Start()
    {
        Debug.Log("LobbyManager Active");
        startButton.onClick.AddListener(OnStartButtonPressed);
        backButton.onClick.AddListener(OnBackButtonPressed);
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerJoined;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerLeave;
        if (IsHost)
        {
            totalPreviousPlayer = NetworkManager.Singleton.ConnectedClientsList.Count;
        }
    }

    private void Update()
    {
        // totalCurrentPlayer = NetworkManager.Singleton.ConnectedClientsList.Count;
        if (IsHost)
        {
            totalCurrentPlayer = NetworkManager.Singleton.ConnectedClientsList.Count;
        }
        if (totalPreviousPlayer != totalCurrentPlayer)
        {
            Debug.Log($"Update - Jumlah player sebelumnya: {totalPreviousPlayer}");
            Debug.Log($"Update - Jumlah player sekarang: {totalCurrentPlayer}");
            Debug.Log($"Update - total player sebelum dan sesudah diubah");
            totalPreviousPlayer = totalCurrentPlayer;
        }
    }

    override public void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerJoined;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnPlayerLeave;
        }
    }

    private void OnPlayerJoined(ulong clientId)
    {
        Debug.Log($"OnPlayerJoined - IsOwner: {IsOwner}");
        Debug.Log($"OnPlayerJoined - IsClient: {IsClient}");
        Debug.Log($"OnPlayerJoined - IsHost: {IsHost}");
        Debug.Log($"OnPlayerJoined - IsServer: {IsServer}");
        Debug.Log($"OnPlayerJoined - OwnerClientId: {OwnerClientId}");
        Debug.Log($"OnPlayerJoined - IsLocalPlayer: {IsLocalPlayer}");
        Debug.Log($"OnPlayerJoined - IsLocalPlayer: {IsLocalPlayer}");
        if (!IsServer)
        {
            loadingScreen.SetActive(false);
            backButtonObject.SetActive(true);
        }
        if (IsServer)
        {
            loadingScreen.SetActive(false);
            startButtonObject.SetActive(true);
            backButtonObject.SetActive(true);
            Debug.Log($"OnPlayerJoined - Username server: {DataPersistence.LoadUsername()}");
            UpdateRoomCodeClientRpc(codeRoomOutput.text);
            SendPlayerDataToNewClient(clientId);
            // yg ke add data si servernya lagi tp pake id si client
            // string username = DataPersistence.LoadUsername(); 
            // PlayerData newPlayer = new(clientId, username);
            // AddPlayerClientRpc(newPlayer);
        }

        // Hanya dieksekusi oleh host saat pertama kali join
        if (IsHost && !serverHasJoin)
        {
            string username = DataPersistence.LoadUsername();
            Debug.Log($"OnPlayerJoined - Username SERVER: {username}");
            PlayerData newPlayer = new(clientId, username);
            AddPlayerServerRpc(newPlayer);
            serverHasJoin = true; // yg update si server doang klo dia udh gabung
        }

        // Hanya dieksekusi oleh client yang baru bergabung
        if (IsClient && !IsServer)
        {
            string username = DataPersistence.LoadUsername();
            Debug.Log($"OnPlayerJoined - Username CLIENT: {username}");
            PlayerData newPlayer = new(clientId, username);
            AddPlayerServerRpc(newPlayer);
        }

        // NotifyPlayerJoinClientRpc(clientId);
    }

    private void OnPlayerLeave(ulong clientId)
    {
        if (IsServer)
        {
            Debug.Log($"OnPlayerLeave - Client {clientId} disconnected.");
            RemovePlayerClientRpc(clientId);
        }
        // NotifyPlayerLeaveClientRpc(clientId);
    }

    public void OnStartButtonPressed()
    {
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
            Debug.Log("OnBackButtonPressed - NetworkManager in Host shut down successfully.");
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
            Debug.Log("OnBackButtonPressed - NetworkManager in Client shut down successfully.");
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void UpdateRoomCode(string roomCode)
    {
        codeRoomOutput.text = roomCode.ToLower();
    }

    private void UpdatePlayerUI()
    {
        Debug.Log("Current connected clients:");
        foreach (var client in playerDataList)
        {
            Debug.Log($"- ClientID: {client.clientId} Name: {client.username}");
        }
        for (int i = 0; i < playerJoin.Length; i++)
        {
            playerJoin[i].SetActive(false);
            playerName[i].text = "Waiting";
        }
        for (int i = 0; i < playerDataList.Count; i++)
        {
            playerJoin[i].SetActive(true);
            // playerName[i].text = $"Player {i + 1}";
            playerName[i].text = playerDataList[i].username.ToString(); ;
        }
    }

    private void SendPlayerDataToNewClient(ulong clientId)
    {
        // Kirim seluruh playerDataList ke client yang baru
        SendPlayerDataListClientRpc(playerDataList.ToArray(), new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { clientId } // Targetkan hanya client yang baru
            }
        });
    }

    // ====================================== SERVER RPC ======================================
    [ServerRpc(RequireOwnership = false)]
    private void LoadGamePlaySceneServerRpc()
    {
        NetworkManager.SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddPlayerServerRpc(PlayerData newPlayer)
    {
        AddPlayerClientRpc(newPlayer);
    }



    // ====================================== CLIENT RPC ======================================
    [ClientRpc]
    private void ReturnToMainMenuClientRpc()
    {
        SceneManager.LoadScene("MainMenu");
    }

    [ClientRpc]
    private void UpdateRoomCodeClientRpc(string codeRoom)
    {
        codeRoomOutput.text = codeRoom.ToLower();
    }

    [ClientRpc]
    private void AddPlayerClientRpc(PlayerData newPlayer)
    {
        Debug.Log($"AddPlayerClientRpc - ClientID: {newPlayer.clientId} Name: {newPlayer.username}");
        playerDataList.Add(newPlayer);
        UpdatePlayerUI();
    }

    [ClientRpc]
    private void RemovePlayerClientRpc(ulong clientId)
    {
        for (int i = 0; i < playerDataList.Count; i++)
        {
            if (playerDataList[i].clientId == clientId)
            {
                playerDataList.RemoveAt(i);
                break;
            }
        }
        UpdatePlayerUI();
    }

    [ClientRpc]
    private void SendPlayerDataListClientRpc(PlayerData[] existingPlayers, ClientRpcParams clientRpcParams = default)
    {
        // Tambahkan semua pemain yang sudah ada di server ke client yang baru
        foreach (var player in existingPlayers)
        {
            playerDataList.Add(player);
        }
        UpdatePlayerUI();
    }




}

public struct PlayerData : INetworkSerializable
{
    public ulong clientId;
    public string username;

    // Constructor to initialize the struct
    public PlayerData(ulong id, string name)
    {
        clientId = id;
        username = name;
    }

    // Implementasi INetworkSerializable
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref username);
    }
}
