using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class GamePlayManager : NetworkBehaviour
{
    public GameObject playerKidPrefab;
    public GameObject playerPocongPrefab;
    public GameObject CandlePrefab; // Ensure this is assigned in the Inspector

    private Vector3 spawnKidPosition = new Vector3(0, 0, 0); // PlayerKid spawn position
    private Vector3 spawnPocongPosition = new Vector3(0, 0, 0); // Pocong spawn position

    public Vector3[] spawnPoints;
    private Vector3[] selectedPoints;
    // Fungsi yang dipanggil setelah scene GamePlay di-load
    private void Start()
    {
        spawnPoints = new Vector3[20];
        spawnPoints[0] = new Vector3(10, 0, 0);
        spawnPoints[1] = new Vector3(20, 0, 0);
        spawnPoints[2] = new Vector3(30, 0, 0);
        spawnPoints[3] = new Vector3(40, 0, 0);
        spawnPoints[4] = new Vector3(50, 0, 0);
        spawnPoints[5] = new Vector3(60, 0, 0);
        spawnPoints[6] = new Vector3(70, 0, 0);
        spawnPoints[7] = new Vector3(80, 0, 0);
        spawnPoints[8] = new Vector3(90, 0, 0);
        spawnPoints[9] = new Vector3(100, 10, 0);
        spawnPoints[10] = new Vector3(10, 10, 0);
        spawnPoints[11] = new Vector3(20, 10, 0);
        spawnPoints[12] = new Vector3(30, 10, 0);
        spawnPoints[13] = new Vector3(40, 10, 0);
        spawnPoints[14] = new Vector3(50, 10, 0);
        spawnPoints[15] = new Vector3(60, 10, 0);
        spawnPoints[16] = new Vector3(70, 10, 0);
        spawnPoints[17] = new Vector3(80, 10, 0);
        spawnPoints[18] = new Vector3(90, 10, 0);
        spawnPoints[19] = new Vector3(100, 10, 0);

        selectedPoints = GetRandomPoints(spawnPoints, 10);
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnPlayers();
            // SpawnItems();
        }
    }

    void SpawnItems()
    {
        if (CandlePrefab == null)
        {
            Debug.LogError("CandlePrefab is not assigned in the GamePlayManager.");
            return;
        }

        for (int i = 0; i < 10; i++)
        {
            // Instantiate the candle at the selected spawn points
            GameObject spawnedItem = Instantiate(CandlePrefab, selectedPoints[i], Quaternion.identity);

            // Ensure that it has a NetworkObject and spawn it for network synchronization
            NetworkObject networkObject = spawnedItem.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.Spawn();  // Spawns the object across the network for all clients
            }
            else
            {
                Debug.LogError("Spawned item does not have a NetworkObject component attached.");
            }
        }
    }

    Vector3[] GetRandomPoints(Vector3[] points, int numberOfPoints)
    {
        // Create a list from the original array for shuffling
        List<Vector3> pointsList = new List<Vector3>(points);

        // Shuffle the list using Fisher-Yates algorithm
        for (int i = pointsList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Vector3 temp = pointsList[i];
            pointsList[i] = pointsList[randomIndex];
            pointsList[randomIndex] = temp;
        }

        // Create an array to store the selected points
        Vector3[] selectedPoints = new Vector3[numberOfPoints];

        // Copy the first 'numberOfPoints' elements from the shuffled list to the new array
        for (int i = 0; i < numberOfPoints; i++)
        {
            selectedPoints[i] = pointsList[i];
        }

        return selectedPoints; // Return the array of selected points
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

