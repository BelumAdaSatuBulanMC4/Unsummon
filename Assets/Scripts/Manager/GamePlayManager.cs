using UnityEngine;
using Unity.Netcode;

public class GamePlayManager : NetworkBehaviour
{
    public GameObject playerPrefab; // Drag and drop PlayerPrefab di inspector
    public Vector3 spawnPosition = new(0, 0, 0); // Posisi spawn yang sama untuk semua pemain

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
        foreach (var client in NetworkManager.ConnectedClientsList)
        {
            Debug.Log($"Player: {client.ClientId}");
            GameObject playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
        }
    }
}

