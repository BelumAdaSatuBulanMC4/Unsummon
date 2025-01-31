using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class GamePlayManager : NetworkBehaviour
{
    public static GamePlayManager instance;
    public GameObject playerKidPrefab;
    public GameObject playerPocongPrefab;
    private Vector3 spawnKidPosition = new(22.45f, -23.59f, 0);
    private Vector3 spawnPocongPosition = new(28.19f, -24.32f, 0);

    [SerializeField] GameObject roleKidScene;
    [SerializeField] GameObject rolePocongScene;

    [Header("Error")]
    [SerializeField] private GameObject UI_OtherPlayerLeave;

    private int totalPreviousPlayer;
    private int totalCurrentPlayer;

    [SerializeField] private Button leaveButton;
    private readonly List<PlayerCharacter> playerCharacterList = new();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        leaveButton.onClick.AddListener(OnLeaveButtonPressed);
        HostManager.Instance.DeleteLobbyAsync();
    }

    private void OnLeaveButtonPressed()
    {
        HostManager.Instance.DeleteLobbyAsync();
        Debug.Log($"OnLeaveButtonPressed - Terdapat player yang sengaja AFK");
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        totalCurrentPlayer = NetworkManager.Singleton.ConnectedClientsList.Count;
        Debug.Log($"Update - Jumlah player sebelumnya: {totalPreviousPlayer}");
        Debug.Log($"Update - Jumlah player sekarang: {totalCurrentPlayer}");
    }

    private void OnPlayerLeave(ulong clientId)
    {
        HandlePlayerLeave(clientId);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerLeave;
            SpawnPlayers();
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnPlayerLeave;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnPlayerLeave;
        }
    }

    private void SpawnPlayers()
    {
        if (!IsServer) return;
        int totalPlayer = NetworkManager.ConnectedClients.Count;
        int randomPocongId = UnityEngine.Random.Range(0, totalPlayer);
        int i = 0;
        foreach (var client in NetworkManager.ConnectedClientsList)
        {
            Debug.Log($"Previous player: {totalPreviousPlayer}");
            Debug.Log($"Current player: {totalCurrentPlayer}");
            Debug.Log($"Total player: {totalPlayer}");
            if (i == randomPocongId && totalPlayer != 1)
            // if ((int)client.ClientId == 0)
            // if (false)
            {
                ShowRolePocongSceneClientRpc(new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { client.ClientId }
                    }
                });

                GameObject playerInstance = Instantiate(playerPocongPrefab, spawnPocongPosition, Quaternion.identity);
                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
                AddPlayerCharacterListClientRpc(client.ClientId, CharacterType.Pocong);
            }
            else
            {
                GameObject playerInstance = Instantiate(playerKidPrefab, spawnKidPosition, Quaternion.identity);
                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
                ShowRoleKidSceneClientRpc(new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { client.ClientId }
                    }
                });
                AddPlayerCharacterListClientRpc(client.ClientId, CharacterType.Kid);
            }
            i++;
        }
    }


    private void HandlePlayerLeave(ulong clienId)
    {
        if (!IsHost) return;
        Debug.Log("HandlePlayerLeave - Berhasil dijalanlan");
        RemovePlayerCharacterListClientRpc(clienId);
    }

    public void DebugTesting()
    {
        Debug.Log("DebugTesting - berhasil dijalankan");
    }


    // ====================================== COUROTINE ======================================
    private IEnumerator DisableSceneAfterDelay(GameObject scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        scene.SetActive(false);
    }


    // ====================================== SERVER RPC ======================================


    // ====================================== CLIENT RPC ======================================
    [ClientRpc]
    private void ShowRolePocongSceneClientRpc(ClientRpcParams clientRpcParams = default)
    {
        rolePocongScene.SetActive(true);
        StartCoroutine(DisableSceneAfterDelay(rolePocongScene, 15f));
    }

    [ClientRpc]
    private void ShowRoleKidSceneClientRpc(ClientRpcParams clientRpcParams = default)
    {
        roleKidScene.SetActive(true);
        StartCoroutine(DisableSceneAfterDelay(roleKidScene, 5f));
    }


    [ClientRpc]
    private void AddPlayerCharacterListClientRpc(ulong clientId, CharacterType typeChar)
    {
        PlayerCharacter player = new(clientId, typeChar);
        playerCharacterList.Add(player);
        Debug.Log($"AddPlayerCharacterListClienRpc - Player berhasil ditambahkan {clientId} tipe: {typeChar}");
    }

    [ClientRpc]
    public void UpdatePlayerCharacterListClientRpc(ulong clientId, CharacterType typeChar)
    {
        try
        {
            PlayerCharacter player = playerCharacterList.Find(p => p.clientId == clientId);
            playerCharacterList.Remove(player);
            PlayerCharacter newPlayer = new(clientId, typeChar);
            playerCharacterList.Add(newPlayer);
            Debug.Log($"UpdatePlayerCharacterListClientRpc - Player ID {clientId} berhasil diperbarui menjadi tipe: {typeChar}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"UpdatePlayerCharacterListClientRpc - Player gagal diupdate: {ex.Message}.");
        }

    }


    [ClientRpc]
    private void RemovePlayerCharacterListClientRpc(ulong clientId)
    {
        try
        {
            PlayerCharacter player = playerCharacterList.Find(player => player.clientId == clientId);
            if (player.typeChar == CharacterType.Kid)
            {
                Debug.Log($"RemovePlayerCharacterListClientRpc - Player adalah Kid");
                GameManager.instance.totalKids--;
            }
            else if (player.typeChar == CharacterType.Pocong)
            {
                Debug.Log($"RemovePlayerCharacterListClientRpc - Player adalah Pocong");
                SceneManager.LoadScene("MainMenu");
            }
            else if (player.typeChar == CharacterType.Spirit)
            {
                Debug.Log($"RemovePlayerCharacterListClientRpc - Player adalah Spirit");
            }
            playerCharacterList.Remove(player);
            UI_OtherPlayerLeave.SetActive(true);
            Debug.Log($"RemovePlayerCharacterListClientRpc - Player berhasil dihapus ID: {clientId}.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"RemovePlayerCharacterListClientRpc - Player gagal dihapus: {ex.Message}.");

        }

    }


}

public struct PlayerCharacter : INetworkSerializable
{
    public ulong clientId;
    public CharacterType typeChar;

    public PlayerCharacter(ulong id, CharacterType typeCharParams)
    {
        clientId = id;
        typeChar = typeCharParams;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref typeChar);
    }
}

public enum CharacterType
{
    Pocong, Kid, Spirit
}