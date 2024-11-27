using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostManager : MonoBehaviour
{
    public static HostManager Instance { get; private set; }
    public string JoinCode;
    public bool lostConnection = false;
    private int maxConnections = 5;
    private string lobbyId;
    private string hostName;
    private bool isCreatingLobby = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        hostName = DataPersistence.LoadUsername();
    }

    public async void StartHost()
    {
        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient)
        {
            Debug.Log("StartHost HostManager - Host sudah berjalan, matikan terlebih dahulu.");
            NetworkManager.Singleton.Shutdown();
        }
        if (isCreatingLobby)
        {
            Debug.LogWarning("StartHost - Lobby is already being created, please wait!");
            return;
        }

        isCreatingLobby = true;

        try
        {
            JoinCode = "...";
            Allocation allocation;

            try
            {
                allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            }
            catch (RelayServiceException ex)
            {
                Debug.Log($"StartHost - Relay create allocation failed: {ex.Message}");
                lostConnection = true;
                throw;
            }

            try
            {
                JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                Debug.Log($"Room Code: {JoinCode}");
            }
            catch (RelayServiceException ex)
            {
                Debug.LogError($"StartHost - Relay join code request failed: {ex.Message}");
                lostConnection = true;
                throw;
            }

            var relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            Debug.Log($"StartHost - server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
            Debug.Log($"StartHost - server: {allocation.AllocationId}");

            try
            {
                var createLobbyOptions = new CreateLobbyOptions
                {
                    IsPrivate = false,
                    Data = new Dictionary<string, DataObject>()
                {
                    {
                        "JoinCode", new DataObject(
                            visibility: DataObject.VisibilityOptions.Public,
                            value: JoinCode
                        )
                    }
                }
                };

                Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(JoinCode, maxConnections, createLobbyOptions);
                lobbyId = lobby.Id;
                Debug.Log("HostManager try CreateLobby - berhasil dijalankan");
                StartCoroutine(HeartbeatLobbyCoroutine(15));
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"StartHost - CreateLobbyOptions failed: {e.Message}");
                throw;
            }

            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("StartHost - Host started successfully!");
                LobbyManager lobbyManager = FindObjectOfType<LobbyManager>();
                if (lobbyManager != null)
                {
                    lobbyManager.UpdateRoomCode(JoinCode);
                }
            }
            else
            {
                Debug.LogError("StartHost - Failed to start host!");
                lostConnection = true;
                SceneManager.LoadScene("MainMenu");
            }
        }
        finally
        {
            isCreatingLobby = false; // Reset flag setelah proses selesai
        }
    }

    private IEnumerator HeartbeatLobbyCoroutine(float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            Debug.Log("HostManager HeartbeatLobbyCoroutine - berhasil dijalankan");
            yield return delay;
        }
    }

    public string GetLobbyId()
    {
        if (lobbyId != null) return lobbyId;
        else return null;
    }


    public async void DeleteLobbyAsync()
    {
        try
        {
            lobbyId ??= PlayerInfo.Instance.CurrentLobbyId;
            Debug.Log($"DeleteLobbyAsync - CurrentLobbyID Host: {lobbyId}");
            Debug.Log($"DeleteLobbyAsync - CurrentLobbyID: {PlayerInfo.Instance.CurrentLobbyId}");
            await Lobbies.Instance.DeleteLobbyAsync(lobbyId);
            Debug.Log("DeleteLobbyAsync - Successfully delete lobby.");
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"DeleteLobbyAsync - Failed delete lobby: {e.Message}");
        }
    }

    // public async void RemovePlayerFromLobby(string playerId)
    public async Task RemovePlayerFromLobby(string playerId, string currentLobbyId)
    {
        Debug.Log("RemovePlayerFromLobby - berhasil dijalankan.");
        try
        {
            Debug.Log($"RemovePlayerFromLobby Join -  LobbyID: {currentLobbyId}");
            await Lobbies.Instance.RemovePlayerAsync(currentLobbyId, playerId);
            Debug.Log("RemovePlayerFromLobby - Successfully remove player from lobby.");
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"RemovePlayerFromLobby - Failed remove player from lobby: {e.Message}");
        }
    }

    // public async Task StartHost()
    // public async void StartHost()
    // {

    //     JoinCode = "...";
    //     Allocation allocation;
    //     try
    //     {
    //         allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
    //     }
    //     catch (RelayServiceException ex)
    //     {
    //         Debug.Log($"StartHost - Relay create allocation failed: {ex.Message}");
    //         lostConnection = true;
    //         throw;
    //     }

    //     try
    //     {
    //         JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
    //         Debug.Log($"Room Code: {JoinCode}");
    //     }
    //     catch (RelayServiceException ex)
    //     {
    //         Debug.LogError($"StartHost - Relay join code request failed: {ex.Message}");
    //         lostConnection = true;
    //         throw;
    //     }

    //     var relayServerData = new RelayServerData(allocation, "dtls");
    //     NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

    //     Debug.Log($"StartHost - server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
    //     Debug.Log($"StartHost - server: {allocation.AllocationId}");

    //     try
    //     {
    //         var createLobbyOptions = new CreateLobbyOptions
    //         {
    //             IsPrivate = false,
    //             Data = new Dictionary<string, DataObject>()
    //             {
    //                 {
    //                     "JoinCode", new DataObject(
    //                         visibility: DataObject.VisibilityOptions.Member,
    //                         value: JoinCode
    //                     )
    //                 }
    //             }
    //         };

    //         Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(hostName, maxConnections, createLobbyOptions);
    //         lobbyId = lobby.Id;
    //         Debug.Log("HostManager try CreateLobby - berhasil dijalankan");
    //         StartCoroutine(HeartbeatLobbyCoroutine(15));
    //     }
    //     catch (LobbyServiceException e)
    //     {
    //         Debug.LogError($"StartHost - CreateLobbyOptions failed: {e.Message}");
    //         throw;
    //     }

    //     // codeRoomOutput.text = JoinCode;

    //     // if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient)
    //     // {
    //     //     Debug.Log("StartHost - Host sudah berjalan, matikan terlebih dahulu.");
    //     //     // DeleteLobbyAsync();
    //     //     NetworkManager.Singleton.Shutdown();
    //     // }

    //     if (NetworkManager.Singleton.StartHost())
    //     {
    //         Debug.Log("StartHost - Host started successfully!");
    //         LobbyManager lobbyManager = FindObjectOfType<LobbyManager>();
    //         if (lobbyManager != null)
    //         {
    //             lobbyManager.UpdateRoomCode(JoinCode);
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogError("StartHost - Failed to start host!");
    //         lostConnection = true;
    //         SceneManager.LoadScene("MainMenu");
    //     }
    // }    
}
