using UnityEngine;
using Unity.Netcode;

public class GamePlayManager : NetworkBehaviour
{
    public GameObject playerKidPrefab;
    public GameObject playerPocongPrefab;
    private Vector3 spawnKidPosition = new(0, 0, 0); // Posisi Spawn playerKid
    private Vector3 spawnPocongPosition = new(1, 1, 0); // Posisi Spawn playerKid

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
            // Debug.Log($"Player: {client.ClientId}");
            // int randomPocongId = Random.Range(0, 5);
            // if ((int)client.ClientId == randomPocongId)
            // if ((int)client.ClientId == 0)
            if (false)
            {
                GameObject playerInstance = Instantiate(playerPocongPrefab, spawnPocongPosition, Quaternion.identity);
                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
            }
            else
            {
                GameObject playerInstance = Instantiate(playerKidPrefab, spawnKidPosition, Quaternion.identity);
                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
            }
            // GameObject playerInstance = Instantiate(playerKidPrefab, spawnKidPosition, Quaternion.identity);
            // playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);

        }
    }
}

