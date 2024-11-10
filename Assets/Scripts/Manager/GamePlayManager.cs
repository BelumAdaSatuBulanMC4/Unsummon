using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.Netcode.Transports.UTP;

public class GamePlayManager : NetworkBehaviour
{
    [SerializeField] private GameObject[] playerKidPrefabs;
    public GameObject playerKidPrefab;
    public GameObject playerPocongPrefab;
    private Vector3 spawnKidPosition = new(-40f, -10f, 0); // Posisi Spawn playerKid
    private Vector3 spawnPocongPosition = new(-36f, -14f, 0); // Posisi Spawn playerKid

    [SerializeField] GameObject roleKidScene;
    [SerializeField] GameObject rolePocongScene;

    [Header("Error")]
    [SerializeField] private GameObject UI_PopUpLostConnection;

    private int totalPreviousPlayer;
    private int totalCurrentPlayer;

    [SerializeField] private Button leaveButton;
    private int totalPreviousPersonalLeave = 0;
    private int totalCurrentPersonalLeave = 0;
    private bool personalLeave = false;

    private void Start()
    {
        leaveButton.onClick.AddListener(OnLeaveButtonPressed);
        if (IsHost)
        {
            totalPreviousPlayer = NetworkManager.Singleton.ConnectedClientsList.Count;
            // UpdateTotalPlayersClientRpc(totalPreviousPlayer);
        }
        NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerLeave;
    }

    private void OnLeaveButtonPressed()
    {
        Debug.Log($"OnLeaveButtonPressed - Terdapat player yang sengaja AFK");
        SetPersonalLeaveServerRpc(true);
        // AddPersonalLeaveServerRpc();
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("MainMenu");
        // SetLeavePersonalServerRpc(false);
    }

    private void Update()
    {
        totalCurrentPlayer = NetworkManager.Singleton.ConnectedClientsList.Count;
        Debug.Log($"Update - Jumlah player sebelumnya: {totalPreviousPlayer}");
        Debug.Log($"Update - Jumlah player sekarang: {totalCurrentPlayer}");
        if (IsHost)
        {
            if (personalLeave)
            {
                totalPreviousPlayer--;
                personalLeave = false;
            }
            // if (totalPreviousPersonalLeave > totalCurrentPersonalLeave)
            // {
            //     totalPreviousPersonalLeave = totalCurrentPersonalLeave;
            //     totalPreviousPlayer--;
            // }
            // if (totalPreviousPlayer != totalCurrentPlayer)
            if (totalPreviousPlayer > totalCurrentPlayer)
            {
                Debug.Log($"Update - CLIENT DISCONNECT");
                // ReturnToMainMenuClientRpc();
                totalPreviousPlayer = totalCurrentPlayer;
                NotifyPlayerClientRpc();
                StartCoroutine(WaitingReturnToMainMenu(7f));
            }
        }
    }

    private void OnPlayerLeave(ulong clientId)
    {
        Debug.Log($"OnPlayerLeave - Player leave dengan ID: {clientId}");
        Debug.Log($"OnPlayerLeave - OwnerClientID: {OwnerClientId}");
        Debug.Log($"OnPlayerLeave - IsHost: {IsHost}");
    }

    // Fungsi yang dipanggil setelah scene GamePlay di-load
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnPlayers();
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
    }

    // Spawn semua pemain di posisi yang sama
    private void SpawnPlayers()
    {
        if (!IsServer) return;
        int totalPlayer = NetworkManager.ConnectedClients.Count;
        int randomPocongId = Random.Range(0, totalPlayer);
        int i = 0;
        foreach (var client in NetworkManager.ConnectedClientsList)
        {
            Debug.Log($"Previous player: {totalPreviousPlayer}");
            Debug.Log($"Current player: {totalCurrentPlayer}");
            Debug.Log($"Total player: {totalPlayer}");
            // if (i == randomPocongId && totalPlayer != 1)
            if ((int)client.ClientId == 0)
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
            }
            else
            {
                GameObject playerInstance = Instantiate(playerKidPrefab, spawnKidPosition, Quaternion.identity);
                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
                // Panggil ClientRpc hanya untuk client ini
                ShowRoleKidSceneClientRpc(new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { client.ClientId }
                    }
                });
            }
            i++;
        }
    }

    // ====================================== START COUROTINE ======================================
    private IEnumerator DisableSceneAfterDelay(GameObject scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        scene.SetActive(false);
    }


    private IEnumerator WaitingReturnToMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToMainMenuClientRpc();
    }

    // ====================================== SERVER RPC ======================================
    [ServerRpc]
    private void AddPersonalLeaveServerRpc()
    {
        totalCurrentPersonalLeave++;
    }

    [ServerRpc]
    private void SetPersonalLeaveServerRpc(bool isPersonalLeave)
    {
        personalLeave = isPersonalLeave;
    }

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

    // Server membuat semua client berpindah ke MainMenu
    [ClientRpc]
    private void ReturnToMainMenuClientRpc()
    {
        SceneManager.LoadScene("MainMenu");
    }

    [ServerRpc(RequireOwnership = false)]
    private void NotifyPlayerServerRpc()
    {
        NotifyPlayerClientRpc();
    }

    [ClientRpc]
    private void NotifyPlayerClientRpc()
    {
        Debug.Log("CLIENT DAPET NOTIF ADA YG DISCONNECT");
        UI_PopUpLostConnection.SetActive(true);
    }

    // [ClientRpc]
    // private void UpdateTotalPlayersClientRpc(int totalPlayers)
    // {
    //     totalPreviousPlayer = totalPlayers;
    // }

}
