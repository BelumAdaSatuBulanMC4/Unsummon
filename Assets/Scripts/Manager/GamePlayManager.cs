using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayManager : NetworkBehaviour
{
    public GameObject playerKidPrefab;
    public GameObject playerPocongPrefab;
    private Vector3 spawnKidPosition = new(0, 0, 0); // Posisi Spawn playerKid
    private Vector3 spawnPocongPosition = new(0, 0, 0); // Posisi Spawn playerKid

    [SerializeField] private Button homeButton;
    [SerializeField] private Button playAgainButton;

    private void Start()
    {
        homeButton.onClick.AddListener(OnHomeButtonPressed);
        playAgainButton.onClick.AddListener(OnPlayAgainButtonPressed);
    }

    private void OnHomeButtonPressed()
    {
        if (IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            ReturnToMainMenuClientRpc();
        }
        else if (IsClient)
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnPlayAgainButtonPressed()
    {
        SceneManager.LoadScene("LobbyRoom");
    }

    // Fungsi yang dipanggil setelah scene GamePlay di-load
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnPlayers();
        }
    }

    // Spawn semua pemain di posisi yang sama
    private void SpawnPlayers()
    {
        if (!IsServer) return;
        int randomPocongId = Random.Range(0, 3);
        foreach (var client in NetworkManager.ConnectedClientsList)
        {
            // Debug.Log($"Player: {client.ClientId}");
            // if ((int)client.ClientId == randomPocongId)
            if ((int)client.ClientId == 0)
            // if (false)
            {
                GameObject playerInstance = Instantiate(playerPocongPrefab, spawnPocongPosition, Quaternion.identity);
                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
            }
            else
            {
                GameObject playerInstance = Instantiate(playerKidPrefab, spawnKidPosition, Quaternion.identity);
                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
            }

        }
    }

    // Server membuat semua client berpindah ke MainMenu
    [ClientRpc]
    private void ReturnToMainMenuClientRpc()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

