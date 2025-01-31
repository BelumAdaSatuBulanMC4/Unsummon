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
    [SerializeField] private Button backForceButton;
    [SerializeField] private GameObject backButtonObject;
    [SerializeField] private TextMeshProUGUI codeRoomOutput;
    [SerializeField] private GameObject waitingHostStart;
    [SerializeField] private Color enabledColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color disabledColor = new Color(1f, 1f, 1f, 0.5f);
    private int totalPreviousPlayer;
    private int totalCurrentPlayer;

    private bool serverHasJoin = false;

    private void Start()
    {
        Debug.Log("LobbyManager Active");
        startButton.onClick.AddListener(OnStartButtonPressed);
        backButton.onClick.AddListener(OnBackButtonPressed);
        backForceButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerJoined;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerLeave;
        if (IsHost)
        {
            totalPreviousPlayer = NetworkManager.Singleton.ConnectedClientsList.Count;
        }
    }

    private void Update()
    {
        if (IsHost)
        {
            if (SceneManager.GetActiveScene().name == "LobbyRoomTesting")
            {
                EnableButton(startButton);
            }
            else
            {
                totalCurrentPlayer = NetworkManager.Singleton.ConnectedClientsList.Count;
                if (totalCurrentPlayer >= 3)
                {
                    EnableButton(startButton);
                }
            }
        }
        if (totalPreviousPlayer != totalCurrentPlayer)
        {
            Debug.Log($"Update - Jumlah player sebelumnya: {totalPreviousPlayer}");
            Debug.Log($"Update - Jumlah player sekarang: {totalCurrentPlayer}");
            Debug.Log($"Update - total player sebelum dan sesudah diubah");
            totalPreviousPlayer = totalCurrentPlayer;
            UpdatePlayerUI();
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            SceneManager.LoadScene("MainMenu");
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
            waitingHostStart.SetActive(true);
        }
        if (IsServer)
        {
            loadingScreen.SetActive(false);
            startButtonObject.SetActive(true);
            DisableButton(startButton);
            backButtonObject.SetActive(true);
            Debug.Log($"OnPlayerJoined - Username server: {DataPersistence.LoadUsername()}");
            UpdateRoomCodeClientRpc(codeRoomOutput.text);
            SendPlayerDataToNewClient(clientId);
        }

        if (IsHost && !serverHasJoin)
        {
            string username = DataPersistence.LoadUsername();
            Debug.Log($"OnPlayerJoined - Username SERVER: {username}");
            PlayerData newPlayer = new(clientId, username);
            AddPlayerServerRpc(newPlayer);
            serverHasJoin = true;
        }

        if (IsClient && !IsServer)
        {
            string username = DataPersistence.LoadUsername();
            Debug.Log($"OnPlayerJoined - Username CLIENT: {username}");
            PlayerData newPlayer = new(clientId, username);
            AddPlayerServerRpc(newPlayer);
        }

    }

    private void OnPlayerLeave(ulong clientId)
    {
        if (IsServer)
        {
            Debug.Log($"OnPlayerLeave - Client {clientId} disconnected.");
            RemovePlayerClientRpc(clientId);
        }
    }

    public void OnStartButtonPressed()
    {
        if (IsHost)
        {
            LoadGamePlaySceneServerRpc();
        }
    }

    public async void OnBackButtonPressed()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost)
        {
            HostManager.Instance.DeleteLobbyAsync();
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
            if (PlayerInfo.Instance.CurrentLobbyId != null)
            {
                await HostManager.Instance.RemovePlayerFromLobby(PlayerInfo.Instance.PlayerId, PlayerInfo.Instance.CurrentLobbyId);

            }
            NetworkManager.Singleton.Shutdown();
            Debug.Log("OnBackButtonPressed - NetworkManager in Client shut down successfully.");
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void UpdateRoomCode(string roomCode)
    {
        if (codeRoomOutput != null)
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
            if (playerJoin[i] == null) continue;
            playerJoin[i].SetActive(false);
            playerName[i].text = "Waiting";
        }
        for (int i = 0; i < playerDataList.Count; i++)
        {
            if (playerJoin[i] == null) continue;
            playerJoin[i].SetActive(true);
            if (playerDataList[i].clientId == NetworkManager.Singleton.LocalClientId)
            {
                playerName[i].text = "You";
            }
            else
            {
                playerName[i].text = playerDataList[i].username;
            }
        }
    }


    private void SendPlayerDataToNewClient(ulong clientId)
    {
        SendPlayerDataListClientRpc(playerDataList.ToArray(), new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { clientId }
            }
        });
    }

    // ====================================== SERVER RPC ======================================
    [ServerRpc(RequireOwnership = false)]
    private void LoadGamePlaySceneServerRpc()
    {
        if (SceneManager.GetActiveScene().name == "LobbyRoomTesting")
        {
            NetworkManager.SceneManager.LoadScene("GamePlayKid", LoadSceneMode.Single);
            Debug.Log("scene: " + SceneManager.GetActiveScene().name);
        }
        else
        {
            NetworkManager.SceneManager.LoadScene("GamePlayNew", LoadSceneMode.Single);
            Debug.Log("else scene: " + SceneManager.GetActiveScene().name);
        }
        Debug.Log("luar scene: " + SceneManager.GetActiveScene().name);

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
        foreach (var player in existingPlayers)
        {
            playerDataList.Add(player);
        }
        UpdatePlayerUI();
    }

    // ==== HELPER ===
    private void EnableButton(Button button)
    {
        button.interactable = true;
        button.GetComponent<Image>().color = enabledColor;
    }

    private void DisableButton(Button button)
    {
        button.interactable = false;
        button.GetComponent<Image>().color = disabledColor;
    }




}

public struct PlayerData : INetworkSerializable
{
    public ulong clientId;
    public string username;

    public PlayerData(ulong id, string name)
    {
        clientId = id;
        username = name;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref username);
    }
}
