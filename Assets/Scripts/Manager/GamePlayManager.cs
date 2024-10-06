using UnityEngine;
using Unity.Netcode;

public class GamePlayManager : NetworkBehaviour
{
    public GameObject playerKidPrefab;
    public GameObject playerPocongPrefab;
    public GameObject playerSpiritPrefab;
    private Vector3 spawnKidPosition = new(0, 0, 0); // Posisi Spawn playerKid
    private Vector3 spawnPocongPosition = new(0, 0, 0); // Posisi Spawn playerKid

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
            if ((int)client.ClientId == randomPocongId)
            // if ((int)client.ClientId == 0)
            // if (false)
            {
                GameObject playerInstance = Instantiate(playerPocongPrefab, spawnPocongPosition, Quaternion.identity);
                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
            }
            else
            {
                GameObject playerInstance = Instantiate(playerKidPrefab, spawnKidPosition, Quaternion.identity);
                // GameObject playerInstance = Instantiate(playerSpiritPrefab, spawnKidPosition, Quaternion.identity);
                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
            }

        }
    }
}

